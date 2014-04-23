//Draw reference from http://wiki.unity3d.com/index.php/SceneViewCameraFollower
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FollowSceneCamera : MonoBehaviour {

#if UNITY_EDITOR
 
	void LateUpdate()
	{
		if(Application.isPlaying)
			Follow();
	}
 
	public void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			Follow();
	}
 
	//Make Camera.main follor the first sceneview camera
	void Follow()
	{
		ArrayList sceneViews = UnityEditor.SceneView.sceneViews;
		if(sceneViews.Count == 0) 
			return;

		Camera cur_game_cam = Camera.main;
		for(int i=0;i<sceneViews.Count;++i)
		{	
			UnityEditor.SceneView sceneView = (UnityEditor.SceneView) sceneViews[i];
			if(null==sceneView)
				continue;
			
			Camera cur_scene_cam = sceneView.camera;
			if (null==cur_scene_cam)
				continue;
			
			cur_game_cam.orthographic = sceneView.orthographic;
			cur_game_cam.transform.position = cur_scene_cam.transform.position;
			cur_game_cam.transform.rotation = cur_scene_cam.transform.rotation;
		}	
	}
 
 
#endif
}
