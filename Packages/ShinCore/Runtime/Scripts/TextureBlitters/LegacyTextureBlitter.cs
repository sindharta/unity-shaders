#if !SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP


using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
internal class LegacyTextureBlitter : TextureBlitter {

    protected override void AwakeInternalV() { }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        BlitToDest(destination);
    }
    
}

} //end namespace

#endif //!SHIN_CORE_USE_URP && !SHIN_CORE_USE_HDRP