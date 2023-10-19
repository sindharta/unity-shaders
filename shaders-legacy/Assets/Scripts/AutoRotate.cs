using UnityEngine;

public class AutoRotate : MonoBehaviour {
    private void OnEnable() {
        m_transform = transform;
    }

    void Update() {

        float sinResult      = Mathf.Sin(Time.realtimeSinceStartup * m_rotateYSpeed);
        float yHalfAngleDiff = (m_maxYAngle - m_minYAngle) * 0.5f;
        
        float yAngle = m_minYAngle + (yHalfAngleDiff) + (yHalfAngleDiff * sinResult);        
        m_transform.localRotation = Quaternion.Euler(0, yAngle,0);        
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private float m_minYAngle = -90.0f;
    [SerializeField] private float m_maxYAngle = 90.0f;
    [SerializeField] private float m_rotateYSpeed = 1.0f;

    private Transform m_transform;

}
