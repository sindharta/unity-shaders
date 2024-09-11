using UnityEngine;

namespace Shin.Core {

[ExecuteAlways]
public class CachedBehaviour : MonoBehaviour {

    public Transform GetTransform() { return m_transform; }

//----------------------------------------------------------------------------------------------------------------------

    protected virtual void Awake() {
        m_transform = transform;
    }

//----------------------------------------------------------------------------------------------------------------------
    
    private Transform m_transform = null;
}

} //end namespace