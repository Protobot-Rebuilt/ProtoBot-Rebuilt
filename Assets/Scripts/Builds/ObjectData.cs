using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Serialization;

namespace Protobot.Builds
{
    [Serializable]
    public class ObjectData
    {
        public double xPos, yPos, zPos;
        public double xRot, yRot, zRot;
        public double rColor, gColor, bColor;
        public string states;
        [FormerlySerializedAs("meshId")] public string partId;
        public Vector3 GetPos() => new Vector3((float)xPos, (float)yPos, (float)zPos);
        public Quaternion GetRot() => Quaternion.Euler((float)xRot, (float)yRot, (float)zRot);
        public Color GetColor() => new Color((float)rColor, (float)gColor, (float)bColor, 1);

        public override bool Equals(object obj)
        {
            var data = obj as ObjectData;
            if (data == null) return false;

            if (data.GetPos() != GetPos()) return false;
            if (data.GetRot() != GetRot()) return false;
            if (data.partId != partId) return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + GetPos().GetHashCode();
                hash = hash * 23 + GetRot().GetHashCode();
                hash = hash * 23 + (partId != null ? partId.GetHashCode() : 0);
                return hash;
            }
        }
    }
}