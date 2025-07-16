// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================

//<snippetSystemUserProvider>
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;

// This namespace is found in the Microsoft.Crm.Sdk.Proxy.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Crm.Sdk.Messages;

namespace Microsoft.Crm.Sdk.Samples
{
    /// <summary>
    /// This class contains methods which retrieve the IDs of several fictitious Microsoft Dynamics
    /// CRM system users.  Several SDK samples require these additional user accounts in order to run.
    /// </summary>
    /// <remarks>For On-premises and IFD deployments, if these users do not exist they are created in
    /// Active Directory. This assumes that the system user account under which the application runs has
    /// system administrator privileges. Since it is not possible to programmatically create user accounts
    /// in Windows Live ID, when running this code against a Microsoft Dynamics CRM Online server, you will have
    /// to manually add these users.</remarks>
    public class SystemUserProvider
    {
        public static Guid RetrieveSalesManager(OrganizationServiceProxy proxy)
        {
            String ldapPath = String.Empty;
            return RetrieveSystemUser("kcook", "Kevin", "Cook", "Sales Manager", proxy, ref ldapPath);
        }
        public static Guid RetrieveSalesManager(OrganizationServiceProxy proxy, ref String ldapPath)
        {
            return RetrieveSystemUser("kcook", "Kevin", "Cook", "Sales Manager", proxy, ref ldapPath);
        }

        public static List<Guid> RetrieveSalespersons(OrganizationServiceProxy proxy, ref String ldapPath)
        {
            List<Guid> reps = new List<Guid>();

            reps.Add(RetrieveSystemUser("nanderson", "Nancy", "Anderson", "Salesperson", proxy, ref ldapPath));
            reps.Add(RetrieveSystemUser("dbristol", "David", "Bristol", "Salesperson", proxy, ref ldapPath));

            return reps;
        }

        public static List<Guid> RetrieveDelegates(OrganizationServiceProxy proxy,
            ref String ldapPath)
        {
            List<Guid> delegates = new List<Guid>();

            delegates.Add(RetrieveSystemUser("dwilson", "Dan", "Wilson", "Delegate", proxy, ref ldapPath));
            delegates.Add(RetrieveSystemUser("canderson", "Christen", "Anderson", "Delegate", proxy, ref ldapPath));
            return delegates;
        }

        public static Guid RetrieveVPSales(OrganizationServiceProxy proxy, ref String ldapPath)
        {
            return RetrieveSystemUser("mtucker", "Michael", "Tucker", "Vice President of Sales", proxy, ref ldapPath);
        }

        public static Guid RetrieveMarketingManager(OrganizationServiceProxy proxy)
        { 
            String ldapPath = String.Empty;
            return RetrieveMarketingManager(proxy, ref ldapPath);
        }

        public static Guid RetrieveMarketingManager(OrganizationServiceProxy proxy, ref String ldapPath)
        {
            return RetrieveSystemUser("ssmith", "Samantha", "Smith", "Marketing Manager", proxy, ref ldapPath);
        }

