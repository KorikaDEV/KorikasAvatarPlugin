using UnityEngine;
using UnityEditor;

public class MainWindow : EditorWindow {

	static GameObject source;
	static GameObject destination;
	static GameObject model;

    public bool fingerpoint = false;
    public bool fist = false;
    public bool victory = false;
    public bool handgun = false;
    public bool thumbsup = false;
    public bool rocknroll = false;
    public bool handopen = false;

	public bool gameobjects = true;
	public bool particle_system = true;
	public bool dynamic_bone = true;

	public static Color background;
	public static Color buttons;

    int toolbarInt = 0;

	private static Texture2D tex;

	[MenuItem("Korikas Avatar Tools/Main")]
	public static void ShowWindow(){

		EditorWindow window = EditorWindow.GetWindow<MainWindow>("KAT v1.0.0");
        window.maxSize = new Vector2(265,265);
        window.minSize = window.maxSize;
    }
	void OnGUI(){
		initColor ();
		tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel(0, 0, background);
		tex.Apply();
		GUI.DrawTexture(new Rect(0, 0, position.width,position.height), tex, ScaleMode.StretchToFill);
		GUI.Label(new Rect(200, 200, 100, 100), "");
		GUI.TextField(new Rect(20, 20, 70, 30), "");
		GUI.backgroundColor = buttons;
        
		Texture logo = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Korikas Avatar Tools/logo.png", typeof(Texture));
		GUI.DrawTexture(new Rect(0, 0, position.width,70), logo, ScaleMode.ScaleAndCrop);

        string[] toolbarString = {"gestures", "generate", "copy", "credits"};
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarString);

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		switch (toolbarInt) {
		case 0:
			toolbarInt = 0;
            RenderGestureTab();
            break;
		case 1:
			toolbarInt = 1;
			renderAnimationsTab ();
			break;
		case 2:
			toolbarInt = 2;
			RenderCopyContentsTab ();
			break;
        case 3:
            RenderCreditsTab();
            break;
        }

    }

	public void renderAnimationsTab(){
		GUILayout.Label ("generate structure", EditorStyles.boldLabel);

		model = (GameObject)EditorGUILayout.ObjectField ("select your avatar:", model, typeof(GameObject), true);
		if (model != null) {
			bool btn = GUILayout.Button ("do it!");
			if (btn == true) {
				OverrideBuilder.BuildOverride (model);
			}
		} else {
			GUI.enabled = false;
			GUILayout.Button ("please select a GameObject");
			GUI.enabled = true;
		}
	}

	public void RenderCopyContentsTab(){
		string label = "select your avatar:";
		GUILayout.Label ("source", EditorStyles.boldLabel);
		source = (GameObject)EditorGUILayout.ObjectField(label, source, typeof(GameObject), true);
		GUILayout.Label ("destination", EditorStyles.boldLabel);
		destination = (GameObject)EditorGUILayout.ObjectField(label, destination, typeof(GameObject), true);
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		gameobjects = EditorGUILayout.Toggle("GameObjects", gameobjects);
		if (gameobjects) {
			GUI.enabled = false;
			particle_system = EditorGUILayout.Toggle("Particle Systems", true);
			GUI.enabled = true;
		} else {
			particle_system = EditorGUILayout.Toggle("Particle Systems", particle_system);
		}
		dynamic_bone = EditorGUILayout.Toggle("Dynamic Bones", dynamic_bone);
		if (source != null && destination != null) {
			bool copy = GUILayout.Button ("copy contents!");
            if (copy)
            {
                bool[] options = { gameobjects, particle_system, dynamic_bone };
                AvatarContentsCopy.copy(source, destination, options);
            }
		} else {
			GUI.enabled = false;
			GUILayout.Button ("please select 2 GameObject");
			GUI.enabled = true;
		}
		GUI.enabled = false;
		GUILayout.Label ("It only copies mapped bones childs");
		GUILayout.Label ("It wont copy contents inside the fingers");
		GUI.enabled = true;
	}

    public void RenderCreditsTab()
    {
        GUILayout.Label("These tools been coded by Korika!", EditorStyles.boldLabel);
        bool discord = GUILayout.Button("Discord");
        if (discord)
        {
            Application.OpenURL("https://discord.gg/z2SmFqg");
        }
        bool vrcmods = GUILayout.Button("VRCMods");
        if (vrcmods)
        {
            Application.OpenURL("https://vrcmods.com/user/Korika");
        }
        bool youtube = GUILayout.Button("YouTube");
        if (youtube)
        {
            Application.OpenURL("https://www.youtube.com/channel/UCRyYgGgXM6ugIjfmgQ6Go3A?view_as=subscriber");
        }
		bool github = GUILayout.Button("GitHub");
		if (github)
		{
			Application.OpenURL("https://github.com/KorikaDEV");
		}
    }

    public void RenderGestureTab()
    {
        if (EditorApplication.isPlaying)
        {
            fingerpoint = EditorGUILayout.Toggle("fingerpoint", fingerpoint);
            if (fingerpoint)
            {
                fingerpoint = true;
                fist = false;
                victory = false;
                handgun = false;
                thumbsup = false;
                rocknroll = false;
                handopen = false;
				string s = "FINGERPOINT";
				GestureDisplay.show (s);
}
            fist = EditorGUILayout.Toggle("fist", fist);
            if (fist)
            {
                fingerpoint = false;
                fist = true;
                victory = false;
                handgun = false;
                thumbsup = false;
                rocknroll = false;
                handopen = false;
				string s = "FIST";
				GestureDisplay.show (s);            }
            victory = EditorGUILayout.Toggle("victory", victory);
            if (victory)
            {
                fingerpoint = false;
                fist = false;
                victory = true;
                handgun = false;
                thumbsup = false;
                rocknroll = false;
                handopen = false;
				string s = "VICTORY";
				GestureDisplay.show (s);            }
            handgun = EditorGUILayout.Toggle("handgun", handgun);
            if (handgun)
            {
                fingerpoint = false;
                fist = false;
                victory = false;
                handgun = true;
                thumbsup = false;
                rocknroll = false;
                handopen = false;
				string s = "HANDGUN";
				GestureDisplay.show (s);            }
            thumbsup = EditorGUILayout.Toggle("thumbsup", thumbsup);
            if (thumbsup)
            {
                fingerpoint = false;
                fist = false;
                victory = false;
                handgun = false;
                thumbsup = true;
                rocknroll = false;
                handopen = false;
				string s = "THUMBSUP";
				GestureDisplay.show (s);            }
            rocknroll = EditorGUILayout.Toggle("rocknroll", rocknroll);
            if (rocknroll)
            {
                fingerpoint = false;
                fist = false;
                victory = false;
                handgun = false;
                thumbsup = false;
                rocknroll = true;
                handopen = false;
				string s = "ROCKNROLL";
				GestureDisplay.show (s);            }
            handopen = EditorGUILayout.Toggle("handopen", handopen);
            if (handopen)
            {
                fingerpoint = false;
                fist = false;
                victory = false;
                handgun = false;
                thumbsup = false;
                rocknroll = false;
                handopen = true;
				string s = "HANDOPEN";
				GestureDisplay.show (s);
            }
			GUILayout.Label ("check to play the animation", EditorStyles.boldLabel);
        }
        else
        {
			fingerpoint = false;
			fist = false;
			victory = false;
			handgun = false;
			thumbsup = false;
			rocknroll = false;
			handopen = false;
			GUI.enabled = false;
			fingerpoint = EditorGUILayout.Toggle("fingerpoint", fingerpoint);
			fist = EditorGUILayout.Toggle("fist", fist);
			victory = EditorGUILayout.Toggle("victory", victory);
			handgun = EditorGUILayout.Toggle("handgun", handgun);
			thumbsup = EditorGUILayout.Toggle("thumbsup", thumbsup);
			rocknroll = EditorGUILayout.Toggle("rocknroll", rocknroll);
			handopen = EditorGUILayout.Toggle("handopen", handopen);
			GUILayout.Label ("play animations in playmode!", EditorStyles.boldLabel);
			GUI.enabled = true;
        }
    }
	public static Color fromRGB(float r, float g, float b){
		return new Color (r / 255f, g / 255f, b / 255);
	}
	public static void initColor(){
		if (EditorGUIUtility.isProSkin) {
			background = fromRGB (124f, 96f, 77f);
			buttons = fromRGB (193f, 144f, 92f);
		}else{
			background = fromRGB (209f, 189f, 169f);
			buttons = fromRGB (203f, 164f, 112f);
		}
	}
}
