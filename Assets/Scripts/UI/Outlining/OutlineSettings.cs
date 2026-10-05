using System.Collections;
using UnityEngine;

namespace Protobot.Outlining
{
    [CreateAssetMenu(fileName = "New Outline Settings")]
    public class OutlineSettings : ScriptableObject
    {
        //Singleton set up
        private static OutlineSettings _instance;
        public static OutlineSettings instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<OutlineSettings>("General Data/Outline Settings");

                return _instance;
            }
            private set => _instance = value;
        }

        public OutlineSettings()
        {
            instance = this;
        }

        public static Color GetColor(int index)
        {
            return instance.colors[index];
        }

        public static float GetDefaultWidth()
        {
            return instance.defaultWidth;
        }

        public Color[] colors;

        [Space(10)]
        [Range(0, 10)]
        public float defaultWidth;
    }
}