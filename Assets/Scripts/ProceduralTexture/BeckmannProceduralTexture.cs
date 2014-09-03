using UnityEngine;
using System.IO;

public class BeckmannProceduralTexture : MonoBehaviour {

    [SerializeField]
    int m_texSize = 512;
    
    [SerializeField]
    bool m_saveTex = false; 
       
    Texture2D m_generatedTex = null;   
    Material m_currentMaterial = null;
    
    void Awake() {
        m_currentMaterial = transform.renderer.sharedMaterial;        
        m_generatedTex = GenerateBeckmannTexture();
        m_currentMaterial.SetTexture("_MainTex",m_generatedTex);        
        
        if (m_saveTex) {
            string path = Application.dataPath + "/Textures/Procedural/Beckmann.png";
            var bytes = m_generatedTex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);            
            Debug.Log ("Beckmann Texture saved to " + path);
        }
    }
    
   
    float Beckmann(float nDotH, float roughness) {
        float alpha =  Mathf.Acos( nDotH );  
        float ta = Mathf.Tan( alpha );          
        float m2 = roughness * roughness;
        
        float val = Mathf.Exp( -(ta*ta)/ m2 ) / ( Mathf.PI * m2 * Mathf.Pow(alpha,4.0f));  
        return val;  
        
    }
    
    //http://http.developer.nvidia.com/GPUGems3/gpugems3_ch14.html    
    Texture2D GenerateBeckmannTexture () {
        Texture2D tex = new Texture2D(m_texSize,m_texSize);
        float inv_size = 1.0f / (float) m_texSize;
                       
        for (uint i=0;i<m_texSize;++i) {
            float n_dot_h = (float)(i) * inv_size;
            for (uint j=0;j<m_texSize;++j) {
                float roughness = (float) (j) * inv_size;
                
                float c = Beckmann(n_dot_h, roughness);
                
                // Scale the value to fit within [0,1] – invert upon lookup.  
                c = 0.5f * Mathf.Pow( c, 0.1f );  
                                
                Color color = new Color(c,c,c,1.0f);
                tex.SetPixel( (int)i,(int)j,color);                
            }
        }
        
        tex.Apply();
        return tex;        
    }
    
}
