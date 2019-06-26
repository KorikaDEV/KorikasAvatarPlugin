using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Collections;
using System;
public class BeatFinder
{

    public static void generateBeatAnimation(TextAsset text, GameObject source)
    {
        float[] fl = textToFloatArray(text);
        AnimationClip ac = new AnimationClip();
        ac.legacy = true;
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._blurfactor", "zoom", 1f, 3f, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._mag", "zoom", 1f, 1.013f, 0.5f);
        AddKeyFrames(typeof(GameObject), fl, ac, "m_IsActive", "trigger", 0f, 0.01f, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._greenvalue", "zoom", 0.05f, 0f, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._bluevalue", "zoom", 0.1f, 0.05f, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._redvalue", "zoom", 0f, 0.05f, 0.5f);

        AddKeyFrames(typeof(ParticleSystem), fl, ac, "simulationSpeed", "example", 0.5f, 1f, 0.5f);

        ac.legacy = false;
        AssetDatabase.CreateAsset(ac, "Assets/BeatFinderOutput.anim");
        Animator a = source.GetComponent<Animator>();
        if(a.runtimeAnimatorController == null){
			UnityEditor.Animations.AnimatorController acnew = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/animator.controller");
			a.runtimeAnimatorController = acnew;
		}
        AnimatorController anc = a.runtimeAnimatorController as AnimatorController;
        GestureDisplay.addMotionToController("BeatFinderOutput", anc, "Assets/");
    }

    private static void AddKeyFrames(Type kind, float[] fl, AnimationClip ac, string name, string path, float min, float max, float maxtimestamp)
    {
        List<Keyframe> keys = new List<Keyframe>();
        keys.Add(new Keyframe(0f, min));
        foreach (float f in fl)
        {
            int index = System.Array.IndexOf(fl, f);
            if (index > 0)
            {
                float step = ((fl[index] - fl[index - 1]));
                float betweenval;
                if(step / 5 > maxtimestamp){
                    betweenval = (step - maxtimestamp) + fl[index - 1];
                }else{
                    betweenval = (step / 5) * 4 + fl[index - 1];
                }
                keys.Add(new Keyframe(betweenval, min));
            }
            else
            {
                float step = fl[index];
                float betweenval;
                if(step / 5 > maxtimestamp){
                    keys.Add(new Keyframe(step - maxtimestamp, min));
                }else{
                    keys.Add(new Keyframe(((step / 5) * 4), min));
                }
            }
            keys.Add(new Keyframe(f, max));
        }
        AnimationCurve curve = new AnimationCurve(keys.ToArray());
        ac.SetCurve(path, kind, name, curve);
    }

    public static bool ifHasStructure(GameObject source)
    {
        if(source.GetComponent<Animator>()){
            return true;
        }else{
            return false;
        }
    }
    public static float[] textToFloatArray(TextAsset text)
    {
        string content = text.ToString();
        string[] v1 = content.Split('\n');
        List<float> ls = new List<float>();
        foreach (string line in v1)
        {
            string[] v2 = line.Split('\t');
            if (v2[0] != "")
            {
                ls.Add(float.Parse(v2[0]));
            }
        }
        return ls.ToArray();
    }
}