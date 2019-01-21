using System;
using System.Linq;
using System.Reflection;

namespace Actions.Domain.Entities
{
    public abstract class Model
    {
        public bool HasAnyEmptyProperties()
        {
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var values = properties.Select(
                x =>
                x.GetValue(this, null));
            var hasProperty = values.Any(v => v == null || String.IsNullOrWhiteSpace(v.ToString()) || String.IsNullOrEmpty(v.ToString()));
            return hasProperty;
        }
    }
}
