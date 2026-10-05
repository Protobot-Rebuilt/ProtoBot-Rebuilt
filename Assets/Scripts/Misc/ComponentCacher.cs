using System;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Protobot
{
    public static class ComponentCacher
    {
        private static readonly Dictionary<Type, FieldInfo[]> CachedFieldsByType = new Dictionary<Type, FieldInfo[]>();
        private static readonly List<Type> CachedComponentTypes = new List<Type>();
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;
            BuildTypeCache();
            CacheComponents();
        }

        private static void BuildTypeCache()
        {
            CachedComponentTypes.Clear();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }

                if (types == null)
                    continue;

                foreach (var type in types)
                {
                    if (type == null || type.IsAbstract || !typeof(MonoBehaviour).IsAssignableFrom(type))
                        continue;

                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                    var attFields = fields.Where(f => f.GetCustomAttributes(typeof(CacheComponentAttribute), true).Length != 0).ToArray();

                    if (attFields.Length == 0)
                        continue;

                    CachedFieldsByType[type] = attFields;
                    CachedComponentTypes.Add(type);
                }
            }
        }

        public static void CacheComponents()
        {
            if (!initialized)
                Init();

            int componentsCached = 0;

            foreach (var type in CachedComponentTypes)
            {
                if (!CachedFieldsByType.TryGetValue(type, out var attFields) || attFields.Length == 0)
                {
                    continue;
                }

                var instances = UnityEngine.Object.FindObjectsOfType(type, true);

                foreach (var instance in instances)
                {
                    var m = instance as MonoBehaviour;
                    if (m == null || !m.gameObject.scene.IsValid())
                        continue;

                    Component[] components = m.GetComponents<Component>();

                    foreach (FieldInfo f in attFields)
                    {
                        bool componentFound = false;
                        Type fieldType = f.FieldType;

                        foreach (Component c in components)
                        {
                            if (c == null)
                                continue;

                            Type cType = c.GetType();

                            if (cType == fieldType || fieldType.IsAssignableFrom(cType))
                            {
                                f.SetValue(m, c);
                                componentFound = true;
                                componentsCached++;
                                break;
                            }
                        }

                        if (!componentFound)
                            Debug.LogError("[ComponentCacher] Component of type " + f.FieldType + " not found on " + m.gameObject.name);
                    }
                }
            }

#if UNITY_EDITOR
            Debug.LogWarning("[ComponentCacher] Components Cached: " + componentsCached);
            Debug.LogWarning("[ComponentCacher] Cached Component Types: " + CachedFieldsByType.Count);
#endif
        }
    }
}