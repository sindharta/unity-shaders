﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Assertions;
using UnityEngine.Timeline;

namespace Shin.Core.Editor {

internal static class ExtendedClipEditorUtility {
    
    internal static void ResetClipDataCurve<T>(BaseExtendedClipPlayableAsset<T> playableAsset, EditorCurveBinding curveBinding) 
        where T: BaseClipData, IAnimationCurveOwner
    {
                
        T clipData = playableAsset.GetBoundClipData();        
        Assert.IsNotNull(clipData);

        TimelineClip clip = clipData.GetOwner();
        Assert.IsNotNull(clip);
        
        AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, (float) (clip.duration * clip.timeScale),1 );
        clipData.SetAnimationCurve(animationCurve);        
        SetTimelineClipCurve(clip, animationCurve, curveBinding);
        clip.clipIn = 0;
    }
       
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    
    
    internal static bool SetClipDataCurve<T>(BaseExtendedClipPlayableAsset<T> playableAsset, 
        AnimationCurve srcCurve) 
        where T: BaseClipData, IAnimationCurveOwner
    {
       
        IAnimationCurveOwner clipData = playableAsset.GetBoundClipData() as IAnimationCurveOwner;
        if (null == clipData) {
            //The srcClip is not ready. Perhaps the deserialization is not finished yet
            return false;
        }
               
        clipData.SetAnimationCurve(srcCurve);            
        return true;
    }

    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    
    internal static void CreateTimelineClipCurve(TimelineClip clip, EditorCurveBinding curveBinding) {        
        clip.CreateCurves("Curves: " + clip.displayName);
                
        AnimationCurve curve = CreateDefaultAnimationCurve(clip);
        SetTimelineClipCurve(clip,curve, curveBinding);

    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    

    //Make sure that TimelineClip has a curve set
    internal static AnimationCurve ValidateTimelineClipCurve(TimelineClip clip, EditorCurveBinding curveBinding)         
    {
        AnimationCurve curve = null;
        if (null == clip.curves) {
            clip.CreateCurves("Curves: " + clip.displayName);
        } else {
            curve = AnimationUtility.GetEditorCurve(clip.curves, curveBinding);            
        }        
        
        if (null == curve) {
            curve = CreateDefaultAnimationCurve(clip);
            SetTimelineClipCurve(clip,curve, curveBinding);
        }

        return curve;
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    private static AnimationCurve CreateDefaultAnimationCurve(TimelineClip clip) {
        return AnimationCurve.Linear(0f,0f,(float)(clip.duration * clip.timeScale),1f);        
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------


    private static void SetTimelineClipCurve(TimelineClip destClip, AnimationCurve srcCurve, EditorCurveBinding curveBinding) {
        AnimationUtility.SetEditorCurve(destClip.curves, curveBinding, srcCurve);
        
        TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw );
        
    }
}

} //end namespace


