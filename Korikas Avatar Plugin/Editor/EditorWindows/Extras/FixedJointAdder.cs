using UnityEngine;
using KAPStuff;

public class FixedJointAdder : MonoBehaviour {
    public static void add(){
        Animator an = GestureDisplay.getVRCSceneAvatar().GetComponent<Animator>();
        Transform rh = an.GetBoneTransform(HumanBodyBones.RightHand);
        Transform lh = an.GetBoneTransform(HumanBodyBones.LeftHand);

        createFixedJointObject("FixedJoint_RH", rh);
        createFixedJointObject("FixedJoint_LH", lh);

    }

    public static void createFixedJointObject(string name, Transform hand){
        GameObject g = new GameObject(name);
        FixedJoint fxj = g.AddComponent<FixedJoint>();
        g.transform.parent = GestureDisplay.getVRCSceneAvatar().transform;
        Rigidbody rb = g.GetComponent<Rigidbody>();
        rb.angularDrag = 0.0f;
        rb.useGravity = false;
        Rigidbody handrb = hand.gameObject.GetComponent<Rigidbody>();
        if(!handrb){
            handrb = hand.gameObject.AddComponent<Rigidbody>();
        }
        handrb.angularDrag = 0.0f;
        handrb.useGravity = false;
        fxj.connectedBody = handrb;
        g.transform.position = hand.position;
    }
    public static bool hasFixedJoints(){
        bool result = false;
        GameObject g = GestureDisplay.getVRCSceneAvatar();
        FixedJoint[] fxjs = g.transform.GetComponentsInChildren<FixedJoint>();
        if(fxjs.Length > 0){
            result = true;
        }
        return result;
    }
}