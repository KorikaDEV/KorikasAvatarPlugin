using UnityEngine;
using UnityEditor;
using KatStuff;
public class AvatarsWindow : EditorWindow
{
    static Vector2 scrollPosition = Vector2.zero;
    static GameObject model;

    [MenuItem("Korikas Avatar Tools/Avatars")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<AvatarsWindow>("KATAvatars");
        window.minSize = new Vector2(265, 265);
    }
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,  GUILayout.Width(position.width),  GUILayout.Height(position.height - 74));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        KatProfile[] kps = KatProfile.getAllInProject();
        foreach (KatProfile item in kps)
        {
            AvatarsContainer.initKPValue(item.name);
			GUILayout.Label(item.name, EditorStyles.boldLabel);
			GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.performance());
			Rect r = EditorGUILayout.BeginVertical();
			EditorGUI.ProgressBar(r, ((4 - item.perfP.performance()) / 4), PerformanceProfile.doubleToPerfName(item.perfP.performance()) + " (" + (4 - item.perfP.performance()) + ")");
			GUILayout.Space(18);
			EditorGUILayout.EndVertical();
			GUI.color = Color.white;
			AvatarsContainer.kpsfoldout[item.name] = EditorGUILayout.Foldout(AvatarsContainer.kpsfoldout[item.name], "perfomance details"); 
			if(AvatarsContainer.kpsfoldout[item.name]){
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.polysperf);
				GUILayout.Label(item.polys + " polygons");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.boneamountperf);
				GUILayout.Label(item.boneamount + " bones");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.meshrenderersperf);
				GUILayout.Label(item.meshrenderers + " meshrenderer");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.dynboneamountperf);
				GUILayout.Label(item.dynboneamount + " dynbones");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.dynbonecollidersperf);
				GUILayout.Label(item.dynbonecolliders + " dynbonecolliders");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.particle_systemsperf);
				GUILayout.Label(item.particle_systems + " particles");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.audio_sourcesperf);
				GUILayout.Label(item.audio_sources + " audios");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.lightsperf);
				GUILayout.Label(item.lights + " lights");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.animatorsperf);
				GUILayout.Label(item.animators + " animators");
				GUI.color = PerformanceProfile.doubleToPerfColor(item.perfP.clothperf);
				GUILayout.Label(item.cloth + " cloths");
				GUI.color = Color.white;
			}
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			AvatarsContainer.kpsscene[item.name] = GUILayout.Button("scene");
			AvatarsContainer.kpsfolder[item.name] = GUILayout.Button("folder");
			AvatarsContainer.kpsdelete[item.name] = GUILayout.Button("delete");
			GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

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
        GUILayout.Label("add avatar", EditorStyles.boldLabel);

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
}