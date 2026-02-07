using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace mdz.unity.editor
{

    public struct SerializedPropertyInfo
    {
        public readonly object fieldInfoinstance;
        public readonly Type fieldType;

        public SerializedPropertyInfo(object fieldInfoinstance, Type fieldType)
        {
            this.fieldInfoinstance = fieldInfoinstance;
            this.fieldType = fieldType;
        }
    }

    public static class PropertyExtensions
    {
        public static SerializedProperty FindPropertyRecursive(this SerializedObject serializedObject, string path)
        {
            var pathParts = path.Split('.');
            var currentProperty = serializedObject.FindProperty(pathParts[0]);
            for (int i = 1; i < pathParts.Length; i++)
            {
                currentProperty = currentProperty.FindPropertyRelative(pathParts[i]);
            }
            return currentProperty;
        }

        public static SerializedPropertyInfo GetCurrentPropertyFieldInfo(this SerializedProperty serializedProperty)
        {
            var pathParts = serializedProperty.propertyPath.Split('.');
            var targetObject = serializedProperty.serializedObject.targetObject;
            object instance = targetObject;
            FieldInfo fi = null;
            Type fieldType = instance.GetType();
            Type targetType = fieldType;
            for (int i = 0; i < pathParts.Length; i++)
            {
                fi = targetType.GetField(pathParts[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                fieldType = fi?.FieldType;
                if (fi != null)
                {
                    if (fi.FieldType.IsArray)
                    {
                        fieldType = fi.FieldType.GetElementType();
                        var index = int.Parse(pathParts[i + 2].Trim('d', 'a', 't', 'a', '[', ']'));
                        instance = (fi.GetValue(instance) as System.Collections.IList)[index];
                        i += 2;
                        targetType = instance?.GetType() ?? fieldType;
                        continue;
                    }

                    instance = fi.GetValue(instance);
                    targetType = instance?.GetType() ?? fieldType;
                    continue;
                }
                else
                {
                    return new SerializedPropertyInfo(instance, null);
                }
            }

            return new SerializedPropertyInfo(instance, fieldType);
        }

        public static bool IsAssignableTo(this Type fromType, Type type)
        {
            if (type.IsAssignableFrom(fromType))
            {
                return true;
            }
            else if (type.IsGenericTypeDefinition)
            {
                while (fromType != null)
                {
                    if (fromType.GetGenericTypeDefinition() == type)
                    {
                        return true;
                    }

                    fromType = fromType.BaseType;
                }
            }

            return false;
        }

        public static bool TryGetGenericDefines(this Type fromType, Type genericTypeDefinitiom, List<Type> types)
        {
            types.Clear();
            if (genericTypeDefinitiom.IsGenericTypeDefinition)
            {
                if (genericTypeDefinitiom.IsClass)
                {
                    while (fromType != null)
                    {
                        if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == genericTypeDefinitiom)
                        {
                            types.Add(fromType);
                            return true;
                        }
                        fromType = fromType.BaseType;
                    }
                }
                else if (genericTypeDefinitiom.IsInterface)
                {
                    foreach (var variable in fromType.GetInterfaces())
                    {
                        if (variable.IsGenericType && variable.GetGenericTypeDefinition() == genericTypeDefinitiom)
                        {
                            types.Add(variable);
                        }
                    }

                    return types.Count > 0;
                }
            }

            return false;
        }
        
        static StringBuilder displayNameBuilder = new StringBuilder();
        public static string GetDisplayName(this SerializedProperty property, SerializedProperty from)
        {
            var path = property.propertyPath.Split('.');
            var fromIndex = from != null ? from.propertyPath.Split('.').Length - 1 : 0;
            displayNameBuilder.Clear();
            for(int i = fromIndex; i < path.Length; i++)
            {
                var propPath = string.Join(".", path.Take(i + 1));
                var prop = property.serializedObject.FindProperty(propPath);
                if(prop.isArray)
                {
                    i ++;
                }
                displayNameBuilder.Append(prop.displayName)
                .Append(".");
            }
            return displayNameBuilder.ToString();
        }
    }
}