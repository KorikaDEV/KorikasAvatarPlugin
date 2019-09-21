using UnityEngine;
using System.Collections.Generic;

public class BoneHelper : MonoBehaviour {
    
    public static DynamicBone[] getAllDynBones(Transform avatar){
        Transform hips = avatar.gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
        Transform[] hipschilds = checkForEveryChildren(hips, new List<Transform>()).ToArray();

        List<DynamicBone> result = new List<DynamicBone>();

        foreach (Transform t in hipschilds)
        {
            if(t.gameObject.GetComponent<DynamicBone>()){
                result.Add(t.gameObject.GetComponent<DynamicBone>());
            }
        }

        return result.ToArray();
    }

    public static List<Transform> checkForEveryChildren(Transform t, List<Transform> tl){
        int count = t.childCount;
        if(count > 0){
            for (int i = 0; i < count; i++)
            {
                Transform tf = t.GetChild(i);
                tl.Add(tf);
                tl = checkForEveryChildren(tf, tl);
            }
            return tl;
        }else{
            return tl;
        }
    }
}