using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using VRCSDK2;

namespace KATStuff
{
    public class GestureDisplay
    {
        public static void show(string name)
        {
            GameObject avatar = getVRCSceneAvatar();
            if (avatar != null)
            {
                Animator animator = avatar.GetComponent<Animator>();
                string path = "Assets/KATAvatars/" + avatar.name + "/Animations/";
                if (animator.runtimeAnimatorController == null)
                {
                    UnityEditor.Animations.AnimatorController ac = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path + "override.controller");
                    animator.runtimeAnimatorController = ac;
                }
                PlayMotion(path, name, animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController);
            }
        }
        public static void PlayMotion(string path, string name, UnityEditor.Animations.AnimatorController ac)
        {

            UnityEditor.Animations.AnimatorStateMachine sm = ac.layers[0].stateMachine;

            for (int i = 0; i < sm.states.Length; i++)
            {
                sm.RemoveState(sm.states[i].state);
            }
            addMotionToController(name, ac);
        }
        public static void addMotionToController(string name, UnityEditor.Animations.AnimatorController ac)
        {
            GameObject av = getVRCSceneAvatar();
            AnimatorOverrideController controller = av.GetComponent<VRC_AvatarDescriptor>().CustomStandingAnims;
            AnimationClip anclip = controller[name];
            Motion fistmotion = (Motion)anclip as Motion;
            ac.AddMotion(fistmotion);
        }
        public static void addMotionToControllerByPath(string path, UnityEditor.Animations.AnimatorController ac)
        {
            AnimationClip fist = (AnimationClip)AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip));
            Motion fistmotion = (Motion)fist as Motion;
        }
        public static GameObject getVRCSceneAvatar()
        {
            VRC_AvatarDescriptor[] vrcads = Object.FindObjectsOfType<VRC_AvatarDescriptor>();
            if (vrcads.Length > 0)
            {
                VRC_AvatarDescriptor vrcad = vrcads[0];
                GameObject avatar = vrcad.gameObject;
                return avatar;
            }
            return null;
        }
    }
}
