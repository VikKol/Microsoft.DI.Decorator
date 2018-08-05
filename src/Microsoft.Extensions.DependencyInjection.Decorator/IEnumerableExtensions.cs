using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection.Decorator
{
    internal static class IEnumerableExtensions
    {
        public static TSource SingleOrThrow<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> filter,
            Func<Exception> notFoundException,
            Func<Exception> multipleFoundException)
        {
            var result = default(TSource);

            var hasResult = false;
            var hasMany = false;

            foreach (var item in source)
            {
                if (filter(item))
                {
                    if (hasResult)
                    {
                        hasMany = true;
                        break;
                    }

                    hasResult = true;
                    result = item;
                }
            }

            if (!hasResult)
            {
                throw notFoundException();
            }

            if (hasMany)
            {
                throw multipleFoundException();
            }

            return result;
        }
    }
}