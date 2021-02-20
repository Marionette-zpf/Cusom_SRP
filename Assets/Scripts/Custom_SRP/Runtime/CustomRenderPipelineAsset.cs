using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline Asset")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private bool m_useDynamicBatching = true, m_useGpuInstancing = true, m_useSRPBatcher = true;

        [SerializeField]
        private ShadowSettings m_shadowSettings = default;

        private PipelineSettings m_settings = new PipelineSettings();

        protected override RenderPipeline CreatePipeline()
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = m_useSRPBatcher;
            GraphicsSettings.lightsUseLinearIntensity = true;

            m_settings = new PipelineSettings();
            m_settings.UseDynamicBatching = m_useDynamicBatching;
            m_settings.UseGpuInstancing = m_useGpuInstancing;
            m_settings.ShadowSettings = m_shadowSettings;

            return new CustomRenderPipeline(m_settings);
        }
    }

    public class PipelineSettings
    {
        public bool UseDynamicBatching;
        public bool UseGpuInstancing;

        public ShadowSettings ShadowSettings;
    }

    [System.Serializable]
    public class ShadowSettings
    {
        [Min(0f)]
        public float MaxDistance = 100f;

        public Directional directional = new Directional
        {
            atlasSize = TextureSize._1024
        };
    }


    [System.Serializable]
    public struct Directional
    {
        public TextureSize atlasSize;
    }

    public enum TextureSize
    {
        _256 = 256, _512 = 512, _1024 = 1024,
        _2048 = 2048, _4096 = 4096, _8192 = 8192
    }
}
