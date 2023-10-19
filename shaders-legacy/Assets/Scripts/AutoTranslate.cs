using UnityEngine;
using UnityEngine.Serialization;


public class AutoTranslate : MonoBehaviour {
    private void OnEnable() {
        m_transform = transform;
        m_startPos  = m_transform.position;
    }

    void Update() {

        float   sinResult = Mathf.Sin(Time.realtimeSinceStartup * m_translateSpeed);
        Vector3 halfOffset = m_endOffset * 0.5f;
        
        m_transform.localPosition = m_startPos + (halfOffset) + (halfOffset * sinResult);
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    [FormerlySerializedAs("m_endPos")] [SerializeField] private Vector3 m_endOffset = new Vector3(0,0,10);
    [SerializeField] private float   m_translateSpeed = 1.0f;


    private Vector3   m_startPos = Vector3.zero;
    private Transform m_transform;

}
