using UnityEngine;
using UnityEngine.UIElements;
namespace _20Advanced2DShaderEffects.CustomLitAndEmission_FX
{
    public class ShaderLightRotator : MonoBehaviour
    {
        [SerializeField]
        private Material m_Material;
        [SerializeField]
        private float m_RotationSpeed;
        [SerializeField]
        private float m_RotationSpread;
        [SerializeField]
        private float m_XRotation;
        
        private readonly int m_LightDirection = Shader.PropertyToID("_LightDirection");
        
        void Update()
        {
            var matAngle = transform.rotation * Vector3.forward;
            m_Material.SetVector(m_LightDirection, matAngle);
            
            float rotationAngle = Mathf.Sin(Time.time * m_RotationSpeed);
            rotationAngle *= m_RotationSpread;
            transform.rotation = Quaternion.Euler(m_XRotation, rotationAngle, 0);
        }
    }
}
