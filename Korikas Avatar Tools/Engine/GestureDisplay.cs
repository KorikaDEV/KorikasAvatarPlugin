using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using VRCSDK2;

public class GestureDisplay {
	public static void show(string name){
		VRC_AvatarDescriptor[] vrcads = Object.FindObjectsOfType<VRC_AvatarDescriptor> ();
		if (vrcads.Length > 0) {
			VRC_AvatarDescriptor vrcad = vrcads[0];
			GameObject avatar = vrcad.gameObject;
			Animator animator = avatar.GetComponent<Animator> ();
			string path = "Assets/KATAvatars/" + avatar.name + "/Animations/";
			if(animator.runtimeAnimatorController == null){
				UnityEditor.Animations.AnimatorController ac = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (path + "override.controller");
				animator.runtimeAnimatorController = ac;
			}
			PlayMotion (path, name, animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController);
		}
	}
	public static void PlayMotion(string path, string name, UnityEditor.Animations.AnimatorController ac){

		UnityEditor.Animations.AnimatorStateMachine sm = ac.layers [0].stateMachine;

		for (int i = 0; i < sm.states.Length; i++) {
			sm.RemoveState (sm.states[i].state);
		}

		switch (name) {
		case "FINGERPOINT":
			addMotionToController ("fingerpoint", ac, path);
			break;
		case "FIST":
			addMotionToController ("fist", ac, path);
			break;
		case "VICTORY":
			addMotionToController ("victory", ac, path);
			break;
		case "HANDGUN":
			addMotionToController ("handgun", ac, path);
			break;
		case "THUMBSUP":
			addMotionToController ("thumbsup", ac, path);
			break;
		case "ROCKNROLL":
			addMotionToController ("rocknroll", ac, path);
			break;
		case "HANDOPEN":
			addMotionToController ("handopen", ac, path);
			break;
		}
	}
	public static void addMotionToController(string name, UnityEditor.Animations.AnimatorController ac, string path){
		AnimationClip fist = (AnimationClip)AssetDatabase.LoadAssetAtPath (path + name + ".anim", typeof(AnimationClip));
		Motion fistmotion = (Motion)fist as Motion;
		ac.AddMotion (fistmotion);
	}
}
