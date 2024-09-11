
using UnityEngine.Timeline;

namespace Unity.FilmInternalUtilities {

internal static class TimelineAssetExtensions {

    internal static double GetFPS(this TimelineAsset.EditorSettings editorSettings) {
        
        return editorSettings.frameRate;
    }
    
    
}

} //end namespace

