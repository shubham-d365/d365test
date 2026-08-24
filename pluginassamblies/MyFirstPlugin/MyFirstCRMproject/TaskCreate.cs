using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Security.Principal;


namespace MyFirstCRMproject
{
    public class TaskCreate : IPlugin
    {
     

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contact = (Entity)context.InputParameters["Target"];
                var user = "abc@gmail.com";
                var password = "Abcd@123";
                // Obtain the IOrganizationService instance which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.
                    //    Entity taskID = new Entity("task");
                    //    taskID.Attributes.Add("Subject")
                    //    Guid Taskguid = service.Create(taskID);
                    // Plug-in business logic goes here.
                    //Entity taskID = new Entity("task");
                    //taskID.Attributes.Add("Subject", "Test");
                    //taskID.Attributes.Add("description", "Test Desec");
                    //taskID.Attributes.Add("scheduledend", DateTime.Now);
                    //taskID.Attributes.Add("regardingobjectid", contact.ToEntityReference());
                    //Guid Taskguid = service.Create(taskID);

                    string title = contact.Attributes["emailaddress1"].ToString();

                    contact.Attributes.Add("ap_description", "Hello World " + title);
                   
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
