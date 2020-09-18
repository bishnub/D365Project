using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooper.CRM.Incident
{
    public class CaseHandler
    {
        public static bool ValidateIfAnyActivePayOffRequest(IOrganizationService service, string customerId)
        {
            QueryExpression caseQuery = new QueryExpression
            {
                EntityName = IncidentAttributes.EntityName,
                ColumnSet = new ColumnSet(IncidentAttributes.IncidentId),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                          new ConditionExpression { AttributeName = IncidentAttributes.CustomerId, Operator = ConditionOperator.Equal, Values = { Guid.Parse(customerId) } },
                          new ConditionExpression { AttributeName = IncidentAttributes.Status, Operator = ConditionOperator.Equal, Values = { (int)IncidentStatus.Active} },
                          new ConditionExpression { AttributeName = IncidentAttributes.CaseType, Operator = ConditionOperator.Equal, Values = { (int)CaseType.PayOffRequest} }
                    }
                }
            };

            EntityCollection cases = service.RetrieveMultiple(caseQuery);

            if (cases != null && cases.Entities.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
