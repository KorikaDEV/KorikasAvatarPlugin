using UnityEngine;
using UnityEditor;

public class MainWindow : EditorWindow
{

    static GameObject source;
    static GameObject destination;
    static GameObject model;
    static GameObject beatfindersource;

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

    static TextAsset beatfinder;

    int toolbarInt = 0;

    private static Texture2D tex;
    static Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Korikas Avatar Tools/Main")]
    public static void ShowWindow()
    {

        EditorWindow window = EditorWindow.GetWindow<MainWindow>("KAT v1.0.0");
        window.minSize = new Vector2(265, 265);
    }
    void OnGUI()
    {
        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        initColor();
        tex.SetPixel(0, 0, background);
        tex.Apply();
        GUI.DrawTexture(new Rect(0, 0, position.width, position.height), tex, ScaleMode.StretchToFill);
        GUI.Label(new Rect(200, 200, 100, 100), "");
        GUI.TextField(new Rect(20, 20, 70, 30), "");
        GUI.backgroundColor = buttons;
        Texture logo = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Korikas-Avatar-Tool/Korikas Avatar Tools/logo.png", typeof(Texture));
        GUI.DrawTexture(new Rect(0, 0, position.width, 70), logo, ScaleMode.ScaleAndCrop);

        string[] toolbarString = { "gestures", "generate", "copy", "credits" };
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,  GUILayout.Width(position.width),  GUILayout.Height(position.height - 74));
        switch (toolbarInt)
        {
            case 0:
                toolbarInt = 0;
                RenderGestureTab();
                break;
            case 1:
                toolbarInt = 1;
                renderGenerationTab();
                break;
            case 2:
                toolbarInt = 2;
                RenderCopyContentsTab();
                break;
            case 3:
                RenderCreditsTab();
                break;
        }
        EditorGUILayout.EndScrollView();

    }

    public void renderGenerationTab()
    {
        GUILayout.Label("generate structure", EditorStyles.boldLabel);

        model = (GameObject)EditorGUILayout.ObjectField("select your avatar:", model, typeof(GameObject), true);
        if (model != null)
        {
            bool btn = GUILayout.Button("do it!");
            if (btn == true)
            {
                AvatarStructureBuilder.BuildOverride(model);
            }
        }
        else
        {
            GUI.enabled = false;
            GUILayout.Button("please select a .fbx");
            GUI.enabled = true;
        }
        GUILayout.Label("generate beatfinder animation", EditorStyles.boldLabel);
        beatfindersource = (GameObject)EditorGUILayout.ObjectField("your gameobject:", beatfindersource, typeof(GameObject), true);
        beatfinder = (TextAsset)EditorGUILayout.ObjectField("your beatfinder file:", beatfinder, typeof(TextAsset), true);
        bool beatbutton = false;
        if(beatfindersource != null){
            if(!BeatFinder.ifHasStructure(beatfindersource)){
                GUI.enabled = false;
                beatbutton = GUILayout.Button("gameobject not ready");
                GUI.enabled = true;
            }else{
                beatbutton = GUILayout.Button("generate beatfinder animation");
            }
        }else{
            beatbutton = GUILayout.Button("generate beatfinder animation");
        }
        if(beatbutton){
            BeatFinder.generateBeatAnimation(beatfinder, beatfindersource);
        }
    }

    public void RenderCopyContentsTab()
    {
        string label = "select your avatar:";
        GUILayout.Label("source", EditorStyles.boldLabel);
        source = (GameObject)EditorGUILayout.ObjectField(label, source, typeof(GameObject), true);
        GUILayout.Label("destination", EditorStyles.boldLabel);
        destination = (GameObject)EditorGUILayout.ObjectField(label, destination, typeof(GameObject), true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        gameobjects = EditorGUILayout.Toggle("GameObjects", gameobjects);
        if (gameobjects)
        {
            GUI.enabled = false;
            particle_system = EditorGUILayout.Toggle("Particle Systems", true);
            GUI.enabled = true;
        }
        else
        {
            particle_system = EditorGUILayout.Toggle("Particle Systems", particle_system);
        }
        dynamic_bone = EditorGUILayout.Toggle("Dynamic Bones", dynamic_bone);
        if (source != null && destination != null)
        {
            bool copy = GUILayout.Button("copy contents!");
            if (copy)
            {
                bool[] options = { gameobjects, particle_system, dynamic_bone };
                AvatarContentsCopy.copy(source, destination, options);
            }
        }
        else
        {
            GUI.enabled = false;
            GUILayout.Button("please select 2 GameObjects");
            GUI.enabled = true;
        }
        GUI.enabled = false;
        GUILayout.Label("It only copies mapped bones childs");
        GUILayout.Label("It wont copy contents inside the fingers");
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
    public void disableEveryCheck()
    {
        fingerpoint = false;
        fist = false;
        victory = false;
        handgun = false;
        thumbsup = false;
        rocknroll = false;
        handopen = false;
    }
    public void RenderGestureTab()
    {
        if (EditorApplication.isPlaying)
        {
            fingerpoint = EditorGUILayout.Toggle("fingerpoint", fingerpoint);
            if (fingerpoint)
            {
                disableEveryCheck();
                fingerpoint = true;
                string s = "FINGERPOINT";
                GestureDisplay.show(s);
            }
            fist = EditorGUILayout.Toggle("fist", fist);
            if (fist)
            {
                disableEveryCheck();
                fist = true;
                string s = "FIST";
                GestureDisplay.show(s);
            }
            victory = EditorGUILayout.Toggle("victory", victory);
            if (victory)
            {
                disableEveryCheck();
                victory = true;
                string s = "VICTORY";
                GestureDisplay.show(s);
            }
            handgun = EditorGUILayout.Toggle("handgun", handgun);
            if (handgun)
            {
                disableEveryCheck();
                handgun = true;
                string s = "HANDGUN";
                GestureDisplay.show(s);
            }
            thumbsup = EditorGUILayout.Toggle("thumbsup", thumbsup);
            if (thumbsup)
            {
                disableEveryCheck();
                thumbsup = true;
                string s = "THUMBSUP";
                GestureDisplay.show(s);
            }
            rocknroll = EditorGUILayout.Toggle("rocknroll", rocknroll);
            if (rocknroll)
            {
                disableEveryCheck();
                rocknroll = true;
                string s = "ROCKNROLL";
                GestureDisplay.show(s);
            }
            handopen = EditorGUILayout.Toggle("handopen", handopen);
            if (handopen)
            {
                disableEveryCheck();
                handopen = true;
                string s = "HANDOPEN";
                GestureDisplay.show(s);
            }
            GUILayout.Label("check to play the animation", EditorStyles.boldLabel);
        }
        else
        {
            disableEveryCheck();
            GUI.enabled = false;
            fingerpoint = EditorGUILayout.Toggle("fingerpoint", fingerpoint);
            fist = EditorGUILayout.Toggle("fist", fist);
            victory = EditorGUILayout.Toggle("victory", victory);
            handgun = EditorGUILayout.Toggle("handgun", handgun);
            thumbsup = EditorGUILayout.Toggle("thumbsup", thumbsup);
            rocknroll = EditorGUILayout.Toggle("rocknroll", rocknroll);
            handopen = EditorGUILayout.Toggle("handopen", handopen);
            GUILayout.Label("play animations in playmode!", EditorStyles.boldLabel);
            GUI.enabled = true;
        }
    }
    public static Color fromRGB(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255);
    }
    public static void initColor()
    {
        if (EditorGUIUtility.isProSkin)
        {
            background = fromRGB(124f, 96f, 77f);
            buttons = fromRGB(193f, 144f, 92f);
        }
        else
        {
            background = fromRGB(209f, 189f, 169f);
            buttons = fromRGB(203f, 164f, 112f);
        }
    }
}
