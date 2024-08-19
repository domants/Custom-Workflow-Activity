using DMSN.Plugin.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace DMSN.Plugin
{
    public class _3TestWorkflow : WorkflowBase
    {
        [Input("Account Record to get values")]
        [RequiredArgument]
        [ReferenceTarget("account")] //Account Lookup as an input parameter
        public InArgument<EntityReference> Account { get; set; }

        [Input("Contact record to insert values")]
        [RequiredArgument]
        [ReferenceTarget("contact")] //Contact Lookup as an input parameter
        public InArgument<EntityReference> Contact { get; set; }

        public override void Execute(IWorkflowContext workflowContext, IOrganizationService service, ITracingService tracingService)
        {
           // Cast workflow context to CodeActivityContext to access input parameters
           var context = workflowContext as CodeActivityContext;
           Guid AccountId = this.Account.Get(context).Id;

           tracingService.Trace("test");

            
        }
    }
}
