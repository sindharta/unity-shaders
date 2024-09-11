
using UnityEngine.Timeline;

namespace Shin.Core {

internal static class TimelineAssetExtensions {

    internal static double GetFPS(this TimelineAsset.EditorSettings editorSettings) {
        
        return editorSettings.frameRate;
    }
    
    
}

} //end namespace

