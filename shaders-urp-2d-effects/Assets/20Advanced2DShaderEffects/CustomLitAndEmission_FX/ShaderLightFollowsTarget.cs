using UnityEngine;
namespace _20Advanced2DShaderEffects.CustomLitAndEmission_FX
{
    [ExecuteInEditMode]
    public class ShaderLightFollowsTarget : MonoBehaviour
    {
        [SerializeField]
        private Material m_Material;
        [SerializeField]
        private Transform m_Target;
        private readonly int m_TargetPosition = Shader.PropertyToID("_TargetPosition");

        // Update is called once per frame
        void Update()
        {
            m_Material.SetVector(m_TargetPosition, m_Target.position);
        }
    }
}
