using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Eml.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<string> GetDifference(this IEnumerable<string> source, IEnumerable<string> target)
        {
            var diff = target.Except(source).ToList();
            return diff;
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        public static IEnumerable<Assembly> GetAssembliesFromDirectory(this DirectoryInfo directory)
        {
            var assemblies = new List<Assembly>();
            var files = Directory.GetFiles(directory.FullName, "*.dll").ToList();

            files.ForEach(r => assemblies.Add(Assembly.LoadFile(r)));

            return assemblies;
        }

        public static IEnumerable<Assembly> GetAssembliesFromDirectory(this DirectoryInfo directory, string filePattern)
        {
            var assemblies = new List<Assembly>();
            var files = Directory.GetFiles(directory.FullName, filePattern).ToList();

            files.ForEach(r => assemblies.Add(Assembly.LoadFile(r)));

            return assemblies;
        }

        public static IEnumerable<string> GetMemberNames(this Type type)
        {
            var members = new List<string>();

            members.AddRange(type.GetPropertyNames());
            members.AddRange(type.GetMethodNames());

            return members;
        }

        public static IEnumerable<string> GetMethodNames(this Type type)
        {
            return type.GetMethods()
                .Where(r => !r.IsSpecialName)
                .Select(r => r.GetMethodSignature());
        }

        public static string GetMethodSignature(this MethodInfo mi)
        {
            var param = mi.GetParameters()
                          .Select(p => $"{p.ParameterType.Name} {p.Name}")
                          .ToArray();
            var signature = $"{mi.ReturnType.Name} {mi.Name}({string.Join(",", param)})";

            return signature;
        }

        public static IEnumerable<string> GetPropertyNames(this Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties.Select(property => property.Name);
        }

        public static IEnumerable<string> GetPropertyNames(this Type type, Func<PropertyInfo, bool> selector)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties
                .Where(selector)
                .Select(property => property.Name);
        }

        public static IEnumerable<string> GetClassNames(this Assembly assembly, string nameSpace)
        {
            return assembly.GetClasses(nameSpace).Select(type => type.Name);
        }

        public static IEnumerable<string> GetClassNames(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetClasses(selector).Select(type => type.Name);
        }

        public static IEnumerable<Type> GetClasses(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes(nameSpace, type => !type.IsAbstract);
        }

        public static IEnumerable<Type> GetClasses(this Assembly assembly)
        {
            return assembly.GetTypes(type => !type.IsAbstract);
        }

        public static IEnumerable<Type> GetClasses(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetTypes(type => !type.IsAbstract && selector(type));
        }

        public static IEnumerable<string> GetInterfaceNames(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes(nameSpace, type => type.IsInterface && !type.IsGenericType).Select(type => type.Name);
        }

        public static IEnumerable<string> GetInterfaceNames(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetInterfaces(selector).Select(type => type.Name);
        }

        public static IEnumerable<Type> GetInterfaces(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetTypes(type => type.IsInterface && selector(type) && !type.IsGenericType);
        }

        public static IEnumerable<Type> GetTypes(this Assembly assembly, string nameSpace, Func<Type, bool> selector)
        {
            return assembly.GetTypes()
                           .Where(type => string.Equals(type.Namespace, nameSpace, StringComparison.Ordinal)
                                          && type.IsPublic
                                          && selector(type));
        }

        public static IEnumerable<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector)
        {
            var types = assembly.GetTypes();

            return types.Where(type => selector(type) && type.IsPublic);
        }

        public static T GetAttribute<T>(this PropertyInfo prop) where T : class
        {
            return prop.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        public static string ConstructTestMessageForMissingArrays<T>(this IEnumerable<T> items, string msg)
        {
            var enumerable = items as T[] ?? items.ToArray();

            return $"{Environment.NewLine}({enumerable.Count()}){msg}: {Environment.NewLine}    {string.Join(Environment.NewLine + "    ", enumerable.ToArray())}{Environment.NewLine}";
        }
    }
}
