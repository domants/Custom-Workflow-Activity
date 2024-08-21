using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using System;
using System.Activities;
using DMSN.Plugin.Workflow;
using System.Threading;

namespace DMSN.Plugin
{
    public class _3AutopopulateWF : WorkflowBase
    {
        [Input("Account Record to get values")]
        [RequiredArgument]
        [ReferenceTarget("account")] //Account Lookup as an input parameter
        public InArgument<EntityReference> Account { get; set; }

        [Input("Contact record to insert values")]
        [RequiredArgument]
        [ReferenceTarget("contact")] //Contact Lookup as an input parameter
        public InArgument<EntityReference> Contact { get; set; }

        public override void Execute(CodeActivityContext context, IOrganizationService service, ITracingService tracingService)
        {
            try
            {
                tracingService.Trace("Tracing Started...");
                // Retrieve the input parameters using 'context'
                
                Guid AccountId = Account.Get(context).Id;
                tracingService.Trace($"Account ID: {AccountId}");
                ColumnSet columnSet = new ColumnSet("fax", "telephone1", "address1_line1", "address1_city", "address1_stateorprovince", "address1_postalcode", "address1_country");
                Entity accountEntity = service.Retrieve("account", AccountId, columnSet);
                var fax = accountEntity.GetAttributeValue<string>("fax");
                var telephone = accountEntity.GetAttributeValue<string>("telephone1");
                var address1_line1 = accountEntity.GetAttributeValue<string>("address1_line1");
                var address1_city = accountEntity.GetAttributeValue<string>("address1_city");
                var address1_stateorprovince = accountEntity.GetAttributeValue<string>("address1_stateorprovince");
                var address1_postalcode = accountEntity.GetAttributeValue<string>("address1_postalcode");
                var address1_country = accountEntity.GetAttributeValue<string>("address1_country");

                tracingService.Trace($"Account Fax: {fax.ToString()}");

                //Populate contact record
                Guid contactId = Contact.Get(context).Id;
                Entity contactRecord = new Entity("contact");  //Instantiate new contact record
                contactRecord.Id = contactId;
                contactRecord["fax"] = fax;
                contactRecord["telephone1"] = telephone;
                contactRecord["address1_line1"] = address1_line1;
                contactRecord["address1_city"] = address1_city;
                contactRecord["address1_stateorprovince"] = address1_stateorprovince;
                contactRecord["address1_postalcode"] = address1_postalcode;
                contactRecord["address1_country"] = address1_country;

                tracingService.Trace($"Contact populated...Workflow Base is working!");
                service.Update(contactRecord);
             
                
            }
            catch (Exception ex)
            {
                // Throw an error message to be displayed to the user
                throw new InvalidPluginExecutionException($"An error occurred: {ex.Message}");
            }
        }
    }

}
