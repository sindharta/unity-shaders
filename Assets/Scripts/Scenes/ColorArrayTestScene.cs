using UnityEngine;

[ExecuteInEditMode]
public class ColorArrayTestScene : MonoBehaviour {
    Color[] m_colors = new Color[] {
        new Color(0,0,0,1),
        new Color(1,0,0,1),
        new Color(0,1,0,1),
        new Color(0,0,1,1),
    };

    [SerializeField]
    PlaneComponent m_plane = null;

    Material m_planeMaterial = null;

//---------------------------------------------------------------------------------------------------------------------

    void Awake() {
        MeshRenderer mesh_renderer = m_plane.GetComponent<MeshRenderer>();
        m_planeMaterial = mesh_renderer.sharedMaterial;
    }

//---------------------------------------------------------------------------------------------------------------------

    // Use this for initialization
    void Start() {
        m_plane.CreatePlaneCallback = OnCreatePlane;

        //set array in the shader
        int num_colors = m_colors.Length;
        for (int i=0; i < num_colors; ++i) {
            m_planeMaterial.SetColor("g_colors" + i.ToString(), m_colors[i]);
        }
        m_planeMaterial.SetInt("g_numColors", num_colors);
    }

//---------------------------------------------------------------------------------------------------------------------

    void OnCreatePlane(Mesh mesh) {
        //setup new colors for the mesh
        int num_vertices = mesh.colors.Length;
        Color[] new_colors     = new Color[num_vertices];

        for (int i=0; i < num_vertices; ++i) {
            //the red component encodes the index to m_colors
            new_colors[i] = new Color(Random.Range(0.0f, 1.0f), 0, 0, 1);
        }

        mesh.colors = new_colors;
    }
}