using System;
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
        private void RenderCommon()
        {
            ExecuteBuffer(buffer =>
            {
                var clearFlags = m_camera.clearFlags;
                buffer.ClearRenderTarget(
                    clearFlags <= CameraClearFlags.Depth,
                    clearFlags == CameraClearFlags.Color,
                    clearFlags == CameraClearFlags.Color ? m_camera.backgroundColor.linear : Color.red
                    );
                buffer.BeginSample(m_sampleName);
            });

            //render support shaders
            RenderSupportedSahders();

            //render unsupport shaders
            RenderUnsupportedShaders();

            //darw gizmos
            DrawGizmos();

            ExecuteBuffer(buffer =>
            {
                buffer.EndSample(m_sampleName);
            });
        }

        #region render shadow

        private void RenderShadows()
        {
            RenderDirectionalShadows(m_shadowBuffer);
        }
                     
        private void RenderDirectionalShadows(CommandBuffer buffer, string bufferName = SHADOW_BUFFER_NAME)
        {
            if(m_shadowedDirectionalLightCount > 0)
            {
                int atlasSize = (int)m_settings.ShadowSettings.directional.atlasSize;
                buffer.GetTemporaryRT(
                    g_dir_shadow_atlas_id, atlasSize, atlasSize,
                    32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap
                );
                buffer.SetRenderTarget(
                    g_dir_shadow_atlas_id,
                    RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
                );
                buffer.ClearRenderTarget(true, false, Color.clear);
                buffer.BeginSample(bufferName);

                for (int i = 0; i < m_shadowedDirectionalLightCount; i++)
                {
                    RenderDirectionalShadow(i, atlasSize, buffer);
                }


                buffer.EndSample(bufferName);
                ExecuteBuffer(buffer);
            }
            else
            {
                buffer.GetTemporaryRT(
                   g_dir_shadow_atlas_id, 1, 1,
                   32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap
                );
                ExecuteBuffer(buffer);
            }

        }

        private void RenderDirectionalShadow(int index, int tileSize, CommandBuffer buffer)
        {
            ShadowedDirectionalLight light = m_shadowedDirectionalLights[index];
            var shadowSettings = new ShadowDrawingSettings(m_cullingResult, light.visibleLightIndex);

            m_cullingResult.ComputeDirectionalShadowMatricesAndCullingPrimitives(
                light.visibleLightIndex, 0, 1, Vector3.zero, tileSize, 0f,
                out Matrix4x4 viewMatrix, out Matrix4x4 projectionMatrix,
                out ShadowSplitData splitData
            );

            shadowSettings.splitData = splitData;
            buffer.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
            ExecuteBuffer(buffer);

            m_renderContext.DrawShadows(ref shadowSettings);
        }

        #endregion

        #region render objs
        private void RenderSupportedSahders()
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

        #endregion


    }
}