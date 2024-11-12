using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule.Util;

//This example blits the active CameraColor to a new texture.
//It shows how to do a blit with material, and how to use the ResourceData to avoid another blit back to the active color target.
//This example is for API demonstrative purposes. 

// This pass blits the whole screen for a given material to a temp texture, and swaps the UniversalResourceData.cameraColor to this temp texture.
// Therefore, the next pass that references the cameraColor will reference this new temp texture as the cameraColor, saving us a blit. 
// Using the ResourceData, you can manage swapping of resources yourself and don't need a bespoke API like the SwapColorBuffer API that was specific for the cameraColor. 
// This allows you to write more decoupled passes without the added costs of avoidable copies/blits.
public class DitherEffectPass : ScriptableRenderPass {
    // Function used to transfer the material from the renderer feature to the render pass.
    public void Setup(Material mat) {
        m_blitMaterial = mat;

        //The pass will read the current color texture. That needs to be an intermediate texture. It's not supported to use the BackBuffer as input texture. 
        //By setting this property, URP will automatically create an intermediate texture. 
        //It's good practice to set it here and not from the RenderFeature.
        //This way, the pass is self containing and you can use it to directly enqueue the pass from a monobehaviour without a RenderFeature.
        requiresIntermediateTexture = true;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
        VolumeStack stack = VolumeManager.instance.stack;
        CustomDitherVolumeComponent customEffect = stack.GetComponent<CustomDitherVolumeComponent>();
        // Only process if the effect is active
        if (!customEffect.IsActive())
            return;

        // UniversalResourceData contains all the texture handles used by the renderer, including the active color and depth textures
        // The active color and depth textures are the main color and depth buffers that the camera renders into
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

        //This should never happen since we set m_Pass.requiresIntermediateTexture = true;
        //Unless you set the render event to AfterRendering, where we only have the BackBuffer. 
        if (resourceData.isActiveTargetBackBuffer) {
            Debug.LogError($"Skipping render pass. DitherEffectRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
            return;
        }

        // The destination texture is created here, 
        // the texture is created with the same dimensions as the active color texture
        TextureHandle source = resourceData.activeColorTexture;

        TextureDesc destinationDesc = renderGraph.GetTextureDesc(source);
        destinationDesc.name = $"CameraColor-{PASS_NAME}";
        destinationDesc.clearBuffer = false;

        TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

        RenderGraphUtils.BlitMaterialParameters para = new(source, destination, m_blitMaterial, 0);
        renderGraph.AddBlitPass(para, passName: PASS_NAME);

        //FrameData allows to get and set internal pipeline buffers. Here we update the CameraColorBuffer to the texture that we just wrote to in this pass. 
        //Because RenderGraph manages the pipeline resources and dependencies, following up passes will correctly use the right color buffer.
        //This optimization has some caveats. You have to be careful when the color buffer is persistent across frames and between different cameras, such as in camera stacking.
        //In those cases you need to make sure your texture is an RTHandle and that you properly manage the lifecycle of it.
        resourceData.cameraColor = destination;
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    const string PASS_NAME = "DitherEffectPass";
    Material m_blitMaterial; // Material used in the blit operation.
    
}
