using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using VRCSDK2;
using UnityEditor.SceneManagement;
using KAPStuff;

public class AvatarStructureBuilder : MonoBehaviour
{
    public static string nameval;
    public static float progressval = 0f;

    public static void BuildOverride(GameObject selected)
    {
        progressval = 0f;
        updateProgressBar("generating folders...", 0.0f);

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        nameval = selected.name;

        if (!AssetDatabase.IsValidFolder("Assets/KAPAvatars/" + nameval))
        {
            AssetDatabase.CreateFolder("Assets/KAPAvatars", nameval);
        }
        createKAPFolder("Animations");
        createKAPFolder("Audio");
        createKAPFolder("Materials");
        createKAPFolder("Textures");
        createKAPFolder("Shaders");
        createKAPFolder("Controllers");

        //Generated Folders
        updateProgressBar("copying avatar...", 1.0f);

        string sourcepath = AssetDatabase.GetAssetPath(selected);
        AssetDatabase.CopyAsset(sourcepath, "Assets/KAPAvatars/" + nameval + "/" + nameval + ".fbx");

        //Copied Avatar
        updateProgressBar("generating animation files...", 1.0f);

        CreateAnimationFiles(nameval);

        //Generated Animation Files
        updateProgressBar("instantiating avatar...", 1.0f);

        EditorApplication.NewScene();
        GameObject newobj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/KAPAvatars/" + nameval + "/" + nameval + ".fbx", typeof(GameObject));
        newobj = (GameObject)Instantiate(newobj, new Vector3(0, 0, 0), Quaternion.identity);

        //Instantiated Avatar
        updateProgressBar("instantiating viewpointsetter...", 1.0f);

        GameObject view = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/KorikasAvatarPlugin/Korikas Avatar Plugin/Examples/Prefabs/ViewpointSetter.prefab", typeof(GameObject));
        view = (GameObject)Instantiate(view, new Vector3(0, 0, 0), Quaternion.identity);
        view.name = "ViewpointSetter";

        //Instantiated ViewpointSetter
        updateProgressBar("creating KAPprofile...", 1.0f);

        newobj.name = nameval;

        KAPProfile kp = new KAPProfile(newobj);
        kp.saveFile();

        //Created KAPprofile
        updateProgressBar("deleting old file and adding the VRC_AvatarDescriptor...", 1);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected));

        VRC_AvatarDescriptor vrcad = newobj.AddComponent(typeof(VRC_AvatarDescriptor)) as VRC_AvatarDescriptor;
        AnimatorOverrideController Ovrd = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath("Assets/KAPAvatars/" + nameval + "/" + nameval + ".overrideController", typeof(AnimatorOverrideController));
        vrcad.CustomSittingAnims = Ovrd;
        vrcad.CustomStandingAnims = Ovrd;

        addLipSync(newobj);

        //Deleted old File and added the VRC_AvatarDescriptor
        updateProgressBar("saving scene and refreshing files...", 1.0f);

        EditorApplication.SaveScene("Assets/KAPAvatars/" + nameval + "/" + nameval + ".unity");
		AssetDatabase.Refresh();

        //Saved Scene and refreshed Files
        EditorUtility.ClearProgressBar();
    }

    public static void updateProgressBar(string task, float addvalue){
        progressval = progressval + addvalue;
        EditorUtility.DisplayProgressBar("Avatar Structure Builder", task, progressval / 8.0f);
    }

    public static void createKAPRootFolder(){
        if (!AssetDatabase.IsValidFolder("Assets/KAPAvatars"))
        {
            AssetDatabase.CreateFolder("Assets", "KAPAvatars");
        }
    }
    public static void createKAPFolder(string name){
        createFolder("Assets/KAPAvatars/" + nameval, name);
    }
    public static void createFolder(string path, string name){
        if (!AssetDatabase.IsValidFolder(path + "/" + name))
        {
            AssetDatabase.CreateFolder(path, name);
        }
    }
    public static AnimationClip ExampleAnim(){
        return (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/KorikasAvatarPlugin/Korikas Avatar Plugin/Examples/Animations/ExampleGesture.anim", typeof(AnimationClip));
    }
    public static void CreateAnimationFiles(string name)
    {
        string path = "Assets/KAPAvatars/" + name + "/Animations/";
        string examplepath = "Assets/KorikasAvatarPlugin/Korikas Avatar Plugin/Examples/Animations/ExampleGesture.anim";
        AssetDatabase.CopyAsset("Assets/VRChat Examples/Examples2/Animation/SDK2/CustomOverrideEmpty.overrideController", "Assets/KAPAvatars/" + name + "/" + name + ".overrideController");
        AssetDatabase.CopyAsset(examplepath, path + "fingerpoint.anim");
        AssetDatabase.CopyAsset(examplepath, path + "fist.anim");
        AssetDatabase.CopyAsset(examplepath, path + "victory.anim");
        AssetDatabase.CopyAsset(examplepath, path + "handgun.anim");
        AssetDatabase.CopyAsset(examplepath, path + "thumbsup.anim");
        AssetDatabase.CopyAsset(examplepath, path + "rocknroll.anim");
        AssetDatabase.CopyAsset(examplepath, path + "handopen.anim");

        AnimatorOverrideController animc = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath("Assets/KAPAvatars/" + name + "/" + name + ".overrideController", typeof(AnimatorOverrideController));
        animc["FINGERPOINT"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "fingerpoint.anim", typeof(AnimationClip));
        animc["FIST"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "fist.anim", typeof(AnimationClip));
        animc["VICTORY"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "victory.anim", typeof(AnimationClip));
        animc["HANDGUN"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "handgun.anim", typeof(AnimationClip));
        animc["THUMBSUP"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "thumbsup.anim", typeof(AnimationClip));
        animc["ROCKNROLL"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "rocknroll.anim", typeof(AnimationClip));
        animc["HANDOPEN"] = (AnimationClip)AssetDatabase.LoadAssetAtPath(path + "handopen.anim", typeof(AnimationClip));
    }

    public static void addLipSync(GameObject avatar)
    {
        VRC_AvatarDescriptor vrcad = avatar.GetComponent<VRC_AvatarDescriptor>();
        SkinnedMeshRenderer[] meshes = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (meshes.Length > 0)
        {
            vrcad.VisemeSkinnedMesh = meshes[0];
            vrcad.lipSync = VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;

            SkinnedMeshRenderer head = meshes[0];
            Mesh m = head.sharedMesh;
            string[] blendshapes = { "sil", "pp", "ff", "th", "dd", "kk", "ch", "ss", "nn", "rr", "aa", "_e", "ih", "oh", "ou" };
            string[] appliedbss = { "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-", "-none-" };
            for (int i = 0; i < m.blendShapeCount; i++)
            {
                string name = m.GetBlendShapeName(i);
                for (int ii = 0; ii < blendshapes.Length; ii++)
                {
                    if (name.Contains(blendshapes[ii]))
                    {
                        appliedbss[ii] = name;
                    }
                }
            }
            if(m.blendShapeCount >= 14){
                vrcad.VisemeBlendShapes = appliedbss;
            }else{
                vrcad.lipSync = VRC_AvatarDescriptor.LipSyncStyle.Default;
            }
        }
    }
}
