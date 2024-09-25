#if SHIN_CORE_USE_URP

using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
internal class URPTextureBlitter : TextureBlitter {
    
    protected override void AwakeInternalV() { }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void OnEnable() {
        UnityEngine.Rendering.RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }


    private void OnDisable() {
        UnityEngine.Rendering.RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnEndCameraRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera cam) {
        if (cam == GetCamera() && null != GetSrcTexture()) {
            BlitToDest(null);
        }
    } 
}

} //end namespace

#endif //SHIN_CORE_USE_URP