using System;
using System.Collections.Generic;
using System.Linq;

namespace WooliesXTest.Utility
{
    public static class Extension
    {
        public static Uri CombineUri(this Uri baseUri, string relativeOrAbsoluteUri)
        {
            return new Uri(baseUri, relativeOrAbsoluteUri);
        }

        public static bool HasItems<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }
    }
}
