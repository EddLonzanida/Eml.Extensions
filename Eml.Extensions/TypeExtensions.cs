#if NETCORE
using Microsoft.Extensions.DependencyModel;
#endif

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
#if NETFULL
            return Assembly.Load(assemblyName);
#endif
#if NETCORE
            return Assembly.Load(new AssemblyName(assemblyName));
#endif
        }

#if NETFULL
        public static Assembly GetAssembly(this FileInfo fileInfo)
        {
            return Assembly.LoadFile(fileInfo.FullName);
        }

        public static Assembly GetAssemblyFromPath(string path)
        {
            var fileInfo = new FileInfo(path);

            return fileInfo.GetAssembly();
        }

        /// <summary>
        /// Search for *.dll 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static List<Assembly> GetAssembliesFromDirectory(this DirectoryInfo directory)
        {
            var files = Directory.GetFiles(directory.FullName, "*.dll").ToList();

            return files
               .Select(GetAssemblyFromPath)
               .ToList();
        }

        public static List<Assembly> GetAssembliesFromDirectory(this DirectoryInfo directory, string filePattern)
        {
            var files = Directory.GetFiles(directory.FullName, filePattern).ToList();

            return files
              .Select(GetAssemblyFromPath)
              .ToList();
        }

        public static List<Assembly> GetAssembliesFromDirectory(this DirectoryInfo directory, Func<string, bool> whereClause)
        {
            var files = Directory.GetFiles(directory.FullName).ToList();

            return files
                .Where(whereClause)
                .Select(GetAssemblyFromPath)
                .ToList();
        }

        public static string GetBinDirectory()
        {
            var binDirectory = Path.GetDirectoryName(System.Reflection.Assembly
                .GetExecutingAssembly().GetName().CodeBase)?.Replace(@"file:\", string.Empty);

            if (!Directory.Exists(binDirectory))
            {
                binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            return binDirectory;
        }
#endif
#if NETCORE
        public static string GetBinDirectory<T>()
        {
            return Path.GetDirectoryName(typeof(T).Assembly.Location);
        }

        public static List<Assembly> GetReferencingAssemblies(Func<RuntimeLibrary, bool> whereClause)
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries.ToList();

            return dependencies.Where(whereClause)
                .Select(r => GetAssembly(r.Name))
                .ToList();
        }

        public static List<Assembly> GetReferencingAssemblies(IReadOnlyCollection<string> startsWithAssemblyPattern)
        {
            var withPattern = new UniqueStringPattern(startsWithAssemblyPattern)
                .Build()
                .ConvertAll(r => r.ToLower());
            var referencedAssemblies = withPattern
                .Select(p => GetReferencingAssemblies(r => r.Name.ToLower().StartsWith(p)))
                .SelectMany(assembly => assembly.Select(r => r));

            return referencedAssemblies.ToList();
        }

        public static List<Assembly> GetReferencingAssemblies()
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries.ToList();

            return dependencies
                .Select(r => GetAssembly(r.Name))
                .ToList();
        }
#endif

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

        public static List<string> GetPropertyNames(this Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            return properties
                .Select(property => property.Name)
                .ToList();
        }

        public static List<string> GetPropertyNames(this Type type, Func<PropertyInfo, bool> selector)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            return properties
                .Where(selector)
                .Select(property => property.Name)
                .ToList();
        }

        public static List<string> GetClassNames(this Assembly assembly, string nameSpace)
        {
            return assembly.GetClasses(nameSpace).Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetClassNames(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetClasses(selector)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<Type> GetClasses(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes(nameSpace, type => !type.IsAbstract);
        }

        public static List<Type> GetClasses(this Assembly assembly)
        {
            return assembly.GetTypes(type => !type.IsAbstract);
        }

        public static List<Type> GetClasses(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetTypes(type => !type.IsAbstract && selector(type));
        }

        public static List<string> GetInterfaceNames(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes(nameSpace, type => type.IsInterface && !type.IsGenericType)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetInterfaceNames(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetInterfaces(selector)
                .Select(type => type.Name)
                .ToList();
        }

        public static List<string> GetInterfaceNames(this Assembly assembly)
        {
            return assembly.GetInterfaces()
                .Select(type => type.Name)
                .ToList();
        }

        public static List<Type> GetInterfaces(this Assembly assembly, Func<Type, bool> selector)
        {
            return assembly.GetTypes(type => type.IsInterface && selector(type) && !type.IsGenericType);
        }

        public static List<Type> GetInterfaces(this Assembly assembly)
        {
            return assembly.GetTypes(type => type.IsInterface && !type.IsGenericType);
        }

        public static List<Type> GetTypes(this Assembly assembly, string nameSpace, Func<Type, bool> selector)
        {
            return assembly.GetTypes()
                           .Where(type => string.Equals(type.Namespace, nameSpace, StringComparison.Ordinal)
                                          && type.IsPublic
                                          && selector(type))
                           .ToList();
        }

        public static List<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector)
        {
            var types = assembly.GetTypes();

            return types
                .Where(type => selector(type) && type.IsPublic)
                .ToList();
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
