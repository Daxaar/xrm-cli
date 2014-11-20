using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public class PrimaryAttributeEqualityComparer : IEqualityComparer<Entity>
    {
        private readonly string _primaryAttributeName;

        public PrimaryAttributeEqualityComparer(string primaryAttributeName)
        {
            _primaryAttributeName = primaryAttributeName;
        }

        public bool Equals(Entity x, Entity y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            if (y == null) return false;

            return String.Equals(x.GetAttributeValue<string>(_primaryAttributeName),
                                 y.GetAttributeValue<string>(_primaryAttributeName),
                                 StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Entity obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash*23 + obj.Id.GetHashCode();
                hash = hash*23 + obj.LogicalName.GetHashCode();
                return hash;
            }
        }
    }
}