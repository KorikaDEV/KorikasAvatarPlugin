using UnityEngine;
using UnityEditor;
using KATStuff;
public class BonesUI : EditorWindow {
	[MenuItem("Korikas Avatar Tools/Bones")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<BonesUI>("KATBones");
        window.minSize = new Vector2(265, 265);
    }
	
	bool addcolliders = false;
	bool addtoeverydynbone = false;
	float posx = 0f;
	float posy = 0f;
	float posz = 0f;
	float size = 0f;
	
	void OnGUI(){
		GUILayout.Label("override handcolliders", EditorStyles.boldLabel);
		addcolliders = EditorGUILayout.Toggle("enabled", addcolliders);
		if(addcolliders){
			GUILayout.Label("position:", EditorStyles.boldLabel);
			GUILayout.Label("x:");
			posx = EditorGUILayout.Slider(posx, -5, 5);
			GUILayout.Label("y:");
			posy = EditorGUILayout.Slider(posy, -5, 5);
			GUILayout.Label("z:");
			posz = EditorGUILayout.Slider(posz, -5, 5);
			GUILayout.Label("size:", EditorStyles.boldLabel);
			size = EditorGUILayout.Slider(size, 0, 5);
			addtoeverydynbone = GUILayout.Button("add to every dynamicbone");

			HandColliderAdder.addColliderIfDontExistsAndUpdate(GestureDisplay.getVRCSceneAvatar(), new Vector3(posx,posy,posz), size, addtoeverydynbone);
			SceneView.RepaintAll();
		}
	}
}