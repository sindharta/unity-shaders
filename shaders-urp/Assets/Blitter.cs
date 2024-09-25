using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.StreamingImageSequence {

    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    internal class Blitter : MonoBehaviour {    
    
        void Awake() {
            m_camera = GetComponent<Camera>();
        
            //Render nothing
//            m_camera.clearFlags  = CameraClearFlags.Depth;
  //          m_camera.cullingMask = 0;
        }
    
//---------------------------------------------------------------------------------------------------------------------
    
        private void OnEnable() {
            UnityEngine.Rendering.RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }


        private void OnDisable() {
            UnityEngine.Rendering.RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }
    

//----------------------------------------------------------------------------------------------------------------------    
    
        void OnEndCameraRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera cam) {
            if (cam == GetCamera()) {
                BlitToDest(m_dest);
            }
        } 

    
//----------------------------------------------------------------------------------------------------------------------    

        protected void BlitToDest(RenderTexture destination) {
            // if (null == m_srcTexture) 
            //     return;

            if (null == m_blitMaterial) {
                Graphics.Blit(m_srcTexture, destination);
                return;
            }
        
            Graphics.Blit(m_srcTexture, destination, m_blitMaterial);
        
        }

//----------------------------------------------------------------------------------------------------------------------    

        internal void SetSrcTexture(Texture tex) { m_srcTexture = tex; }
        protected Texture GetSrcTexture() { return m_srcTexture; }
    
        internal void SetBlitMaterial(Material blitMat) { m_blitMaterial = blitMat; }
        internal void SetCameraDepth(int depth) { m_camera.depth = depth; }

        protected Camera GetCamera() { return m_camera; }
    
//----------------------------------------------------------------------------------------------------------------------    

        [FormerlySerializedAs("m_texture")] [SerializeField] private Texture  m_srcTexture;    
        [SerializeField] Material m_blitMaterial = null;


        [SerializeField] private RenderTexture m_dest; 
    
        private Camera m_camera;
    }

} //end namespace