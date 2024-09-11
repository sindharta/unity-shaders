using System.Collections;
using UnityEngine;

namespace Shin.Core {

public static class UnityUtility  {

    //[Note-sin: 2019-10-24] May not work for all Unity built-in coroutines
    public static void WaitCoroutine(IEnumerator coroutineFunc) {
        while (coroutineFunc.MoveNext ()) {
            if (null == coroutineFunc.Current)
                continue;
            
            IEnumerator yieldedFunc = coroutineFunc.Current as IEnumerator;
            if (null == yieldedFunc) {
                Debug.LogError("Can't cast to IEnumerator: " + coroutineFunc.Current.ToString() );
                return;
            }
            WaitCoroutine (yieldedFunc);
        }
    }
}

}
