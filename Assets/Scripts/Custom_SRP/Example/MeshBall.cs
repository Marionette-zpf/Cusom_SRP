﻿using UnityEngine;

namespace Custom_SRP.Example
{
    /// <summary>
    /// Date    2021/2/19 10:23:28
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class MeshBall : MonoBehaviour
    {
        private const int RENDER_COUNT = 1023;
        private static int g_baseColorId { get; } = Shader.PropertyToID("_BaseColor");
        private static int g_metallicId { get; } = Shader.PropertyToID("_Metallic");
        private static int g_smoothnessId { get; } = Shader.PropertyToID("_Smoothness");

        [SerializeField] private Mesh m_mesh = default;
        [SerializeField] private Material m_material;
        [SerializeField] private float m_radius = 10;

        private MaterialPropertyBlock m_propertyBlock;
        private Matrix4x4[] m_transformMatrixs;
        private Vector4[] m_colors;
        private float[] m_metallic;
        private float[] m_smoothness;

        private void Awake()
        {
            m_propertyBlock = new MaterialPropertyBlock();
            m_transformMatrixs = new Matrix4x4[RENDER_COUNT];
            m_colors = new Vector4[RENDER_COUNT];
            m_metallic = new float[RENDER_COUNT];
            m_smoothness = new float[RENDER_COUNT];

            for (int i = 0; i < RENDER_COUNT; i++)
            {
                m_transformMatrixs[i] = Matrix4x4.TRS(
                    Random.insideUnitSphere * m_radius, 
                    Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                    Vector3.one * Random.Range(0.5f, 1.5f)
                    );
                var color = Random.insideUnitSphere;
                m_colors[i] = new Vector4(color.x, color.y, color.z, Random.Range(0.5f, 1.0f));
                m_metallic[i] = Random.Range(0.0f, 1.0f) > 0.5f ? 0.0f : 1.0f;
                m_smoothness[i] = Random.value;
            }

            m_propertyBlock.SetVectorArray(g_baseColorId, m_colors);
            m_propertyBlock.SetFloatArray(g_metallicId, m_metallic);
            m_propertyBlock.SetFloatArray(g_smoothnessId, m_smoothness);
        }

        void Update()
        {
            Graphics.DrawMeshInstanced(m_mesh, 0, m_material, m_transformMatrixs, RENDER_COUNT, m_propertyBlock);
        }
    }
}