using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.Extensions
{
    public static class InterfaceExtensions
    {
        public static bool Implements(this Type @this, Type @interface)
        {
            if (@this == null || @interface == null)
                return false;

            return @interface.GenericTypeArguments.Length > 0 ? @interface.IsAssignableFrom(@this) : @this.GetInterfaces().Any(c => c.Name == @interface.Name);
        }

        public static bool Implements(this PropertyInfo propertyInfo, string @interface)
        {
            if (propertyInfo == null || string.IsNullOrEmpty(@interface))
                return false;

            return propertyInfo.PropertyType.GetInterface(@interface) != null;
        }
    }
}