        /// <summary>
        /// Retrieves the requested SystemUser record.  If the record does not exist, a new
        /// Microsoft Dynamics CRM SystemUser record is created and an associated Active
        /// Directory account is created, if it doesn't currently exist.
        /// </summary>
        /// <param name="userName">The username field as set in Microsoft Dynamics CRM</param>
        /// <param name="firstName">The first name of the system user to be retrieved</param>
        /// <param name="lastName">The last name of the system user to be retrieved</param>
        /// <param name="roleStr">The string representing the Microsoft Dynamics CRM security
        /// role for the user</param>
        /// <param name="serviceProxy">The OrganizationServiceProxy object to your Microsoft
        /// Dynamics CRM environment</param>
        /// <param name="ldapPath">The LDAP path for your network - you can either call
        /// ConsolePromptForLDAPPath() to prompt the user or provide a value in code</param>
        /// <returns></returns>
        public static Guid RetrieveSystemUser(String userName, String firstName,
            String lastName, String roleStr, OrganizationServiceProxy serviceProxy,
            ref String ldapPath)
        {
            String domain;
            Guid userId = Guid.Empty;

            if (serviceProxy == null)
                throw new ArgumentNullException("serviceProxy");

            if (String.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("UserName");

            if (String.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException("FirstName");

            if (String.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException("LastName");

            if (String.IsNullOrWhiteSpace(roleStr))
                throw new ArgumentNullException("Role");

            // Obtain the current user's information.
            WhoAmIRequest who = new WhoAmIRequest();
            WhoAmIResponse whoResp = (WhoAmIResponse)serviceProxy.Execute(who);
            Guid currentUserId = whoResp.UserId;

            SystemUser currentUser =
                serviceProxy.Retrieve(SystemUser.EntityLogicalName, currentUserId, new ColumnSet("domainname")).ToEntity<SystemUser>();

            // Extract the domain and create the LDAP object.
            String[] userPath = currentUser.DomainName.Split(new char[] { '\\' });
            if (userPath.Length > 1)
                domain = userPath[0] + "\\";
            else
                domain = String.Empty;

            // Create the system user in Microsoft Dynamics CRM if the user doesn't 
            // already exist.
            QueryExpression userQuery = new QueryExpression
            {
                EntityName = SystemUser.EntityLogicalName,
                ColumnSet = new ColumnSet("systemuserid"),
                Criteria =
                {
                    FilterOperator = LogicalOperator.Or,
                    Filters =
                    {   
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression("domainname", ConditionOperator.Equal, domain + userName)
                            }
                        },
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions = 
                            {
                                new ConditionExpression("firstname", ConditionOperator.Equal, firstName),
                                new ConditionExpression("lastname", ConditionOperator.Equal, lastName)
                            }
                        }
                    }

                }
            };

            DataCollection<Entity> existingUsers = (DataCollection<Entity>)serviceProxy.RetrieveMultiple(userQuery).Entities;

            SystemUser existingUser = null;
            if (existingUsers.Count > 0)
                existingUser = existingUsers[0].ToEntity<SystemUser>();

            if (existingUser != null)
            {
                userId = existingUser.SystemUserId.Value;

                // Check to make sure the user is assigned the correct role.
                Role role = RetrieveRoleByName(serviceProxy, roleStr);

                // Associate the user with the role when needed.
                if (!UserInRole(serviceProxy, userId, role.Id))
                {
                    AssociateRequest associate = new AssociateRequest()
                    {
                        Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                        RelatedEntities = new EntityReferenceCollection()
                        {
                            new EntityReference(Role.EntityLogicalName, role.Id)
                        },
                        Relationship = new Relationship("systemuserroles_association")
                    };
                    serviceProxy.Execute(associate);
                }
            }
            else
            {
                userId = CreateSystemUser(userName, firstName, lastName, domain,
                    roleStr, serviceProxy, ref ldapPath);
            }            

            return userId;
        }

        private static Guid CreateSystemUser(String userName, String firstName,
            String lastName, String domain, String roleStr,
            OrganizationServiceProxy serviceProxy, ref String ldapPath)
        {
            // Check to make sure this is not Microsoft Dynamics CRM Online.
            if (serviceProxy.ServiceConfiguration.AuthenticationType == AuthenticationProviderType.LiveId)
                throw new Exception(String.Format("To run this sample, {0} {1} must be an active system user in your Microsoft Dynamics CRM Online organization.", firstName, lastName));
            if(String.IsNullOrEmpty(ldapPath))
                ldapPath = SystemUserProvider.ConsolePromptForLDAPPath();            
            Guid userId = Guid.Empty;

            // Create an Active Directory user account if it doesn't exist already.
            if (String.IsNullOrEmpty(ldapPath))
                throw new ArgumentException("Required argument ldapPath was not provided.");
            DirectoryEntry directoryEntry = new DirectoryEntry(ldapPath);
            DirectoryEntry userADAccount = null;

            // Search AD to see if the user already exists.
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = String.Format("(sAMAccountName={0})", userName);
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (result == null)
            {
                // Create the Active Directory account. 
                userADAccount = directoryEntry.Children.Add("CN= " + userName, "user");
                userADAccount.Properties["samAccountName"].Value = userName;
                userADAccount.Properties["givenName"].Value = firstName;
                userADAccount.Properties["sn"].Value = lastName;
                userADAccount.CommitChanges();
            }
            else
            {
                // Use the existing AD account.
                userADAccount = result.GetDirectoryEntry();
            }

            // Set the password for the account.
            String password = "pass@word1";
            userADAccount.Invoke("SetPassword", new object[] { password });
            userADAccount.CommitChanges();
            directoryEntry.Close();
            userADAccount.Close();

            // Enable the newly created Active Directory account.
            userADAccount.Properties["userAccountControl"].Value =
                (int)userADAccount.Properties["userAccountControl"].Value & ~0x2;
            userADAccount.CommitChanges();

            // Wait 10 seconds for the AD account to propagate.
            Thread.Sleep(10000);

            // Retrieve the default business unit needed to create the user.
            QueryExpression businessUnitQuery = new QueryExpression
            {
                EntityName = BusinessUnit.EntityLogicalName,
                ColumnSet = new ColumnSet("businessunitid"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("parentbusinessunitid", 
                            ConditionOperator.Null)
                    }
                }
            };

