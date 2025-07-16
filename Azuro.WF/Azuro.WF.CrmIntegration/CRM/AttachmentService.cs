using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Azuro.Models;
using Azuro.Integration;
using System.Collections.Generic;

namespace Azuro.Integration
{
    public class AttachmentService : IAttachmentService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

       #region Constructor

        public AttachmentService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public Attachment SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""activitymimeattachment"">
                              <all-attributes/>
                              <filter type='and'>  " +
                                "<condition attribute='activitymimeattachmentid' operator='eq' value='" + id + "' /> " +
                            @"</filter>  
                              </entity>
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Attachment item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (result != null && result.Entities.Count > 0)
            {
                var singleResult = result.Entities[0];
                item = new Attachment()
                        {
                            Id = id,
                            ActivityId = (Guid)singleResult["activityid"],
                            AttachmentNumber = (int) singleResult["attachmentnumber"],
                            Body = singleResult["body"].ToString(),
                            Filename = singleResult["filename"].ToString(),
                            Filesize = (int) singleResult["filesize"],
                            Mimetype = singleResult["mimetype"].ToString(),
                            Subject = singleResult["subject"].ToString()
                        };
            }

            return item;
        }

        public List<Attachment> SelectByEmailId(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""activitymimeattachment"">
                              <all-attributes/>
                              <filter type='and'>  " +
                                "<condition attribute='activityid' operator='eq' value='" + id + "' /> " +
                            @"</filter>  
                              </entity>
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            List<Attachment> results = new List<Attachment>();
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (result != null && result.Entities.Count > 0)
            {
                var singleResult = result.Entities[0];
                var item = new Attachment();

                item.Id = id;
                item.ActivityId = ((EntityReference)singleResult["activityid"]).Id;

                if (singleResult.Attributes.ContainsKey("body"))
                   item.Body = singleResult["body"].ToString();

                if (singleResult.Attributes.ContainsKey("documentbody"))
                    item.Document = singleResult["documentbody"].ToString();

                if (singleResult.Attributes.ContainsKey("filename"))
                    item.Filename = singleResult["filename"].ToString();

                if (singleResult.Attributes.ContainsKey("filesize"))
                    item.Filesize = (int)singleResult["filesize"];

                if (singleResult.Attributes.ContainsKey("mimetype"))
                    item.Mimetype = singleResult["mimetype"].ToString();

                if (singleResult.Attributes.ContainsKey("subject"))
                   item. Subject = singleResult["subject"].ToString();
                

                results.Add(item);
            }

            return results;
        }

        public Attachment SelectAttachmentByName(string name)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""annotation"">
                              <all-attributes/>
                              <filter type='and'>  " +
                                "<condition attribute='subject' operator='eq' value='" + name + "' /> " +
                            @"</filter>  
                              </entity>
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Attachment item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (result != null && result.Entities.Count > 0)
            {
                var singleResult = result.Entities[0];
                item = new Attachment()
                {
                    Id = (Guid)singleResult["annotationid"],
                    //ActivityId = (Guid)singleResult["activityid"],
                    //AttachmentNumber = (int)singleResult["attachmentnumber"],
                    //Body = singleResult["body"].ToString(),
                    //Filename = singleResult["filename"].ToString(),
                    //Filesize = (int)singleResult["filesize"],
                    //Mimetype = singleResult["mimetype"].ToString(),
                    Subject = singleResult["subject"].ToString()
                };
            }

            return item;
        }

        public OrganizationResponse Insert(Attachment entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "activitymimeattachment";

                detail.Attributes["objectid"] = new EntityReference(entity.ObjectType, entity.ObjectId);
                detail.Attributes["objecttypecode"] = entity.ObjectType;
                detail.Attributes["activityid"] = entity.ActivityId;
                detail.Attributes["attachmentnumber"] = entity.AttachmentNumber;
                detail.Attributes["body"] = entity.Body;
                detail.Attributes["filename"] = entity.Filename;
                detail.Attributes["filesize"] = entity.Filesize;
                detail.Attributes["mimetype"] = entity.Mimetype;
                detail.Attributes["subject"] = entity.Subject;
                detail.Attributes["documentbody"] = entity.Document;

                // Create the request object.
                var create = new CreateRequest();
                create.Target = detail;

                return (CreateResponse)orgService.Execute(create);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
