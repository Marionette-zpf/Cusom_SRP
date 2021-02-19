using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline Asset")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private bool m_useDynamicBatching = true, m_useGpuInstancing = true, m_useSRPBatcher = true;

        protected override RenderPipeline CreatePipeline()
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = m_useSRPBatcher;
            GraphicsSettings.lightsUseLinearIntensity = true;

            PipelineSettings settings = new PipelineSettings();
            settings.UseDynamicBatching = m_useDynamicBatching;
            settings.UseGpuInstancing = m_useGpuInstancing;

            return new CustomRenderPipeline(settings);
        }
    }

    public struct PipelineSettings
    {
        public bool UseDynamicBatching;
        public bool UseGpuInstancing;
    }


}
