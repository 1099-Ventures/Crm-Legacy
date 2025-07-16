using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace Azuro.CrmIntegration.CRM
{
    public class NoteService : INoteService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public NoteService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Inserts an item into Note
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public CreateResponse Insert(Note annotation)
        {
            try
            {
                var entity = new Entity();
                entity.LogicalName = "annotation";

                entity.Attributes["subject"] = annotation.Subject;
                entity.Attributes["notetext"] = annotation.NoteText;

                if (annotation.RegardingId != Guid.Empty)
                    entity.Attributes["objectid"] = new EntityReference(annotation.RegardingType, annotation.RegardingId);

                entity.Attributes["filename"] = annotation.Filename;
                entity.Attributes["filesize"] = annotation.Filesize;
                entity.Attributes["mimetype"] = annotation.Mimetype;

                entity.Attributes["documentbody"] = annotation.Document;
                entity.Attributes["documentbody"] = annotation.Body;

                // Create the case item.
                var create = new CreateRequest { Target = entity };
                var result = (CreateResponse)orgService.Execute(create);

                return result;

            }
            catch (FaultException<OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Note SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""annotation"">
                                    <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='activityid' operator='eq' value='" + id + "' /> " +
                                @"</filter>
                              </entity> 
                      </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Note item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Note();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["activityid"].ToString());
                item.Subject = singleResult["subject"].ToString();
                item.NoteText = singleResult["notetext"].ToString();

                if (singleResult.GetAttributeValue<EntityReference>("regardingobjectid") != null)
                {
                    var regarding = ((EntityReference)singleResult["regardingobjectid"]);
                    item.RegardingId = regarding.Id;
                    item.RegardingType = regarding.LogicalName;
                }
            }

            return item;
        }

        public Note SelectBySubject(string subject)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""annotation"">
                                    <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='subject' operator='like' value='%" + subject + "%' /> " +
                                @"</filter>
                              </entity> 
                      </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Note item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Note();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["activityid"].ToString());
                item.Subject = singleResult["subject"].ToString();
                item.NoteText = singleResult["notetext"].ToString();

                if (singleResult.GetAttributeValue<EntityReference>("regardingobjectid") != null)
                {
                    var regarding = ((EntityReference)singleResult["regardingobjectid"]);
                    item.RegardingId = regarding.Id;
                    item.RegardingType = regarding.LogicalName;
                }
            }

            return item;
        }

        public OrganizationResponse Update(Note entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "annotation";
                detail.Attributes["activityid"] = entity.Id;
                detail.Attributes["regardingobjectid"] = new EntityReference("incident", entity.RegardingId);

                // Create the request object.
                var update = new UpdateRequest { Target = detail };

                // Create the request object.
                return orgService.Execute(update);
            }
            catch (FaultException<OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
