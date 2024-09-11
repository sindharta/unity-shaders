
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Shin.Core {

internal static class TimelineClipExtensions {
    
    [CanBeNull]
    internal static T GetClipData<T>(this TimelineClip clip) where T: BaseClipData {
        
        BaseExtendedClipPlayableAsset<T> clipAsset = clip.asset as BaseExtendedClipPlayableAsset<T>;
        if (null == clipAsset)
            return null;
        
        T clipData = clipAsset.GetBoundClipData();
        return clipData;
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    internal static bool Contains<T>(this IEnumerable<TimelineClip> clips) where T : PlayableAsset {
        using var enumerator = clips.GetEnumerator();
        while (enumerator.MoveNext()) {
            TimelineClip clip = enumerator.Current;
            if (null == clip)
                continue;
            
            T asset = clip.asset as T;
            if (null != asset)
                return true;
        }

        return false;
    }
    
}

} //end namespace

