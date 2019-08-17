using UnityEngine;
using System.Collections.Generic;
public class AvatarsContainer : MonoBehaviour {
    public static Dictionary<string, bool> kpsfoldout = new Dictionary<string, bool>();
    public static Dictionary<string, bool> kpsdelete = new Dictionary<string, bool>();
    public static Dictionary<string, bool> kpsfolder = new Dictionary<string, bool>();
    public static Dictionary<string, bool> kpsscene = new Dictionary<string, bool>();

    public static void initKPValue(string name){
        if(!kpsfoldout.ContainsKey(name)){
            kpsfoldout.Add(name, false);
        }
        if(!kpsdelete.ContainsKey(name)){
            kpsdelete.Add(name, false);
        }
        if(!kpsfolder.ContainsKey(name)){
            kpsfolder.Add(name, false);
        }
        if(!kpsscene.ContainsKey(name)){
            kpsscene.Add(name, false);
        }
    }
}