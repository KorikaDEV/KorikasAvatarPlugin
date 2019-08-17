using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Collections;
using System;
using KATStuff;

public class BeatFinder : MonoBehaviour
{

    public static void generateBeatAnimation(TextAsset text, float zoom, float blur, Color beatcolor, Color betweencolor, AudioClip audio)
    {
        GameObject source = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Korikas-Avatar-Tool/Korikas Avatar Tools/Examples/Prefabs/BeatFinderPrefab.prefab", typeof(GameObject));
        string name = source.name;
        source = (GameObject)Instantiate(source, new Vector3(0, 0, 0), Quaternion.identity);
        source.name = name;

        float[] fl = textToFloatArray(text);
        AnimationClip ac = new AnimationClip();
        ac.legacy = true;
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._blurfactor", "zoom", 0f, blur, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._mag", "zoom", 1f, zoom, 0.5f);
        AddKeyFrames(typeof(GameObject), fl, ac, "m_IsActive", "trigger", 0f, 0.01f, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._greenvalue", "zoom", betweencolor.g, beatcolor.g, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._bluevalue", "zoom", betweencolor.b, beatcolor.b, 0.5f);
        AddKeyFrames(typeof(MeshRenderer), fl, ac, "material._redvalue", "zoom", betweencolor.r, beatcolor.r, 0.5f);
        AddKeyFrames(typeof(ParticleSystem), fl, ac, "simulationSpeed", "example", 0.5f, 1f, 0.5f);

        ac.legacy = false;
        AssetDatabase.CreateAsset(ac, "Assets/"+audio.name+".anim");

        source.GetComponent<AudioSource>().clip = audio;
        Animator a = source.GetComponent<Animator>();
        UnityEditor.Animations.AnimatorController acnew = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/"+audio.name+".controller");
        a.runtimeAnimatorController = acnew;
        AnimatorController anc = a.runtimeAnimatorController as AnimatorController;
        GestureDisplay.addMotionToControllerByPath("Assets/"+audio.name+".anim", anc);
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