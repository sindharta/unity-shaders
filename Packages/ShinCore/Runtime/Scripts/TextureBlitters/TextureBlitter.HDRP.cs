#if SHIN_CORE_USE_HDRP

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Shin.Core {

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(HDAdditionalCameraData))]
internal partial class TextureBlitter : MonoBehaviour {

    private void AwakeInternal() {
        m_hdData                       = GetComponent<HDAdditionalCameraData>();
        m_hdData.fullscreenPassthrough = true;
    }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnEnable() {
        UnityEngine.Rendering.RenderPipelineManager.endFrameRendering += OnEndFrameRendering;
    }

    private void OnDisable() {
        UnityEngine.Rendering.RenderPipelineManager.endFrameRendering -= OnEndFrameRendering; 
        
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    void OnEndFrameRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera[] cams) {
        
        //only blit for specified camera type
        foreach (Camera cam in cams) {
            if (cam.cameraType != m_targetCameraType)
                return;
        }

        if (m_outputToDisplay)
            ExecuteBlit(m_srcTexture, dest:null, m_destTexture);
        else
            ExecuteBlit(m_srcTexture, dest:m_destTexture);
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    internal void SetTargetCameraType(CameraType cameraType) { m_targetCameraType = cameraType; }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private CameraType m_targetCameraType = CameraType.Game;

    private HDAdditionalCameraData m_hdData;
}

} //end namespace 

#endif // SHIN_CORE_USE_HDRP