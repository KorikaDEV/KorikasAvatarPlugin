using UnityEngine;
using UnityEditor;
using KAPStuff;
public class ExtrasUI : EditorWindow {
	[MenuItem("Korikas Avatar Plugin/Extras")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<ExtrasUI>("KAPExtras");
        window.minSize = new Vector2(265, 265);
    }
	
	bool addcolliders = false;
	bool resize = false;
	bool fixedjoints = false;
	bool addtoeverydynbone = false;
	float posx = 0f;
	float posy = 0f;
	float posz = 0f;
	float size = 0f;

	float avatarSize = 0f;
	
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
		GUILayout.Label("resize avatar", EditorStyles.boldLabel);
		GUILayout.Label("current size: " + AvatarResizer.getCurrentSize() + "m");
		GUILayout.Label("size in meters:");
		avatarSize = EditorGUILayout.Slider(avatarSize, 0, 5);
		resize = GUILayout.Button("resize");
		if(resize){
			AvatarResizer.resize(avatarSize);
		}

		GUILayout.Label("add fixedjoints", EditorStyles.boldLabel);
		GUILayout.Label("adds 2 fixedjoints for each hand!");
		fixedjoints = GUILayout.Button("do it!");
		if(fixedjoints){
			FixedJointAdder.add();
		}
	}
}