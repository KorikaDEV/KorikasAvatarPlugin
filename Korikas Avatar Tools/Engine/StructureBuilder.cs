using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using VRCSDK2;

public class OverrideBuilder : MonoBehaviour{
    public static void BuildOverride(GameObject selected) {

        string nameval = selected.name;

        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars")) {
            AssetDatabase.CreateFolder("Assets", "KATAvatars");
        }
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval)) {
            AssetDatabase.CreateFolder("Assets/KATAvatars", nameval);
        }
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval + "/Animations")) {
            AssetDatabase.CreateFolder("Assets/KATAvatars/" + nameval, "Animations");
        }
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval + "/Materials")) {
            AssetDatabase.CreateFolder("Assets/KATAvatars/" + nameval, "Materials");
        }
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval + "/Textures")) {
            AssetDatabase.CreateFolder("Assets/KATAvatars/" + nameval, "Textures");
        }
        if (!AssetDatabase.IsValidFolder("Assets/KATAvatars/" + nameval + "/Shaders")) {
            AssetDatabase.CreateFolder("Assets/KATAvatars/" + nameval, "Shaders");
        }

        string sourcepath = AssetDatabase.GetAssetPath(selected);
        AssetDatabase.CopyAsset(sourcepath, "Assets/KATAvatars/" + nameval + "/" + nameval + ".fbx");
		CreateAnimationFiles (nameval);
        EditorApplication.NewScene();
        GameObject newobj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/KATAvatars/" + nameval + "/" + nameval + ".fbx", typeof(GameObject));
        newobj = (GameObject)Instantiate(newobj, new Vector3(0, 0, 0), Quaternion.identity);

		GameObject newobjanimator = (GameObject)Instantiate(newobj, new Vector3(0, 0, 0), Quaternion.identity);
		newobjanimator.name = "animator";
		newobjanimator.active = false;
		Animator animator = newobjanimator.GetComponent<Animator> ();
		string path = "Assets/KATAvatars/" + nameval + "/Animations/";
		if(animator.runtimeAnimatorController == null){
			UnityEditor.Animations.AnimatorController acnew = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (path + "animator.controller");
			animator.runtimeAnimatorController = acnew;
		}
		AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
		GestureDisplay.addMotionToController ("fingerpoint", ac, path);
		GestureDisplay.addMotionToController ("fist", ac, path);
		GestureDisplay.addMotionToController ("victory", ac, path);
		GestureDisplay.addMotionToController ("handgun", ac, path);
		GestureDisplay.addMotionToController ("thumbsup", ac, path);
		GestureDisplay.addMotionToController ("rocknroll", ac, path);
		GestureDisplay.addMotionToController ("handopen", ac, path);

        newobj.name = nameval;
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected));

        VRC_AvatarDescriptor vrcad = newobj.AddComponent(typeof(VRC_AvatarDescriptor)) as VRC_AvatarDescriptor;
        AnimatorOverrideController Ovrd = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath("Assets/KATAvatars/" + nameval + "/Animations/" + nameval + ".overrideController", typeof(AnimatorOverrideController));
        vrcad.CustomSittingAnims = Ovrd;
        vrcad.CustomStandingAnims = Ovrd;

		addLipSync (newobj);

        EditorApplication.SaveScene("Assets/KATAvatars/" + nameval + "/" + nameval + ".unity");
    }

	public static void CreateAnimationFiles(string name){
		string path = "Assets/KATAvatars/" + name + "/Animations/";
		AssetDatabase.CopyAsset("Assets/VRCSDK/Examples/Sample Assets/Animation/CustomOverrideEmpty.overrideController", path + name + ".overrideController");
		AnimationClip fingerpoint = new AnimationClip ();
		AssetDatabase.CreateAsset (fingerpoint, path + "fingerpoint.anim");
		AnimationClip fist = new AnimationClip ();
		AssetDatabase.CreateAsset (fist, path + "fist.anim");
		AnimationClip victory = new AnimationClip ();
		AssetDatabase.CreateAsset (victory, path + "victory.anim");
		AnimationClip handgun = new AnimationClip ();
		AssetDatabase.CreateAsset (handgun, path + "handgun.anim");
		AnimationClip thumbsup = new AnimationClip ();
		AssetDatabase.CreateAsset (thumbsup, path + "thumbsup.anim");
		AnimationClip rocknroll = new AnimationClip ();
		AssetDatabase.CreateAsset (rocknroll, path + "rocknroll.anim");
		AnimationClip handopen = new AnimationClip ();
		AssetDatabase.CreateAsset (handopen, path + "handopen.anim");

		AnimatorOverrideController animc = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath (path + name + ".overrideController", typeof(AnimatorOverrideController));
		animc ["FINGERPOINT"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "fingerpoint.anim", typeof(AnimationClip));
		animc ["FIST"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "fist.anim", typeof(AnimationClip));
		animc ["VICTORY"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "victory.anim", typeof(AnimationClip));
		animc ["HANDGUN"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "handgun.anim", typeof(AnimationClip));
		animc ["THUMBSUP"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "thumbsup.anim", typeof(AnimationClip));
		animc ["ROCKNROLL"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "rocknroll.anim", typeof(AnimationClip));
		animc ["HANDOPEN"] = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + "handopen.anim", typeof(AnimationClip));
	}

	public static void addLipSync(GameObject avatar){
		VRC_AvatarDescriptor vrcad = avatar.GetComponent<VRC_AvatarDescriptor> ();
		SkinnedMeshRenderer[] meshes = avatar.GetComponentsInChildren<SkinnedMeshRenderer> ();
		if (meshes.Length > 0) {
			vrcad.VisemeSkinnedMesh = meshes [0];
			vrcad.lipSync = VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;

			SkinnedMeshRenderer head = meshes[0];
			Mesh m = head.sharedMesh;
			string[] blendshapes = {"sil","pp","ff","th","dd","kk","ch","ss","nn","rr","aa","ee","ih","oh","ou"}; 
			for (int i = 0; i < m.blendShapeCount; i++) {
				string name = m.GetBlendShapeName(i);
				for (int ii = 0; ii < blendshapes.Length; ii++) {
					if (name.Contains (blendshapes[ii])) {
						blendshapes [ii] = name;
					}
				}
			}
			vrcad.VisemeBlendShapes = blendshapes;
		}
	}
}
