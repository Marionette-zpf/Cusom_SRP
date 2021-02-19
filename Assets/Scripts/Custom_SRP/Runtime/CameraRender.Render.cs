using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/19 15:12:00
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public partial class CameraRender
    {
        private void Render()
        {
            ExecuteBuffer(buffer =>
            {
                var clearFlags = m_camera.clearFlags;
                buffer.ClearRenderTarget(
                    clearFlags <= CameraClearFlags.Depth,
                    clearFlags == CameraClearFlags.Color,
                    clearFlags == CameraClearFlags.Color ? m_camera.backgroundColor.linear : Color.clear
                    );
                buffer.BeginSample(m_sampleName);
            });

            //draw support shaders
            DrawSupportedSahders();

            //draw unsupport shaders
            DrawUnsupportedShaders();

            //darw gizmos
            DrawGizmos();

            ExecuteBuffer(buffer =>
            {
                buffer.EndSample(m_sampleName);
            });
        }

        private void DrawSupportedSahders()
        {
            //settings
            var sortingSettings = new SortingSettings(m_camera) { criteria = SortingCriteria.CommonOpaque };
            var drawingSettings = new DrawingSettings(g_unlit_shader_tag_id, sortingSettings)
            {
                enableDynamicBatching = m_settings.UseDynamicBatching,
                enableInstancing = m_settings.UseGpuInstancing
            };
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            drawingSettings.SetShaderPassName(1, g_lit_shader_tag_id);

            //draw opaque objs
            m_renderContext.DrawRenderers(m_cullingResult, ref drawingSettings, ref filteringSettings);

            //draw skybox
            m_renderContext.DrawSkybox(m_camera);

            //draw transparent objs
            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            m_renderContext.DrawRenderers(m_cullingResult, ref drawingSettings, ref filteringSettings);
        }
    }
}