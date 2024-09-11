using UnityEngine;
using System.Collections;

namespace Shin.Core {

[ExecuteAlways]
public class FollowSceneCamera : MonoBehaviour {

#if UNITY_EDITOR

	void LateUpdate() {
		if (Application.isPlaying)
			Follow();
	}

	public void OnDrawGizmos() {
		if (!Application.isPlaying)
			Follow();
	}

//----------------------------------------------------------------------------------------------------------------------	

	//Make the specified camera (or Camera.main) follow the first SceneView camera
	void Follow() {
		ArrayList sceneViews = UnityEditor.SceneView.sceneViews;
		if (sceneViews.Count == 0)
			return;


		Camera gameCamera = (null != m_gameViewCamera) ? m_gameViewCamera : Camera.main;
		if (null == gameCamera)
			return;

		Transform gameCameraT = gameCamera.transform;
		if (sceneViews.Count <= 0)
			return;

		UnityEditor.SceneView sceneView = (UnityEditor.SceneView) sceneViews[0];
		if (null == sceneView)
			return;

		Camera curSceneCam = sceneView.camera;
		if (null == curSceneCam)
			return;

		Transform curSceneCamT = curSceneCam.transform;
		gameCamera.orthographic = sceneView.orthographic;
		gameCameraT.position    = curSceneCamT.position;
		gameCameraT.rotation    = curSceneCamT.rotation;
	}


#endif

//----------------------------------------------------------------------------------------------------------------------

	[SerializeField] private Camera m_gameViewCamera;

}

} //end namespace


//References: http://wiki.unity3d.com/index.php/SceneViewCameraFollower