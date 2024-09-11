using UnityEngine;

namespace Shin.Core {

internal interface IAnimationCurveOwner {
    void SetAnimationCurve(AnimationCurve curve);
    AnimationCurve  GetAnimationCurve();
}

} //end namespace
