﻿using UnityEngine;
using UnityEngine.Serialization;

namespace Shin.Core {

[RequireComponent(typeof(Camera))]
internal partial class TextureBlitter : MonoBehaviour {
    
    void Awake() {
        m_camera = GetComponent<Camera>();
        
        //Render nothing
        m_camera.clearFlags  = CameraClearFlags.Depth;
        m_camera.cullingMask = 0;

        AwakeInternal();
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void BlitToDest(RenderTexture destination) {
        if (null == m_srcTexture) 
            return;

        if (null == m_blitMaterial) {
            Graphics.Blit(m_srcTexture, destination);
            return;
        }
        
        Graphics.Blit(m_srcTexture, destination, m_blitMaterial);
        
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    internal void SetSrcTexture(Texture tex) { m_srcTexture = tex; }
    
    internal void SetBlitMaterial(Material blitMat) { m_blitMaterial = blitMat; }
    internal void SetCameraDepth(int depth) { m_camera.depth = depth; }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [FormerlySerializedAs("m_texture")] [SerializeField] private Texture  m_srcTexture;    
    [SerializeField] Material m_blitMaterial = null;
    
    private Camera m_camera;
}

} //end namespace