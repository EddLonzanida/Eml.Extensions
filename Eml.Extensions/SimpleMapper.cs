using System;
using System.Linq;

namespace Eml.Extensions
{
    public static class SimpleMapper
    {
        /// <summary>
        /// Map common properties between types. destination is ByRef.
        /// <para>Do not use this to process large lists due to performance penalties.</para>
        /// <para>Example:</para>
        /// <code language="c#">sourceEntity.MapTo(destinationEntity);</code>
        /// </summary>
        public static void MapTo<T, T1>(this T source, ref T1 destination, bool suppressErrors = true)
            where T : class
            where T1 : class
        {
            var sourceProperties = source.GetType().GetProperties().ToList();
            var destinationProperties = destination.GetType().GetProperties().ToList();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

                if (destinationProperty == null) continue;

                try
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
                catch (Exception)
                {
                    if (!suppressErrors)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
