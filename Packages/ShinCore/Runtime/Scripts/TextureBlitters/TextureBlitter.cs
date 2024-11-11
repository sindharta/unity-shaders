using UnityEngine;
using UnityEngine.Serialization;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
internal partial class TextureBlitter : MonoBehaviour {
    
    void Awake() {
        m_camera = GetComponent<Camera>();
        AwakeInternal();
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    //primaryDest: null means the default output display for SRP.
    private void ExecuteBlit(Texture src, RenderTexture primaryDest, RenderTexture additionalDest = null) {

        bool additionalBlit = null != additionalDest; 

        if (null == m_blitMaterial) {
            Graphics.Blit(src, primaryDest);
            if (additionalBlit)
                Graphics.Blit(src, additionalDest);
            return;
        }
        
        Graphics.Blit(src, primaryDest, m_blitMaterial);
        if (additionalBlit)
            Graphics.Blit(src, additionalDest, m_blitMaterial);
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    internal void SetSrcTexture(Texture tex) { m_srcTexture = tex; }
    internal void SetDestTexture(RenderTexture rt) { m_destTexture = rt; }
    
    internal void SetBlitMaterial(Material blitMat) { m_blitMaterial = blitMat; }
    internal void SetCameraDepth(int depth) { m_camera.depth = depth; }

    void SetOutputToDisplay(bool outputToDisplay) {
        m_outputToDisplay = outputToDisplay;
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Texture       m_srcTexture;
    [SerializeField] private RenderTexture m_destTexture; //if null, blits to screen
    
    [SerializeField] Material m_blitMaterial = null;

    [SerializeField] private bool m_outputToDisplay = true;
    
    private Camera m_camera;
}

} //end namespace