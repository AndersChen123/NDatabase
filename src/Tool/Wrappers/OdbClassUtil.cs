using System;
using System.Collections.Concurrent;
using NDatabase.Odb;

namespace NDatabase.Tool.Wrappers
{
    internal static class OdbClassUtil
    {
        private static readonly ConcurrentDictionary<string, string> CacheByFullClassName =
            new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<Type, string> CacheByType =
            new ConcurrentDictionary<Type, string>();

        public static string GetClassName(string fullClassName)
        {
            return CacheByFullClassName.GetOrAdd(fullClassName, ProduceClassName);
        }

        private static string ProduceClassName(string fullClassName)
        {
            var index = fullClassName.LastIndexOf('.');

            var className = index == -1
                                ? fullClassName // primitive type
                                : GetClassName(fullClassName, index);
            return className;
        }

        private static string GetClassName(string fullClassName, int index)
        {
            var startIndex = index + 1;
            return fullClassName.Substring(startIndex, fullClassName.Length - startIndex);
        }

        public static string GetNamespace(string fullClassName)
        {
            var index = fullClassName.LastIndexOf('.');
            return index == -1
                       ? string.Empty
                       : fullClassName.Substring(0, index);
        }

        public static String GetFullName(Type type)
        {
            return !OdbConfiguration.HasAssemblyQualifiedTypes()
                       ? type.FullName
                       : CacheByType.GetOrAdd(type, ProduceFullName);
        }

        private static string ProduceFullName(Type type)
        {
            var name = type.Assembly.GetName();
            var publicKey = name.GetPublicKey();
            var isSignedAsm = publicKey.Length > 0;

            var index = type.Assembly.FullName.IndexOf(',');

            var fullName = string.Format("{0},{1}", type.FullName, isSignedAsm
                                                                       ? type.Assembly.FullName
                                                                       : type.Assembly.FullName.Substring(0, index));
            return fullName;
        }
    }
}
