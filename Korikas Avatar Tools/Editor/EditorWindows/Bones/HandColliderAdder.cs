using UnityEngine;

public class HandColliderAdder : MonoBehaviour {
	
    public static void addColliderIfDontExistsAndUpdate(GameObject g, float position, float size){
        Animator an = g.GetComponent<Animator>();
        checkAdd(HumanBodyBones.LeftHand, an, position, size);
        checkAdd(HumanBodyBones.RightHand, an, position, size);
    }

    public static void checkAdd(HumanBodyBones bone, Animator an, float position, float size){
        Transform child = an.GetBoneTransform(bone);
        DynamicBoneCollider collider = child.gameObject.GetComponent<DynamicBoneCollider>();
        if(!collider){
               DynamicBoneCollider dynbonec =  child.gameObject.AddComponent(typeof(DynamicBoneCollider)) as DynamicBoneCollider;
        }else{
            collider.m_Radius = size;
            collider.m_Center = new Vector3 (0,position,0);
        }
    }
}