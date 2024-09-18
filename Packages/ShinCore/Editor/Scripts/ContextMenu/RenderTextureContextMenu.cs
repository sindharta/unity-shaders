using System;
using UnityEditor;
using UnityEngine;

namespace Shin.Core.Editor {

public static class RenderTextureToFile {

    [MenuItem(SAVE_RT_TO_RGB32, false,  (int) RenderTextureContextPriority.ToRGB32)]
    private static void SaveRT_RGB32() {
        OpenFilePanelAndSaveRT((RenderTexture)Selection.activeObject, "png", TextureFormat.RGBA32, true);
    }

    [MenuItem(SAVE_RT_TO_HDR, false,  (int) RenderTextureContextPriority.ToHDR)]
    private static void SaveRT_HDR() {
        OpenFilePanelAndSaveRT((RenderTexture)Selection.activeObject, "exr", TextureFormat.RGBAFloat, false);
    }

    private static void OpenFilePanelAndSaveRT(RenderTexture rt, string ext, TextureFormat texFormat, bool isPNG) {
        if (rt == null)
            return;

        string path = EditorUtility.SaveFilePanel("Save RenderTexture...", Application.dataPath, rt.name, ext);

        if (string.IsNullOrEmpty(path))
            return;
        
        rt.WriteToFile(path, texFormat, isPNG);

        string normalizedPath = AssetEditorUtility.NormalizePath(path);
        if (AssetUtility.IsAssetPath(normalizedPath, out _)) {
            AssetDatabase.Refresh();
            AssetEditorUtility.PingAssetByPath(normalizedPath);
        }
    }
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    
    [MenuItem(SAVE_RT_TO_HDR, true)]
    [MenuItem(SAVE_RT_TO_RGB32, true)]
    private static bool IsRenderTextureSelected() {
        return Selection.activeObject is RenderTexture;
    }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------    

    private const string SAVE_RT_TO_RGB32 = "Assets/ShinCore/To RGB32";
    private const string SAVE_RT_TO_HDR   = "Assets/ShinCore/To HDR";
    
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    private enum RenderTextureContextPriority {
        ToRGB32 = 1000,
        ToHDR
    }

}

} //end namespace