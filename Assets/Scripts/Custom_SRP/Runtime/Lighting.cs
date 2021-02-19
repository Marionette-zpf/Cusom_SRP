using UnityEngine;
using UnityEngine.Rendering;

namespace Custom_SRP.Runtime
{
    /// <summary>
    /// Date    2021/2/19 14:19:02
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class Lighting
    {
		private const string LIGHT_BUFFER_NAME = "Lighting";

		private static readonly int g_directional_light_color_id = Shader.PropertyToID("_DirectionalLightColor");
		private static readonly int g_directional_light_direction_id = Shader.PropertyToID("_DirectionalLightDirection");

		private CommandBuffer m_buffer = new CommandBuffer
		{
			name = LIGHT_BUFFER_NAME
		};

		public void Setup(ScriptableRenderContext context)
		{
			m_buffer.BeginSample(LIGHT_BUFFER_NAME);
			SetupDirectionalLight();
			m_buffer.EndSample(LIGHT_BUFFER_NAME);
			context.ExecuteCommandBuffer(m_buffer);
			m_buffer.Clear();
		}

		void SetupDirectionalLight() 
		{
			Light light = RenderSettings.sun;
			m_buffer.SetGlobalVector(g_directional_light_color_id, light.color.linear * light.intensity);
			m_buffer.SetGlobalVector(g_directional_light_direction_id, -light.transform.forward);
		}
	}
}