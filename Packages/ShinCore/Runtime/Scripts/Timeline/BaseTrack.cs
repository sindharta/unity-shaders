using UnityEngine.Timeline;

namespace Shin.Core
{    
internal abstract class BaseTrack : TrackAsset {
    
    internal virtual int GetCapsV() { return 0; }    
}

} //end namespace
