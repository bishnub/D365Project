using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooper.CRM.Incident
{
    public class CrmHelper
    {
        public static int GetOptionSetValue(Entity entity, string attributeName)
        {
            if (entity.Contains(attributeName))
            {
                return entity.GetAttributeValue<OptionSetValue>(attributeName).Value;
            }

            return 0;
        }

        public static string GetEntityReferenceId(Entity entity, string attributeName)
        {
            if (entity.Contains(attributeName))
            {
                return entity.GetAttributeValue<EntityReference>(attributeName).Id.ToString();
            }

            return string.Empty;
        }

        public static DateTime GetDateTime(Entity entity, string attributeName)
        {
            if (entity.Contains(attributeName))
            {
                return entity.GetAttributeValue<DateTime>(attributeName);
            }

            return DateTime.Now;
        }

        public static decimal GetCurrencyValue(Entity entity, string attributeName)
        {
            if (entity.Contains(attributeName))
            {
                return entity.GetAttributeValue<Money>(attributeName).Value;
            }

            return 0;
        }

        public static Entity RetrieveEntity(IOrganizationService service, string entityName, string entityId, string[] attributes)
        {
            return service.Retrieve(entityName, Guid.Parse(entityId),
                                            new Microsoft.Xrm.Sdk.Query.ColumnSet(attributes));
        }
    }
}
