﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/18 13:24:08
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public partial class CameraRender
    {
        private CommandBuffer m_commandBuffer = new CommandBuffer()
        {
            name = CAMERA_BUFFER_NAME
        };

        private CommandBuffer m_lightingBuffer = new CommandBuffer()
        {
            name = LIGHT_BUFFER_NAME
        };


        private ScriptableRenderContext m_renderContext;
        private Camera m_camera;
        private PipelineSettings m_settings;
        private CullingResults m_cullingResult;

        private Vector4[] m_dirLightColors = new Vector4[MAX_DIR_LIGHT_COUNT];
        private Vector4[] m_dirLightDirections = new Vector4[MAX_DIR_LIGHT_COUNT];


        public void Render(ScriptableRenderContext renderContext, Camera camera, PipelineSettings settings)
        {
            m_renderContext = renderContext;
            m_camera = camera;
            m_settings = settings;

            //-------------------prepare for editor mode-----------------
            PrepareForSceneWindow();
            PrepareBuffer();


            //-------------------------culling---------------------------
            if (!Culling(out m_cullingResult)) return;


            //-------------------------setup properties------------------
            SetupProperties();


            //-------------------------redner----------------------------
            Render();


            //---------------------submit render---------------------------
            Submit();
        }

        private bool Culling(out CullingResults cullingResults)
        {
            if(m_camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters))
            {
                cullingResults = m_renderContext.Cull(ref cullingParameters);
                return true;
            }

            cullingResults = default;
            return false;
        }

        private void Submit()
        {
            m_renderContext.Submit();
        }

        private void ExecuteBuffer(Action<CommandBuffer> execute)
        {
            ExecuteBuffer(execute, m_commandBuffer);
        }

        private void ExecuteBuffer(Action<CommandBuffer> execute, CommandBuffer buffer)
        {
            execute?.Invoke(buffer);
            m_renderContext.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        private void ExecuteSampleBuffer(Action<CommandBuffer> execute, CommandBuffer buffer, string sampleName)
        {
            buffer.BeginSample(sampleName);
            execute?.Invoke(buffer);
            buffer.EndSample(sampleName);

            m_renderContext.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        private void ExecuteSampleBuffer(Action<CommandBuffer> execute, string sampleName)
        {
            ExecuteSampleBuffer(execute, m_commandBuffer, sampleName);
        }
    }


}