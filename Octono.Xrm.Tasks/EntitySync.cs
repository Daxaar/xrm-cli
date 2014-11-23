using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// WIP: Used for syncing records between Organisations
    /// </summary>
    public class EntitySync
    {
        private readonly IEnumerable<Entity> _source;
        private readonly IEnumerable<Entity> _target;
        private readonly PrimaryAttributeEqualityComparer _comparer;

        public EntitySync(IEnumerable<Entity> source, IEnumerable<Entity> target, string primaryAttributeName)
        {
            _source = source;
            _target = target;
            _comparer = new PrimaryAttributeEqualityComparer(primaryAttributeName);
        }

        public IEnumerable<Entity> ToBeDeleted
        {
            get { return _target.Except(_source, _comparer); }
        }
        public IEnumerable<Entity> ToBeAdded
        {
            get { return _source.Except(_target, _comparer); }
        }

        public IEnumerable<Entity> ToBeUpdated { get { return _source.Intersect(_target, _comparer); }}
    }
}