            BusinessUnit defaultBusinessUnit = serviceProxy.RetrieveMultiple(
                businessUnitQuery).Entities[0].ToEntity<BusinessUnit>();

            // Retrieve the specified security role.
            Role role = RetrieveRoleByName(serviceProxy, roleStr);

            //Create a new system user.
            SystemUser user = new SystemUser
            {
                DomainName = domain + userName,
                FirstName = firstName,
                LastName = lastName,
                BusinessUnitId = new EntityReference
                {
                    LogicalName = BusinessUnit.EntityLogicalName,
                    Name = BusinessUnit.EntityLogicalName,
                    Id = defaultBusinessUnit.Id
                }
            };
            userId = serviceProxy.Create(user);

            // Assign the security role to the newly created Microsoft Dynamics CRM user.
            AssociateRequest associate = new AssociateRequest()
            {
                Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                RelatedEntities = new EntityReferenceCollection()
                {
                    new EntityReference(Role.EntityLogicalName, role.Id),
                },
                Relationship = new Relationship("systemuserroles_association")
            };
            serviceProxy.Execute(associate);

            return userId;
        }

        /// <summary>
        /// Helper method to prompt the user for the LDAP path for the network
        /// </summary>
        /// <returns>The LDAP path for your network</returns>
        public static String ConsolePromptForLDAPPath()
        {
            // Prompt for the LDAP path.
            Console.WriteLine("An Active Directory connection is required. Please enter an");
            Console.WriteLine("LDAP path for your network in the format 'LDAP://server/DC=subdomain,DC=domain,DC=com': ");
            String ldapPath = Console.ReadLine();

            return ldapPath;
        }

        private static bool UserInRole(OrganizationServiceProxy serviceProxy,
            Guid userId, Guid roleId)
        {
            // Establish a SystemUser link for a query.
            LinkEntity systemUserLink = new LinkEntity()
            {
                LinkFromEntityName = SystemUserRoles.EntityLogicalName,
                LinkFromAttributeName = "systemuserid",
                LinkToEntityName = SystemUser.EntityLogicalName,
                LinkToAttributeName = "systemuserid",
                LinkCriteria =
                {
                    Conditions = 
                    {
                        new ConditionExpression(
                            "systemuserid", ConditionOperator.Equal, userId)
                    }
                }
            };

            // Build the query.
            QueryExpression query = new QueryExpression()
            {
                EntityName = Role.EntityLogicalName,
                ColumnSet = new ColumnSet("roleid"),
                LinkEntities = 
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = "roleid",
                        LinkToEntityName = SystemUserRoles.EntityLogicalName,
                        LinkToAttributeName = "roleid",
                        LinkEntities = {systemUserLink}
                    }
                },
                Criteria =
                {
                    Conditions = 
                    {
                        new ConditionExpression("roleid", ConditionOperator.Equal, roleId)
                    }
                }
            };

            // Retrieve matching roles.
            EntityCollection ec = serviceProxy.RetrieveMultiple(query);

            if (ec.Entities.Count > 0)
                return true;

            return false;
        }

        private static Role RetrieveRoleByName(OrganizationServiceProxy serviceProxy,
            String roleStr)
        {
            QueryExpression roleQuery = new QueryExpression
            {
                EntityName = Role.EntityLogicalName,
                ColumnSet = new ColumnSet("roleid"),
                Criteria =
                {
                    Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Equal, roleStr)
                }
                }
            };

            return serviceProxy.RetrieveMultiple(roleQuery).Entities[0].ToEntity<Role>();
        }
    }
    //</snippetSystemUserProvider>
}