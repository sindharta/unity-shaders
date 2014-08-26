using UnityEngine;

public class DebugVertexVectors : MonoBehaviour {
    [SerializeField]
    bool m_showNormals;
    [SerializeField]
    bool m_showViewReflection;

    [SerializeField]
    float m_vectorSize = 1.0f;

//---------------------------------------------------------------------------------------------------------------------

    #region Cached variables
    Mesh m_mesh;
    Transform m_transform;
    Transform m_camTransform;
    #endregion Cached variables

//---------------------------------------------------------------------------------------------------------------------

    void OnDrawGizmos() {
        Gizmos.matrix = GetTransform().localToWorldMatrix;

        //normals
        Mesh cur_mesh = GetMesh();
        if (m_showNormals) {
            for (int i = 0; i < cur_mesh.normals.Length; i++) {
                Vector3 normal = (cur_mesh.normals[i]).normalized;
                Gizmos.color = new Color(normal.x, normal.y, normal.z, 1.0f);
                Gizmos.DrawRay(cur_mesh.vertices[i], normal * m_vectorSize);
            }
        }

        if (m_showViewReflection) {
            //reflection vectors
            Vector3 cam_pos = GetCamTransform().position;
            for (int i = 0; i < m_mesh.vertices.Length; i++) {
                Vector3 view = (cam_pos - m_mesh.vertices[i]).normalized;
                Vector3 refl = Reflect(view, m_mesh.normals[i]);

                Gizmos.color = new Color(refl.x, refl.y, refl.z, 1.0f);
                Gizmos.DrawRay(m_mesh.vertices[i], refl * m_vectorSize);
            }
        }
    }

//---------------------------------------------------------------------------------------------------------------------
    Vector3 Reflect(Vector3 input, Vector3 normal) {
        Vector3 reflection = 2.0f * normal * Vector3.Dot(normal, input) - input;
        return reflection.normalized;
    }

//---------------------------------------------------------------------------------------------------------------------
    #region functions to get cached variables
    Transform GetTransform() {
        if (null == m_transform)
            m_transform = GetComponent<Transform>();

        return m_transform;
    }
    Transform GetCamTransform() {
        if (null == m_camTransform)
            m_camTransform = Camera.main.transform;

        return m_camTransform;
    }

    Mesh GetMesh() {
        if (null == m_mesh)
            m_mesh = GetComponent<MeshFilter>().sharedMesh;

        return m_mesh;
    }
    #endregion functions to get cached variables


//---------------------------------------------------------------------------------------------------------------------


}