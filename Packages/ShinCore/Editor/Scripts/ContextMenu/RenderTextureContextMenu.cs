using System;
using UnityEditor;
using UnityEngine;

namespace Shin.Core.Editor {

public static class RenderTextureToFile {

    [MenuItem(SAVE_RT_TO_RGB32)]
    private static void SaveRT_RGB32() {
        RenderTexture rt = (RenderTexture)Selection.activeObject;
        if (rt == null)
            return;

        string path = EditorUtility.SaveFilePanel("Save RenderTexture...", Application.dataPath, rt.name, "png");

        if (string.IsNullOrEmpty(path))
            return;
        
        rt.WriteToFile(path, TextureFormat.RGBA32, isPNG:true);
    }

    [MenuItem(SAVE_RT_TO_HDR)]
    private static void SaveRT_HDR() {
        RenderTexture rt = (RenderTexture)Selection.activeObject;
        if (rt == null)
            return;

        string path = EditorUtility.SaveFilePanel("Save RenderTexture...", Application.dataPath, rt.name, "exr");

        if (string.IsNullOrEmpty(path))
            return;
        
        rt.WriteToFile(path, TextureFormat.RGBAFloat, isPNG:false);
        
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    

    [MenuItem(SAVE_RT_TO_RGB32, true)]
    private static bool ValidateSaveRT_RGB32() {
        return IsRenderTextureSelected();
    }

    [MenuItem(SAVE_RT_TO_HDR, true)]
    private static bool ValidateSaveRT_HDR() {
        return IsRenderTextureSelected();
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    private static bool IsRenderTextureSelected() {
        return Selection.activeObject is RenderTexture;
    }

    private const string SAVE_RT_TO_RGB32 = "Assets/ShinCore/To RGB32";
    private const string SAVE_RT_TO_HDR = "Assets/ShinCore/To HDR";

}

} //end namespace