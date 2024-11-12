using UnityEngine;
using UnityEngine.Rendering.Universal;

//This example blits the active CameraColor to a new texture.
//It shows how to do a blit with material, and how to use the ResourceData to avoid another blit back to the active color target.
//This example is for API demonstrative purposes. 

public class DitherEffectRendererFeature : ScriptableRendererFeature {    

    // Here you can create passes and do the initialization of them. This is called everytime serialization happens.
    public override void Create() {
        m_pass = new DitherEffectPass {
            renderPassEvent = m_injectionPoint
        };
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        // Early exit if there are no materials.
        if (m_blitMaterial == null) {
            Debug.LogWarning("DitherEffectRendererFeature material is null and will be skipped.");
            return;
        }

        m_pass.Setup(m_blitMaterial);
        renderer.EnqueuePass(m_pass);        
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [Tooltip("The material used when making the blit operation.")]
    [SerializeField] private Material m_blitMaterial;

    [Tooltip("The event where to inject the pass.")]
    [SerializeField] private RenderPassEvent m_injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;

    private DitherEffectPass m_pass;
    
}

