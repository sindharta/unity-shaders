#if !SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP

using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
internal partial class TextureBlitter : MonoBehaviour {
    private void AwakeInternal() { }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        BlitToDest(source, destination);
        
        if (null != m_destTexture) {
            BlitToDest(source, m_destTexture);
        }
    }
    
}

} //end namespace

#endif //!SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP