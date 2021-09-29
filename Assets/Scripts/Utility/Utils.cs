// This class contains some helper functions.
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;

public class Utils
{
    public static string PBKDF2Hash(string text, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
        byte[] hash = pbkdf2.GetBytes(20);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }
    static Dictionary<KeyValuePair<Type,string>, MethodInfo[]> lookup = new Dictionary<KeyValuePair<Type,string>, MethodInfo[]>();
    public static MethodInfo[] GetMethodsByPrefix(Type type, string methodPrefix)
    {
        KeyValuePair<Type,string> key = new KeyValuePair<Type,string>(type, methodPrefix);
        if (!lookup.ContainsKey(key))
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                       .Where(m => m.Name.StartsWith(methodPrefix))
                                       .ToArray();
            lookup[key] = methods;
        }
        return lookup[key];
    }
    public static void InvokeMany(Type type, object onObject, string methodPrefix, params object[] args)
    {
        foreach (MethodInfo method in GetMethodsByPrefix(type, methodPrefix))
            method.Invoke(onObject, args);
    }
}
