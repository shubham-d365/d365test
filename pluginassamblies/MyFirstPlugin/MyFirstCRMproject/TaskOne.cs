using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MyFirstCRMproject
{
    public class TaskOne : IPlugin
    {
        private string logicalName;
        
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            //Test
            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the IOrganizationService instance which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                if (entity.LogicalName != "account")
                    return;

                try
                {
                    Entity followup = new Entity("task");
                    followup["subject"] = "Test";
                    followup["description"] = "Test desc....";
                    followup["statecode"] = new OptionSetValue(1);

                    //if (context.OutputParameters.Contains("id"))
                    //{
                    //    Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                    //    string regardingobjectType = "account";

                    //    followup["regardingobjectid"] = new EntityReference(regardingobjectType, regardingobjectid);
                    //}

                    //followup.Attributes.Add("regardingobjectid", new EntityReference("account", accountId));

                    service.Create(followup);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
