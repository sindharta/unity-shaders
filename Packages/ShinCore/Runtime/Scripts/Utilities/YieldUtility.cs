using System.Collections;

namespace Shin.Core {

internal static class YieldUtility {
                
    internal static IEnumerator WaitForFrames(int numFrames) {
        
        for (int i = 0; i < numFrames; ++i) {
            yield return null;
            
        }
    }
}

} //end namespace