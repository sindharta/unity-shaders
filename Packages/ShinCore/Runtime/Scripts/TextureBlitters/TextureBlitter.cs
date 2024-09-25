using UnityEngine;
using UnityEngine.Serialization;

namespace Shin.Core {

[RequireComponent(typeof(Camera))]
internal partial class TextureBlitter : MonoBehaviour {
    
    void Awake() {
        m_camera = GetComponent<Camera>();
        AwakeInternal();
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void BlitToDest(RenderTexture destination) {

        if (null == m_blitMaterial) {
            Graphics.Blit(m_srcTexture, destination);
            return;
        }
        
        Graphics.Blit(m_srcTexture, destination, m_blitMaterial);
        
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    internal void SetSrcTexture(Texture tex) { m_srcTexture = tex; }
    internal void SetDestTexture(RenderTexture rt) { m_destTexture = rt; }
    
    internal void SetBlitMaterial(Material blitMat) { m_blitMaterial = blitMat; }
    internal void SetCameraDepth(int depth) { m_camera.depth = depth; }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Texture       m_srcTexture;
    [SerializeField] private RenderTexture m_destTexture; //if null, blits to screen
    
    [SerializeField] Material m_blitMaterial = null;
    
    private Camera m_camera;
}

} //end namespace