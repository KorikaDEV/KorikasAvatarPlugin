using UnityEngine;
using UnityEditor;
using KAPStuff;
public class AvatarsUI : EditorWindow
{
    static Vector2 scrollPosition = Vector2.zero;
    static GameObject model;

    [MenuItem("Korikas Avatar Plugin/Avatars")]
    public static void ShowWindow()
    {
        AvatarStructureBuilder.createKAPRootFolder();
        EditorWindow window = EditorWindow.GetWindow<AvatarsUI>("KAPAvatars");
        window.minSize = new Vector2(265, 265);
    }
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,  GUILayout.Width(position.width),  GUILayout.Height(position.height - 74));
        GuiLine(2);
        KAPProfile[] kps = KAPProfile.getAllInProject();
        foreach (KAPProfile item in kps)
        {
            AvatarsContainer.initKPValue(item.name);
            addButton("Avatar Icon", 3, 0, 20, true);
			GUILayout.Label("    " + item.name, EditorStyles.boldLabel);
			GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.performance());
			Rect r = EditorGUILayout.BeginVertical();
			EditorGUI.ProgressBar(r, ((4 - item.perfP.performance()) / 4), PerformanceProfile.doubleToPerfName(item.perfP.performance()) + " (" + (4 - item.perfP.performance()) + ")");
			GUILayout.Space(18);
			EditorGUILayout.EndVertical();
			GUI.color = Color.white;
			AvatarsContainer.kpsfoldout[item.name] = EditorGUILayout.Foldout(AvatarsContainer.kpsfoldout[item.name], "details"); 
			if(AvatarsContainer.kpsfoldout[item.name]){
                GUILayout.BeginHorizontal();
                addPerfSection(item.polys, item.perfP.polysperf, "PolygonCollider2D Icon");
                DrawBreak();
                addPerfSection(item.boneamount, item.perfP.boneamountperf, "Transform Icon");
                GUILayout.EndHorizontal();
                GuiLine();
                GUILayout.BeginHorizontal();
                addPerfSection(item.meshrenderers, item.perfP.meshrenderersperf, "MeshRenderer Icon");
                DrawBreak();
                addPerfSection(item.dynboneamount, item.perfP.dynboneamountperf, "dll Script Icon");
                GUILayout.EndHorizontal();
                GuiLine();
                GUILayout.BeginHorizontal();
                addPerfSection(item.dynbonecolliders, item.perfP.dynbonecollidersperf, "sv_icon_dot4_pix16_gizmo");
                DrawBreak();
                addPerfSection(item.particle_systems, item.perfP.particle_systemsperf, "d_ParticleSystem Icon");
                GUILayout.EndHorizontal();
                GuiLine();
                GUILayout.BeginHorizontal();
                addPerfSection(item.audio_sources, item.perfP.audio_sourcesperf, "AudioSource Icon");
                DrawBreak();
                addPerfSection(item.lights, item.perfP.lightsperf, "Light Icon");
                GUILayout.EndHorizontal();
                GuiLine();
                GUILayout.BeginHorizontal();
                addPerfSection(item.animators, item.perfP.animatorsperf, "Animator Icon");
                DrawBreak();
                addPerfSection(item.cloth, item.perfP.clothperf, "Cloth Icon");
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                AvatarsContainer.kpsscene[item.name] = GUILayout.Button("scene");
                AvatarsContainer.kpsfolder[item.name] = GUILayout.Button("folder");
                GUI.backgroundColor = MainContainer.fromRGB(255f, 119f, 110f);
                AvatarsContainer.kpsdelete[item.name] = GUILayout.Button("delete");
                GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();
			}
            GuiLine(2);

            if(AvatarsContainer.kpsscene[item.name]){
                item.openScene();
            }
            if(AvatarsContainer.kpsfolder[item.name]){
                item.openFolder();
                CleanFolder.cleanFolder(item.name);
            }
            if(AvatarsContainer.kpsdelete[item.name]){
                item.delete();
            }
        }
        GUILayout.EndScrollView();
        addButton("Avatar Icon", 3, 0, 20, true);
        GUILayout.Label("    add avatar", EditorStyles.boldLabel);

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
    }

    void addButton(string name, float down, float right, float wh, bool underlast){
        Rect lastrec = GUILayoutUtility.GetLastRect();
        if(underlast == true){
            GUI.DrawTexture(new Rect(0, lastrec.position.y + lastrec.height + down, wh, wh), EditorGUIUtility.FindTexture(name));
        }else{
            GUI.DrawTexture(new Rect(lastrec.position.x + lastrec.width + right,  lastrec.position.y + down, wh, wh), EditorGUIUtility.FindTexture(name));
        }
    }

    void addPerfSection(float amount, double perf, string iconname){
        float width = Screen.width * 0.9f;
        GUI.color = PerformanceProfile.doubleToPerfColor(perf);
        GUILayout.Label(amount + "", GUILayout.Width(width / 2));
        GUI.color = Color.white;
        addButton(iconname, 0, -20, 15, false);
    }

    void GuiLine( int i_height = 1 )
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height );
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
    }

    void DrawBreak(){
        Rect lastrec = GUILayoutUtility.GetLastRect();
        Rect newrec = new Rect(lastrec.position.x + lastrec.width, lastrec.position.y, 1, lastrec.height);
        EditorGUI.DrawRect(newrec, new Color ( 0.5f,0.5f,0.5f, 1 ) );
    }
}