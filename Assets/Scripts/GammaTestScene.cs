/*
Requirements:
1. Unity rendering setting must be set to "Gamma rendering"
2. Unity ambient color should be set to 0
3. Light intensity should be set to 0.25 (which actually means 0.5 in Unity)

The checkerboard (left up) consists of 50% black and 50% white, which indicates what 50% grey should look like
when looked from afar. 

However, even though the color of the ConstantColorPlane (right up) is set to 0.5, the colors look different. 
This is because the monitor applies gamma or pow(color,gamma) when displaying colors. 
While the pow results of black and white remain the same, pow(0.5, gamma) will generate a different color, 
so that's why the colors look different.

The SimpleDiffusePlane (left bottom) takes this gamma correction into consideration by doing pow(color, inv_gamma) 
at the end of the shader, which will generate a more similar color to the one produced by the checkerboard, 
and thus more "realistic".

The result of UnityDiffusePlane (right bottom) is the same as ConstantColor under "Gamma Rendering" setting,
which confirms that UnityDiffuse performs the lighting in the gamma space,

*/

using UnityEngine;

public class GammaTestScene : MonoBehaviour {
    void Awake() {
        Debug.Log ("You may want to check the comments on GammaTest.cs too");
    }

}
