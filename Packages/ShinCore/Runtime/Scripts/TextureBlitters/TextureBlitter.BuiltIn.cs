#if !SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP

using UnityEngine;

namespace Shin.Core {

internal partial class TextureBlitter : MonoBehaviour {
    private void AwakeInternal() { }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (m_outputToDisplay)
            ExecuteBlit(m_srcTexture, destination, m_destTexture);
        else {
            Graphics.Blit(source,destination);
            ExecuteBlit(m_srcTexture, m_destTexture);
        }
    }
    
}

} //end namespace

#endif //!SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP