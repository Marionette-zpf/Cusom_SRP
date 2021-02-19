using UnityEngine;

namespace Custom_SRP.Example
{
    /// <summary>
    /// Date    2021/2/19 9:58:42
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static MaterialPropertyBlock g_propertyBlock;
        private static int g_baseColorId { get; } = Shader.PropertyToID("_BaseColor");
        private static int g_cutoffId { get; } = Shader.PropertyToID("_Cutoff");
        private static int g_metallicId { get; } = Shader.PropertyToID("_Metallic");
        private static int g_smoothnessId { get; } = Shader.PropertyToID("_Smoothness");

        [SerializeField]
        private Color m_color = Color.white;

        [SerializeField, Range(0.0f, 1.0f)] private float m_cutoff = 0.5f;
        [SerializeField, Range(0.0f, 1.0f)] private float m_metallic = 0.5f;
        [SerializeField, Range(0.0f, 1.0f)] private float m_smoothness = 0.5f;

        void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if(g_propertyBlock == null)
            {
                g_propertyBlock = new MaterialPropertyBlock();
            }

            g_propertyBlock.SetColor(g_baseColorId, m_color);
            g_propertyBlock.SetFloat(g_cutoffId, m_cutoff);
            g_propertyBlock.SetFloat(g_metallicId, m_metallic);
            g_propertyBlock.SetFloat(g_smoothnessId, m_smoothness);

            GetComponent<Renderer>()?.SetPropertyBlock(g_propertyBlock);
        }
    }
}