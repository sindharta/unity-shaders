using System.Collections;
using UnityEditor;

namespace Shin.Core.Editor {

internal static class YieldEditorUtility {
                    
    internal static IEnumerator WaitForFramesAndIncrementUndo(int numFrames) {
        yield return YieldUtility.WaitForFrames(numFrames);
        Undo.IncrementCurrentGroup();
    }
}

} //end namespace