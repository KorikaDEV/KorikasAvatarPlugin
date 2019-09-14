using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using VRCSDK2;
using UnityEditor.SceneManagement;
using KATStuff;

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

        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval))
        {
            AssetDatabase.CreateFolder("Assets/KATAvatars", nameval);
        }
        createKATFolder("Animations");
        createKATFolder("Audio");
        createKATFolder("Materials");
        createKATFolder("Textures");
        createKATFolder("Shaders");

        //Generated Folders
        updateProgressBar("copying avatar...", 1.0f);

        string sourcepath = AssetDatabase.GetAssetPath(selected);
        AssetDatabase.CopyAsset(sourcepath, "Assets/KATAvatars/" + nameval + "/" + nameval + ".fbx");

        //Copied Avatar
        updateProgressBar("generating animation files...", 1.0f);

        CreateAnimationFiles(nameval);

        //Generated Animation Files
        updateProgressBar("instantiating avatar...", 1.0f);

        EditorApplication.NewScene();
        GameObject newobj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/KATAvatars/" + nameval + "/" + nameval + ".fbx", typeof(GameObject));
        newobj = (GameObject)Instantiate(newobj, new Vector3(0, 0, 0), Quaternion.identity);

        //Instantiated Avatar
        updateProgressBar("instantiating viewpointsetter...", 1.0f);

        GameObject view = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Korikas-Avatar-Tool/Korikas Avatar Tools/Examples/Prefabs/ViewpointSetter.prefab", typeof(GameObject));
        view = (GameObject)Instantiate(view, new Vector3(0, 0, 0), Quaternion.identity);
        view.name = "ViewpointSetter";

        //Instantiated ViewpointSetter
        updateProgressBar("instantiating animator...", 1.0f);

        GameObject newobjanimator = (GameObject)Instantiate(newobj, new Vector3(0, 0, 0), Quaternion.identity);
        newobjanimator.name = "animator";
        newobjanimator.active = false;
        Animator animator = newobjanimator.GetComponent<Animator>();
        string path = "Assets/KATAvatars/" + nameval + "/Animations/";
        if (animator.runtimeAnimatorController == null)
        {
            UnityEditor.Animations.AnimatorController acnew = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path + "animator.controller");
            animator.runtimeAnimatorController = acnew;
        }
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
        GestureDisplay.addMotionToControllerByPath(path + "fingerpoint.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "fist.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "victory.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "handgun.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "thumbsup.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "rocknroll.anim", ac);
        GestureDisplay.addMotionToControllerByPath(path + "handopen.anim", ac);

        //Instantiated Animator
        updateProgressBar("creating katprofile...", 1.0f);

        newobj.name = nameval;

        KatProfile kp = new KatProfile(newobj);
        kp.saveFile();

        //Created Katprofile
        updateProgressBar("deleting old file and adding the VRC_AvatarDescriptor...", 1);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected));

        VRC_AvatarDescriptor vrcad = newobj.AddComponent(typeof(VRC_AvatarDescriptor)) as VRC_AvatarDescriptor;
        AnimatorOverrideController Ovrd = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath("Assets/KATAvatars/" + nameval + "/Animations/" + nameval + ".overrideController", typeof(AnimatorOverrideController));
        vrcad.CustomSittingAnims = Ovrd;
        vrcad.CustomStandingAnims = Ovrd;

        addLipSync(newobj);

        //Deleted old File and added the VRC_AvatarDescriptor
        updateProgressBar("saving scene and refreshing files...", 1.0f);

        EditorApplication.SaveScene("Assets/KATAvatars/" + nameval + "/" + nameval + ".unity");
		AssetDatabase.Refresh();

        //Saved Scene and refreshed Files
        EditorUtility.ClearProgressBar();
    }

    public static void updateProgressBar(string task, float addvalue){
        progressval = progressval + addvalue;
        EditorUtility.DisplayProgressBar("Avatar Structure Building", task, progressval / 8.0f);
    }

    public static void createKATRootFolder(){
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars"))
        {
            AssetDatabase.CreateFolder("Assets", "KATAvatars");
        }
    }
    public static void createKATFolder(string name){
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval + "/" + name))
        {
            AssetDatabase.CreateFolder("Assets/KATAvatars/" + nameval, name);
        }
    }

    public static void CreateAnimationFiles(string name)
    {
        string path = "Assets/KATAvatars/" + name + "/Animations/";
        AssetDatabase.CopyAsset("Assets/VRCSDK/Examples/Sample Assets/Animation/CustomOverrideEmpty.overrideController", path + name + ".overrideController");
        AnimationClip fingerpoint = new AnimationClip();
        AssetDatabase.CreateAsset(fingerpoint, path + "fingerpoint.anim");
        AnimationClip fist = new AnimationClip();
        AssetDatabase.CreateAsset(fist, path + "fist.anim");
        AnimationClip victory = new AnimationClip();
        AssetDatabase.CreateAsset(victory, path + "victory.anim");
        AnimationClip handgun = new AnimationClip();
        AssetDatabase.CreateAsset(handgun, path + "handgun.anim");
        AnimationClip thumbsup = new AnimationClip();
        AssetDatabase.CreateAsset(thumbsup, path + "thumbsup.anim");
        AnimationClip rocknroll = new AnimationClip();
        AssetDatabase.CreateAsset(rocknroll, path + "rocknroll.anim");
        AnimationClip handopen = new AnimationClip();
        AssetDatabase.CreateAsset(handopen, path + "handopen.anim");

        AnimatorOverrideController animc = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath(path + name + ".overrideController", typeof(AnimatorOverrideController));
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
            for (int i = 0; i < m.blendShapeCount; i++)
            {
                string name = m.GetBlendShapeName(i);
                for (int ii = 0; ii < blendshapes.Length; ii++)
                {
                    if (name.Contains(blendshapes[ii]))
                    {
                        blendshapes[ii] = name;
                    }
                }
            }
            vrcad.VisemeBlendShapes = blendshapes;
        }
    }
}
