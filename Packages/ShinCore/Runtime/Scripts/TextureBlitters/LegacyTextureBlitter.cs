using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
internal class LegacyTextureBlitter : BaseTextureBlitter {

    protected override void AwakeInternalV() { }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        BlitToDest(destination);
    }
    
}

} //end namespace