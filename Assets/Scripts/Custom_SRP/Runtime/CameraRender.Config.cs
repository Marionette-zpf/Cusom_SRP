using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/19 15:09:21
    /// Name    A12771\Administrator
    /// Desc    static config
    /// </summary>
    public partial class CameraRender
    {
        private const string CAMERA_BUFFER_NAME = "Render Camera";
        private const string LIGHT_BUFFER_NAME = "Lighting";
        private const string SHADOW_BUFFER_NAME = "Shadows";

        private const int MAX_DIR_LIGHT_COUNT = 4;
        private const int MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT = 1;

        private static readonly ShaderTagId g_unlit_shader_tag_id = new ShaderTagId("SRPDefaultUnlit");
        private static readonly ShaderTagId g_lit_shader_tag_id = new ShaderTagId("CustomLit");

        private static readonly int g_directional_light_color_id = Shader.PropertyToID("_DirectionalLightColors");
        private static readonly int g_directional_light_direction_id = Shader.PropertyToID("_DirectionalLightDirections");
        private static readonly int g_dir_light_count_id = Shader.PropertyToID("_DirectionalLightCount");
        private static readonly int g_dir_shadow_atlas_id = Shader.PropertyToID("_DirectionalShadowAtlas");
    }

    public struct ShadowedDirectionalLight
    {
        public int visibleLightIndex;
    }

}