﻿#if SHIN_CORE_USE_URP

using UnityEngine;

namespace Shin.Core {

internal partial class TextureBlitter : MonoBehaviour {
    
    private void AwakeInternal() { }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void OnEnable() {
        UnityEngine.Rendering.RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }


    private void OnDisable() {
        UnityEngine.Rendering.RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void OnEndCameraRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera cam) {
        if (cam != m_camera) return;
        
        if (m_outputToDisplay)
            ExecuteBlit(m_srcTexture, primaryDest:null, m_destTexture);
        else
            ExecuteBlit(m_srcTexture, primaryDest:m_destTexture);
    } 
}

} //end namespace

#endif //SHIN_CORE_USE_URP