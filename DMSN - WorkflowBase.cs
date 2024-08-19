using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace DMSN.Plugin.Workflow
{
    public abstract class WorkflowBase : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var workflowContext = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            try
            {
                Execute(workflowContext, service, tracingService);
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message);
                throw;
            }
        }

        public abstract void Execute(IWorkflowContext context, IOrganizationService service, ITracingService tracingService);
    }
}
