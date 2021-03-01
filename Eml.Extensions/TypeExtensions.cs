using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Eml.Extensions
{
    public static class TypeExtensions
    {
        public static List<string> GetDifference(this IEnumerable<string> source, IEnumerable<string> target)
        {
            var diff = target.Except(source).ToList();

            return diff;
        }
        public static Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        public static string GetBinDirectory<T>()
        {
            return Path.GetDirectoryName(typeof(T).Assembly.Location);
        }

        public static List<Assembly> GetReferencingAssemblies(Func<RuntimeLibrary, bool> whereClause)
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries
                .Where(whereClause);

            return dependencies
                .Select(r => GetAssembly(r.Name))
                .Distinct()
                .ToList();

            // TODO: determine if this is better
            //return dependencies
            //    .Select(r => r.GetType().Assembly)
            //    .Distinct()
            //    .ToList();
        }

        public static List<Assembly> GetReferencingAssemblies(this IReadOnlyCollection<string> startsWithAssemblyPattern)
        {
            var withPattern = new UniqueStringPattern(startsWithAssemblyPattern).Build();
            var referencedAssemblies = withPattern
                .Select(p => GetReferencingAssemblies(r => r.Name.IsWithStart(p)))
                .SelectMany(assembly => assembly.Select(r => r))
                .Distinct()
                ;

            return referencedAssemblies.ToList();
        }

        public static List<Assembly> GetReferencingAssemblies(this IReadOnlyCollection<string> startsWithAssemblyPattern, Func<RuntimeLibrary, bool> whereClause)
        {
            var withPattern = new UniqueStringPattern(startsWithAssemblyPattern).Build();
            var referencedAssemblies = withPattern
                .Select(p => GetReferencingAssemblies(r => r.Name.IsWithStart(p) && whereClause(r)))
                .SelectMany(assembly => assembly.Select(r => r))
                .Distinct()
                ;

            return referencedAssemblies.ToList();
        }
        public static List<Assembly> GetReferencingAssemblies()
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries.ToList();

            return dependencies
                .Select(r => GetAssembly(r.Name))
                .ToList();
        }

        public static List<string> GetMemberNames(this Type type)
        {
            var members = new List<string>();

            members.AddRange(type.GetPropertyNames());
            members.AddRange(type.GetMethodNames());

            return members;
        }

        public static List<string> GetMethodNames(this Type type)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(r => !r.IsSpecialName);

            return methods
                .Select(r => r.GetMethodSignature())
                .ToList();
        }

        public static string GetMethodSignature(this MethodInfo mi)
        {
            var param = mi.GetParameters()
                          .Select(p => $"{p.ParameterType.Name} {p.Name}")
                          .ToArray();
            var signature = $"{mi.ReturnType.Name} {mi.Name}({string.Join(",", param)})";

            return signature;
        }

        /// <summary>
        /// Uses BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy
        /// </summary>
        public static List<PropertyInfo> GetPublicProperties(this Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            return properties.ToList();
        }

        public static List<string> GetPropertyNames(this Type type)
        {
            var properties = type.GetPublicProperties();

            return properties
                .Select(property => property.Name)
                .ToList();
        }

        public static List<string> GetPropertyNames(this Type type, Func<PropertyInfo, bool> whereClause)
        {
            var properties = type.GetPublicProperties();

            return properties
                .Where(whereClause)
                .Select(property => property.Name)
                .ToList();
        }

        public static List<string> GetClassNames(this Assembly assembly, Func<Type, bool> whereClause, Func<string, string> valueSelector)
        {
            return assembly.GetClasses(whereClause)
                .Select(type => valueSelector(type.Name))
                .ToList();
        }

        public static List<string> GetClassNames(this Assembly assembly, Func<Type, bool> whereClause)
        {
            return assembly.GetClasses(whereClause)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetClassNames(this IEnumerable<Assembly> assemblies, Func<Type, bool> whereClause)
        {
            return assemblies.GetClasses(whereClause)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<Type> GetClasses(this IEnumerable<Assembly> assemblies, Func<Type, bool> whereClause, bool includeAbstract = false)
        {
            return assemblies.SelectMany(assembly => assembly.GetClasses(whereClause, includeAbstract))
                .ToList();
        }

        public static List<Type> GetClasses(this IEnumerable<Assembly> assemblies, bool includeAbstract = false)
        {
            return assemblies.SelectMany(assembly => assembly.GetClasses(includeAbstract))
                .ToList();
        }

        public static List<Type> GetClasses(this Assembly assembly, bool includeAbstract = false)
        {
            return includeAbstract
                ? assembly.GetTypes(type => !type.IsInterface)
                : assembly.GetTypes(type => !type.IsInterface && !type.IsAbstract);
        }

        public static List<Type> GetClasses(this Assembly assembly, Func<Type, bool> whereClause, bool includeAbstract = false)
        {
            return includeAbstract
                ? assembly.GetTypes(type => !type.IsInterface && whereClause(type))
                : assembly.GetTypes(type => !type.IsInterface && !type.IsAbstract && whereClause(type));
        }

        public static List<string> GetInterfaceNames(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes(nameSpace, type => type.IsInterface)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetInterfaceNames(this Assembly assembly, Func<Type, bool> whereClause)
        {
            return assembly.GetInterfaces(whereClause)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetInterfaceNames(this Assembly assembly)
        {
            return assembly.GetInterfaces()
                .Select(type => type.Name)
                .ToList();
        }

        public static List<Type> GetInterfaces(this IEnumerable<Assembly> assemblies, Func<Type, bool> whereClause)
        {
            return assemblies.SelectMany(assembly => assembly.GetInterfaces(whereClause))
                .ToList();
        }

        public static List<Type> GetInterfaces(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetInterfaces())
                .ToList();
        }

        public static List<Type> GetInterfaces(this Assembly assembly, Func<Type, bool> whereClause)
        {
            return assembly.GetTypes(type => type.IsInterface && whereClause(type));
        }

        public static List<Type> GetInterfaces(this Assembly assembly)
        {
            return assembly.GetTypes(type => type.IsInterface);
        }

        public static List<Type> GetTypes(this Assembly assembly, string nameSpace, Func<Type, bool> whereClause)
        {
            return assembly.GetTypes()
                           .Where(type => string.Equals(type.Namespace, nameSpace, StringComparison.Ordinal)
                                          && type.IsPublic
                                          && whereClause(type))
                           .ToList();
        }

        public static List<Type> GetTypes(this Assembly assembly, Func<Type, bool> whereClause)
        {
            var types = assembly.GetTypes();

            return types
                .Where(type => whereClause(type) && type.IsPublic)
                .ToList();
        }

        public static T GetPropertyAttribute<T>(this PropertyInfo prop)
            where T : class
        {
            return prop.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        public static T GetClassAttribute<T>(this Type type)
            where T : class
        {
            return type.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        /// <summary>
        /// TODO: Complete this!!
        /// </summary>
        public static bool IsSimpleType<T>(this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                new Type[] {
                    typeof(string),
                    typeof(int),
                    typeof(bool),
                    typeof(decimal),
                    typeof(float),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ;
        }

        public static List<Type> GetStaticClasses(this IEnumerable<Assembly> assemblies, Func<Type, bool> whereClause)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes(type => whereClause(type) 
                                                                               && type.IsClass 
                                                                               && type.IsSealed
                                                                               && type.IsAbstract))
                .ToList();
        }

        public static List<Type> GetStaticClasses(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes(type => type.IsClass
                                                                               && type.IsSealed
                                                                               && type.IsAbstract))
                .ToList();
        }
    }
}
