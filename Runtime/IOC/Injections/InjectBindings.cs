using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XcelerateGames.IOC
{
    public static class InjectBindings
    {
        public static void Inject(object obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            BindingManager bindingManager = BindingManager.Instance;
            if (bindingManager == null)
            {
                XDebug.LogException("BindingManager.Instance is null, Not injecting any dependency");
                return;
            }
            Type type = obj.GetType();
            List<FieldInfo> fieldInfo = GetFields(type);
            foreach (FieldInfo f in fieldInfo)
            {
                object[] attributes = f.GetCustomAttributes(true);
                if (attributes == null)
                    Debug.Log("Attribute[] is null");

                foreach (Attribute attr in attributes)
                {
                    if (attr is InjectSignal)
                    {
                        if (bindingManager._Bindings.ContainsKey(f.FieldType))
                            f.SetValue(obj, bindingManager._Bindings[f.FieldType]);
                        else
                        {
                            string path = null;
                            if (obj is MonoBehaviour)
                                path = ((MonoBehaviour)obj).GetObjectPath(); 
                            Debug.LogError($"Inject 1-> Could not find binding of type : {f.FieldType} for Type: {type}, Path: {path}");
                        }
                    }
                }
            }
        }

        public static void Inject(object obj, Dictionary<Type, object> bindings)
        {
            Type type = obj.GetType();
            List<FieldInfo> fieldInfo = GetFields(type);
            foreach (FieldInfo f in fieldInfo)
            {
                object[] attributes = f.GetCustomAttributes(false);
                foreach (Attribute attr in attributes)
                {
                    if (attr is InjectSignal)
                    {
                        if (bindings.ContainsKey(f.FieldType))
                            f.SetValue(obj, bindings[f.FieldType]);
                        else
                        {
                            string path = null;
                            if (obj is MonoBehaviour)
                                path = ((MonoBehaviour)obj).GetObjectPath();
                            Debug.LogError($"Inject 2-> Could not find binding of type : {f.FieldType} for Type: {type}, Path: {path}");
                        }
                    }
                }
            }
        }

        public static void InjectParameters(AbstractCommand command, Dictionary<Type, object> injectionBindings)
        {
            Type type = command.GetType();
            List<FieldInfo> fieldInfo = GetFields(type);
            foreach (FieldInfo f in fieldInfo)
            {
                object[] attributes = f.GetCustomAttributes(false);
                foreach (Attribute attr in attributes)
                {
                    if (attr is InjectParameter)
                    {
                        if (injectionBindings.ContainsKey(f.FieldType))
                            f.SetValue(command, injectionBindings[f.FieldType]);
                        else
                            Debug.LogError($"InjectParameters-> Could not find binding of type : {f.FieldType} for Type: {type}");
                    }
                    else if (attr is InjectParameterOptional)
                    {
                        if (injectionBindings.ContainsKey(f.FieldType))
                            f.SetValue(command, injectionBindings[f.FieldType]);
                    }
                }
            }
        }

        public static void InjectMethodParameters(object obj)
        {
            Type type = obj.GetType();
            BindingManager bindingManager = BindingManager.Instance;
            if (bindingManager == null)
            {
                XDebug.LogException("BindingManager.Instance is null, Not injecting any dependency");
                return;
            }

            List<MethodInfo> fieldInfo = GetMethods(type);

            foreach (MethodInfo f in fieldInfo)
            {
                object[] attributes = f.GetCustomAttributes(false);
                foreach (Attribute attr in attributes)
                {
                    if (attr is InjectMethod)
                    {
                        Debug.LogError($"THIS FEATURE IS NOT IMPLEMENTED");
                        //foreach (ParameterInfo parameterInfo in f.GetParameters())
                        //{
                        //    Type sig = typeof(AbstractSignal);
                        //    Type paramType = parameterInfo.ParameterType;
                        //    if (sig.IsAssignableFrom(paramType))
                        //    {
                        //        Debug.LogError($"{parameterInfo.Name} : {parameterInfo.ParameterType}");
                        //        if (bindingManager._Bindings.ContainsKey(paramType))
                        //        {
                        //        }
                        //        else
                        //            Debug.LogError($"InjectMethod-> Could not find binding for type : {paramType}");
                        //    }
                        //}
                    }
                }
            }
        }

        static List<FieldInfo> GetFields(Type type)
        {
            List<FieldInfo> fieldInfo = new List<FieldInfo>();
            //Using while loop to get private member variables of base class. Without this, only public & protected members will be updated
            while (type != null)
            {
                //Get all variables
                fieldInfo.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                type = type.BaseType;
            }

            return fieldInfo;
        }

        static List<MethodInfo> GetMethods(Type type)
        {
            List<MethodInfo> fieldInfo = new List<MethodInfo>();
            //Get all private variables
            fieldInfo.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            //Get all public variables
            //fieldInfo.AddRange(type.GetMethods());

            return fieldInfo;
        }
    }
}
