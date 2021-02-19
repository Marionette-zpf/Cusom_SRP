using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/18 13:21:34
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class CustomRenderPipeline : RenderPipeline
    {
        private CameraRender m_cameraRender = new CameraRender();

        private PipelineSettings m_settings;

        public CustomRenderPipeline(PipelineSettings settings)
        {
            m_settings = settings;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                m_cameraRender.Render(context, camera, m_settings);
            }
        }
    }
}