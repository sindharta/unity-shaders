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

        rt.WriteToFile(path, TextureFormat.RGBA32, isPNG:true, !rt.sRGB);
    }


    [MenuItem(SAVE_RT_TO_RGB32, true)]
    private static bool ValidateSaveRT_RGB32() {
        return IsRenderTextureSelected();
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    private static bool IsRenderTextureSelected() {
        return Selection.activeObject is RenderTexture;
    }

    private const string SAVE_RT_TO_RGB32 = "Assets/To RGB32";

}

} //end namespace