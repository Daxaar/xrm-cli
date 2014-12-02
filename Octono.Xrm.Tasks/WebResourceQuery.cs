using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Octono.Xrm.Tasks
{
    public interface IWebResourceQuery
    {
        Entity Retrieve(IOrganizationService service, string name);
    }

    public class WebResourceQuery : IWebResourceQuery
    {
        public Entity Retrieve(IOrganizationService service, string name)
        {
            var query = new QueryExpression("webresource") { ColumnSet = new ColumnSet(new[] { "content","webresourcetype" }) };

            query.Criteria.AddCondition("name", ConditionOperator.Equal, name);
            //query.Criteria.AddCondition("iscustomizable",ConditionOperator.Equal,true);

            query.Criteria.AddFilter(LogicalOperator.And).AddCondition("iscustomizable",ConditionOperator.Equal,true);
            var result = service.RetrieveMultiple(query);

            if (result.Entities.Any() == false)
            {
                throw new InvalidOperationException(string.Format("Cannot find JavaScript web resource {0}", name));                
            } 

            return result.Entities.Single();            
        }
    }
}