using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneComponent : MonoBehaviour {
    [SerializeField]
    float Length = 1;

    [SerializeField]
    float Width = 1;

    [SerializeField]
    int NumSubdivision = 8;

    MeshFilter m_meshFilter = null;
    float m_actualWidth  = 0;
    float m_actualLength = 0;
    int m_actualNumSubDivision = 0;

    public Action<Mesh> CreatePlaneCallback = null;

//---------------------------------------------------------------------------------------------------------------------

    public void SetSize(float length, float width) {
        Length = length;
        Width = width;

        CreatePlane();
    }

//---------------------------------------------------------------------------------------------------------------------

    public MeshFilter GetMeshFilter() {
        return m_meshFilter;
    }

//---------------------------------------------------------------------------------------------------------------------

    #region MonoBehaviour functions.
    void Awake() {
        m_meshFilter = GetComponent<MeshFilter>();
    }

    void Start() {
        CreatePlane();
    }

#if UNITY_EDITOR
    //Run time changing only works in the editor.
    void Update() {
        CreatePlane();
    }
#endif
    #endregion MonoBehaviour functions.

//---------------------------------------------------------------------------------------------------------------------

    void CreatePlane() {
        if (NumSubdivision <= 0)
            return;

        //Check if we should recreate the plane mesh.
        if (!Mathf.Approximately(m_actualWidth, Width) || !Mathf.Approximately(m_actualLength, Length)
            || m_actualNumSubDivision != NumSubdivision
            ) {
            CreatePlaneImmediate();
        }
    }

//---------------------------------------------------------------------------------------------------------------------

    //Assuming that all the internal variables are set up correctly
    void CreatePlaneImmediate() {
        int num_quads = (NumSubdivision * NumSubdivision);

        //[TODO-Sin:2014-09-22] It is possible to reduce the number of vertices here since they are redundant
        int num_vertices = num_quads * 4;

        Vector3[] vertices = new Vector3[num_vertices];
        Vector3[] normals  = new Vector3[num_vertices];
        Color[] colors     = new Color[num_vertices];
        int[] indices      = new int[num_quads * 6];
        float half_length   = Length * 0.5f;
        float half_width    = Width * 0.5f;

        float length_counter = Length / NumSubdivision;
        float width_counter  = Width / NumSubdivision;
        int start_vertex_index = 0;
        int start_triangle_index = 0;

        for (int i=0; i < NumSubdivision; ++i) {
            float x = -half_length + i * length_counter;
            for (int j=0; j < NumSubdivision; ++j) {
                float z = -half_width + j * width_counter;
                CreateXZQuadVertices(x, z, x + length_counter, z + width_counter, 0, start_vertex_index, ref vertices);

                for (int k = 0; k < 4; ++k) {
                    int cur_index = start_vertex_index + k;
                    normals[cur_index] = new Vector3(0, 1, 0);
                    colors[cur_index] = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }

                CreateQuadIndices(start_triangle_index, start_vertex_index, ref indices);

                start_vertex_index += 4;
                start_triangle_index += 6;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.triangles = indices;
        m_meshFilter.mesh = mesh;

        m_actualWidth = Width;
        m_actualLength = Length;
        m_actualNumSubDivision = NumSubdivision;

        if (null != CreatePlaneCallback)
            CreatePlaneCallback(mesh);
    }

//---------------------------------------------------------------------------------------------------------------------

    static void CreateXZQuadVertices(float left, float bottom, float right, float top, float y,
                                            int startIndex, ref Vector3[] vertices) {
        vertices[startIndex++] = new Vector3(left, y, bottom);
        vertices[startIndex++] = new Vector3(left, y, top);
        vertices[startIndex++] = new Vector3(right, y, bottom);
        vertices[startIndex++] = new Vector3(right, y, top);
    }

//---------------------------------------------------------------------------------------------------------------------

    static void CreateQuadIndices(int startTriangleIndex, int startVertexIndex, ref int[] triangles) {
        triangles[startTriangleIndex++] = startVertexIndex;
        triangles[startTriangleIndex++] = startVertexIndex + 1;
        triangles[startTriangleIndex++] = startVertexIndex + 2;

        triangles[startTriangleIndex++] = startVertexIndex + 2;
        triangles[startTriangleIndex++] = startVertexIndex + 1;
        triangles[startTriangleIndex++] = startVertexIndex + 3;
    }
}