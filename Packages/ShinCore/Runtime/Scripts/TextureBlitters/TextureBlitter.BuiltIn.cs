#if !SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP

using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
internal partial class TextureBlitter : MonoBehaviour {
    private void AwakeInternal() { }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        BlitToDest(m_srcTexture, destination);
        
        if (null != m_destTexture) {
            BlitToDest(m_srcTexture, m_destTexture);
        }
    }
    
}

} //end namespace

#endif //!SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP