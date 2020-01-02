using UnityEngine;
using KAPStuff;
using VRCSDK2;
public class AvatarResizer : MonoBehaviour {
    public static void resize(float size){
        GameObject avatar = GestureDisplay.getVRCSceneAvatar();

        float height_cm = getCurrentSize();
        if(size > 0.0f){
            float percent = size / height_cm;
            avatar.transform.localScale *= percent;
            avatar.GetComponent<VRC_AvatarDescriptor>().ViewPosition *= percent;
        }
    }
    public static float getCurrentSize(){
        GameObject avatar = GestureDisplay.getVRCSceneAvatar();

        Renderer[] rr = avatar.GetComponentsInChildren<Renderer>();
        Bounds b = rr[0].bounds;
        foreach(Renderer r in rr)
        {
            b.Encapsulate(r.bounds);
        }
        return b.size.y;
    }
}