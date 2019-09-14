using UnityEngine;
using UnityEditor;
using KATStuff;
public class BonesWindow : EditorWindow {
	[MenuItem("Korikas Avatar Tools/Bones")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<BonesWindow>("KATBones");
        window.minSize = new Vector2(265, 265);
    }
	
	bool addcolliders = false;
	float position = 1f;
	float size = 0f;
	
	void OnGUI(){
		GUILayout.Label("add/change handcolliders", EditorStyles.boldLabel);
		addcolliders = EditorGUILayout.Toggle("enabled", addcolliders);
		if(addcolliders){
			GUILayout.Label("position:");
			position = EditorGUILayout.Slider(position, 0, 5);
			GUILayout.Label("size:");
			size = EditorGUILayout.Slider(size, 0, 5);

			HandColliderAdder.addColliderIfDontExistsAndUpdate(GestureDisplay.getVRCSceneAvatar(), position, size);
			SceneView.RepaintAll();
		}
	}
}