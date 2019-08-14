using System;
using System.Linq;

namespace Eml.Extensions
{
    public static class SimpleMapper
    {
        /// <summary>
        /// Map properties from Entity to Dto. Do not use this to process entities from large lists due to performance penalties.
        /// </summary>
        public static T1 MapTo<T, T1>(this T source, T1 destination)
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
                catch (ArgumentException)
                {
                }
            }

            return destination;
        }
    }
}
