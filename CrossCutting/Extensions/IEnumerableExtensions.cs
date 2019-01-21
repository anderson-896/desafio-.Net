using System;

namespace CrossCutting.Extensions
{
    public static class ObjectExtension
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || String.IsNullOrWhiteSpace(obj.ToString());
        }
    }
}
