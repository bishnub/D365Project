using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cooper.CRM.Incident
{
    public class CasePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains(PluginParameter.Target) && context.InputParameters[PluginParameter.Target] is Entity)
                {
                    Entity incident = (Entity)context.InputParameters[PluginParameter.Target];

                    if (context.PrimaryEntityName.Equals(IncidentAttributes.EntityName)
                        && context.MessageName == PluginMessage.Create
                        && context.Stage == PluginStage.PreOperation
                        && context.Depth == 1)
                    {
                        int caseType = CrmHelper.GetOptionSetValue(incident, IncidentAttributes.CaseType);
                        int initiatedBy = CrmHelper.GetOptionSetValue(incident, IncidentAttributes.InitiatedBy);

                        if (caseType == (int)CaseType.PayOffRequest)
                        {
                            tracer.Trace(string.Format("CasePlugin: {0}", "Validate if any Active Payoff request Available for Customer"));

                            string customerId = CrmHelper.GetEntityReferenceId(incident, IncidentAttributes.CustomerId);
                            bool ifActivePayOffRequestsAvailable = CaseHandler.ValidateIfAnyActivePayOffRequest(service, customerId);

                            tracer.Trace(string.Format("CasePlugin: Active PayOffrequests for CustomerId : {0} : {1}", customerId, ifActivePayOffRequestsAvailable));

                            if (ifActivePayOffRequestsAvailable)
                            {
                                throw new InvalidPluginExecutionException("Active PayOffRequests available for this customer.");
                            }
                        }

                        if (caseType == (int)CaseType.PayOffRequest && initiatedBy == (int)InitiatedBy.Customer)
                        {
                            tracer.Trace(string.Format("CasePlugin: {0}", "Payoff request Initiated by Customer"));

                            string accountId = CrmHelper.GetEntityReferenceId(incident, IncidentAttributes.LoanAccount);

                            if (!string.IsNullOrEmpty(accountId))
                            {
                                Entity loanAccount = CrmHelper.RetrieveEntity(service, LoanAccountAttributes.EntityName, accountId,
                                    new string[] { LoanAccountAttributes.LoanStartDate, LoanAccountAttributes.PayOffAmount });
                                if (loanAccount != null)
                                {
                                    DateTime loanStartDate = CrmHelper.GetDateTime(loanAccount, LoanAccountAttributes.LoanStartDate);
                                    decimal payofAmount = CrmHelper.GetCurrencyValue(loanAccount, LoanAccountAttributes.PayOffAmount);

                                    //If Payment Amount is greater than 500k or payoffdate if less than 2 years of the initial loan Start Date
                                    if (payofAmount > 500000 || (DateTime.Now.Date < loanStartDate.AddYears(2))) // Can be  Added as Configuration Parameter
                                    {
                                        incident.Attributes[IncidentAttributes.IsAVPApprovalRequired] =  true;
                                        tracer.Trace(string.Format("CasePlugin: AVP Approval required {0}", true));
                                    }

                                    else
                                    {
                                        tracer.Trace(string.Format("CasePlugin: AVP Approval required {0}", false));
                                    }

                                }
                            }
                        }
                    }

                    else
                    {
                        tracer.Trace(string.Format("CasePlugin: {0}", "Plugin does not meet the condition"));
                        return;
                    }

                }
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the CasePlugin plug-in.", ex);
            }

            catch (Exception ex)
            {
                tracer.Trace("CasePlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}
