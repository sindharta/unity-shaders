using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom/Dither")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class CustomDitherVolumeComponent : VolumeComponent, IPostProcessComponent {
    [SerializeField] private ClampedFloatParameter m_intensity = new ClampedFloatParameter(value: 0, min: 0, max: 1, overrideState: true);
    
    public bool IsActive() => m_intensity.value > 0; // Tells when our effect should be rendered
}