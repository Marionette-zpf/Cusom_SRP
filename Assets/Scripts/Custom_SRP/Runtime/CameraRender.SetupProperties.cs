﻿using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/19 15:10:54
    /// Name    A12771\Administrator
    /// Desc    setup properties
    /// </summary>
    public partial class CameraRender
    {
        private void SetupCommonProperties()
        {
            //setup camera properties
            SetupCameraProperties();
        }

        private void SetupShadowProperties()
        {
            m_shadowedDirectionalLightCount = 0;

            //setup lighting properties
            ExecuteSampleBuffer(buffer => SetupLightingProperties(buffer), m_lightingBuffer, LIGHT_BUFFER_NAME);
        }

        private void SetupLightingProperties(CommandBuffer buffer)
        {
            NativeArray<VisibleLight> visibleLights = m_cullingResult.visibleLights;

            int directionalLightCount = 0;
            for (int i = 0; i < visibleLights.Length; i++)
            {
                var light = visibleLights[i];
                if (light.lightType == LightType.Directional)
                {
                    SetupDirectionalLight(directionalLightCount++, ref light);
                }

                if(directionalLightCount >= MAX_DIR_LIGHT_COUNT)
                {
                    break;
                }
            }

            buffer.SetGlobalInt(g_dir_light_count_id, directionalLightCount);
            buffer.SetGlobalVectorArray(g_directional_light_color_id, m_dirLightColors);
            buffer.SetGlobalVectorArray(g_directional_light_direction_id, m_dirLightDirections);
        }

        private void SetupDirectionalLight(int index, ref VisibleLight light)
        {
            m_dirLightColors[index] = light.finalColor;
            m_dirLightDirections[index] = -light.localToWorldMatrix.GetColumn(2);
            //error?
            ReserveDirectionalShadows(light.light, index);
        }

        private void SetupCameraProperties()
        {
            m_renderContext.SetupCameraProperties(m_camera);
        }

        private void ReserveDirectionalShadows(Light light, int visibleLightIndex)
        {
            if (IsValidDirectionalShadows(light, visibleLightIndex))
            {
                m_shadowedDirectionalLights[m_shadowedDirectionalLightCount++] =
                    new ShadowedDirectionalLight
                    {
                        visibleLightIndex = visibleLightIndex
                    };
            }
        }

        private bool IsValidDirectionalShadows(Light light, int visibleLightIndex)
        {
            return m_shadowedDirectionalLightCount < MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT
                && light.shadows != LightShadows.None
                && light.shadowStrength > 0f
                && m_cullingResult.GetShadowCasterBounds(visibleLightIndex, out Bounds b);
        }
    }
}