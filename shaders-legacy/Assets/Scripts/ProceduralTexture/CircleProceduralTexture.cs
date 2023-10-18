using UnityEngine;

public class CircleProceduralTexture : MonoBehaviour {

    int m_size = 512;
    Texture2D m_generatedTexture = null;
   
    Material m_currentMaterial = null;
    Vector2 m_centerPos = new Vector2(0.0f,0.0f);
    
    [SerializeField]
    Vector2 m_reqCenterPos = new Vector2(0.5f,0.5f);
            
//--------------------------------------------------------------------------------------------------

    void Awake() {
        if (null== m_currentMaterial) {
            m_currentMaterial = transform.GetComponent<Renderer>().sharedMaterial;        
        }
        
        if (null== m_currentMaterial) {
            Debug.LogError(string.Format("Can not find the material of {0}",transform.name));
            return;
        }
                
    }
                
//--------------------------------------------------------------------------------------------------

	Texture2D GenerateCircle (Vector3 centerPos) {
        Texture2D tex = new Texture2D(m_size,m_size);
        
        Vector2 center_pixel_pos = centerPos * m_size;
        float inv_half_size = 1.0f / (m_size * 0.5f);
        
        for (uint i=0;i<m_size;++i) {
            for (uint j=0;j<m_size;++j) {
                float dist = Vector2.Distance(new Vector2(i,j),center_pixel_pos);
                float normalized_dist = dist * inv_half_size;
                
                //invert
                float c = Mathf.Clamp01(1.0f - normalized_dist);
                
                Color color = new Color(c,c,c,1.0f);
                tex.SetPixel( (int)i,(int)j,color);                
            }
        }
        
        tex.Apply();
        return tex;        
	}
    
//--------------------------------------------------------------------------------------------------


    void Update() {
        if (null==m_currentMaterial)
            return;
           
        //decide if we need to  update the texture
        if ((m_centerPos - m_reqCenterPos).sqrMagnitude > float.Epsilon) {
            m_centerPos = m_reqCenterPos;
            m_generatedTexture = GenerateCircle(m_centerPos);
            m_currentMaterial.SetTexture("_MainTex",m_generatedTexture);        
        }
    
    }
//--------------------------------------------------------------------------------------------------

}
