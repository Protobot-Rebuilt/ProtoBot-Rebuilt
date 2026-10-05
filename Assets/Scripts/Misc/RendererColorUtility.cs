using UnityEngine;

namespace Protobot
{
    public static class RendererColorUtility
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        public static bool IsMetallicSelectable(Renderer renderer)
        {
            if (renderer == null || renderer.sharedMaterial == null)
                return false;

            var material = renderer.sharedMaterial;
            return material.HasProperty("_Metallic") && Mathf.Approximately(material.GetFloat("_Metallic"), 0.754f);
        }

        public static void SetTintColor(Renderer renderer, Color color)
        {
            if (renderer == null)
                return;

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(ColorId, color);
            block.SetColor(BaseColorId, color);
            renderer.SetPropertyBlock(block);
        }

        public static Color GetTintColor(Renderer renderer)
        {
            if (renderer == null)
                return Color.white;

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);

            if (block.HasColor(BaseColorId))
                return block.GetColor(BaseColorId);

            if (block.HasColor(ColorId))
                return block.GetColor(ColorId);

            var material = renderer.sharedMaterial;
            if (material == null)
                return Color.white;

            if (material.HasProperty(BaseColorId))
                return material.GetColor(BaseColorId);

            if (material.HasProperty(ColorId))
                return material.GetColor(ColorId);

            return Color.white;
        }
    }
}