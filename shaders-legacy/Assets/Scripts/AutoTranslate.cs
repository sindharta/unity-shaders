using UnityEngine;


public class AutoTranslate : MonoBehaviour {
    private void OnEnable() {
        m_transform = transform;
        m_startPos  = m_transform.position;
    }

    void Update() {

        float   sinResult = Mathf.Sin(Time.realtimeSinceStartup * m_translateSpeed);
        Vector3 halfPos   = (m_endPos - m_startPos) * 0.5f;
        
        m_transform.position= m_startPos + (halfPos) + (halfPos * sinResult);
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Vector3 m_endPos         = new Vector3(0,0,-10);
    [SerializeField] private float   m_translateSpeed = 1.0f;


    private Vector3 m_startPos = Vector3.zero;
    private Transform m_transform;

}
