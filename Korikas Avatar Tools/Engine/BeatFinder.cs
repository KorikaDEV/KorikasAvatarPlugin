using UnityEngine;
using UnityEditor;
public class BeatFinder {

    public static void generateBeatAnimation(TextAsset text, GameObject source){
        
    }
    public static bool ifHasStructure(GameObject source){
        bool result = false;
        Animator an = source.GetComponent<Animator>();
        if(an){
            float anlength = an.GetCurrentAnimatorStateInfo(0).length;
            if(anlength > 0.016 && anlength < 0.017){
                result = true;
            }
        }
        return result;
    }
}