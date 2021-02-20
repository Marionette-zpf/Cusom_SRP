using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Custom_SRP.Runtime
{


    /// <summary>
    /// Date    2021/2/18 16:46:48
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public partial class CameraRender
    {
        partial void RenderUnsupportedShaders();
        partial void DrawGizmos();
        partial void PrepareForSceneWindow();
        partial void PrepareBuffer();

#if UNITY_EDITOR
        private static readonly ShaderTagId[] g_legacy_shader_tag_ids = {
                new ShaderTagId("Always"),
                new ShaderTagId("ForwardBase"),
                new ShaderTagId("PrepassBase"),
                new ShaderTagId("Vertex"),
                new ShaderTagId("VertexLMRGBM"),
                new ShaderTagId("VertexLM")
            };

        private static Material g_error_material { get; } = new Material(Shader.Find("Hidden/InternalErrorShader"));

        private string m_sampleName { get; set; }

        partial void RenderUnsupportedShaders()
        {
            var sortingSettings = new SortingSettings(m_camera);
            var drawingSettings = new DrawingSettings(g_legacy_shader_tag_ids[0], sortingSettings) { overrideMaterial = g_error_material };
            var filteringSettings = new FilteringSettings(RenderQueueRange.all);

            for (int i = 1; i < g_legacy_shader_tag_ids.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, g_legacy_shader_tag_ids[i]);
            }

            m_renderContext.DrawRenderers(m_cullingResult, ref drawingSettings, ref filteringSettings);
        }

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                m_renderContext.DrawGizmos(m_camera, GizmoSubset.PreImageEffects);
                m_renderContext.DrawGizmos(m_camera, GizmoSubset.PostImageEffects);
            }
        }

        partial void PrepareForSceneWindow()
        {
            if (m_camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(m_camera);
            }
        }
        partial void PrepareBuffer()
        {
            Profiler.BeginSample("Editor Only");
            m_commandBuffer.name = m_sampleName = m_camera.name;
            Profiler.EndSample();
        }

#else
	private const string m_sampleName = CAMERA_BUFFER_NAME;
#endif
    }
}