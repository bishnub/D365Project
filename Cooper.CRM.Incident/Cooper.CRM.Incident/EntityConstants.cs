using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooper.CRM.Incident
{
    public static class PluginParameter
    {
        public const string Target = "Target";
    }

    public static class PluginStage
    {
        public const int PreValidation = 10;
        public const int PreOperation = 20;
        public const int PostOperation = 40;
    }

    public static class PluginMessage
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Associate = "Associate";
    }

    public static class ContactAttributes
    {
        public const string EntityName = "contact";
        public const string ContactId = "contactid";
    }

    public static class IncidentAttributes
    {
        public const string EntityName = "incident";
        public const string IncidentId = "incidentid";
        public const string CustomerId = "customerid";
        public const string CaseType = "casetypecode";
        public const string Status = "statecode";
        public const string InitiatedBy = "coo_initiatedby";
        public const string LoanAccount = "coo_customeraccount";
        public const string IsAVPApprovalRequired = "coo_isavpapprovalrequired";
    }

    public static class LoanAccountAttributes
    {
        public const string EntityName = "coo_loanaccount";
        public const string Id = "coo_loanaccountid";
        public const string LoanStartDate = "coo_loanstartdate";
        public const string PayOffAmount = "coo_payoffamount";

    }

    public enum CaseType
    {
        Question = 1,
        Problem = 2,
        Request,
        PayOffRequest
    }

    public enum InitiatedBy
    {
        Customer = 181390000,
        ThirdParty
    }

    public enum IncidentStatus
    {
        Active = 0,
        Resolved,
        Cancelled
    }
}
