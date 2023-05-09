using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Linq;

namespace AutoPopulateWorkflow
{
    public class AutopopulateWorkflow : CodeActivity
    {

        [Input("Account Record to get values")]
        [RequiredArgument]
        [ReferenceTarget("account")] //Account Lookup as an input parameter
        public InArgument<EntityReference> Account { get; set; }


        [Input("Contact record to insert values")]
        [RequiredArgument]
        [ReferenceTarget("contact")] //Contact Lookup as an input parameter
        public InArgument<EntityReference> Contact { get; set; }

        protected override void Execute(CodeActivityContext context) //executes the code
        {

            //below code is for web services (performs data operations) 
            IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            //Create the tracing service
            ITracingService tracingService = context.GetExtension<ITracingService>();

            Guid accountID = this.Account.Get(context).Id; //get id of account
            Entity accountEntity = service.Retrieve("account", accountID, new ColumnSet("fax", "name", "address1_line1"));
            var accountFax = accountEntity["fax"].ToString(); //get value of fax field in account record
            string accountAddress = (string)accountEntity["address1_line1"];
            string accountName = (string)accountEntity["name"];
            var firstNameAcc = accountName.Split(' ').Skip(0).FirstOrDefault(); //extract the first word of account
            var secondNameAcc = accountName.Split(' ').Skip(1).FirstOrDefault(); //extract the second word of account


            Guid contactID = this.Contact.Get(context).Id; //get id of contact

            //instantiate new contact
            Entity contactRecord = new Entity("contact");
            contactRecord.Id = contactID;
            //populating the contact attributes/fields
            contactRecord["fax"] = accountFax;
            contactRecord["jobtitle"] = "Business Application Specialist/Developer";
            contactRecord["firstname"] = firstNameAcc;
            contactRecord["lastname"] = secondNameAcc;
            contactRecord["address1_line1"] = accountAddress;
            contactRecord["mobilephone"] = accountFax;
            contactRecord["telephone1"] = accountFax;
            contactRecord["emailaddress1"] = "contoso@default.com";
            service.Update(contactRecord);
        }
    }
}
