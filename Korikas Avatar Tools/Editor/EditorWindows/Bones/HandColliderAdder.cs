using UnityEngine;
using System.Collections.Generic;
using KATStuff;

public class HandColliderAdder : MonoBehaviour
{
    public static void addColliderIfDontExistsAndUpdate(GameObject g, Vector3 position, float size, bool addevery){
        Animator an = g.GetComponent<Animator>();
        checkAdd(HumanBodyBones.LeftHand, an, position, size);
        checkAdd(HumanBodyBones.RightHand, an, position, size);

        if(addevery){
            addToEveryDynBone(an);
        }
    }

    public static void checkAdd(HumanBodyBones bone, Animator an, Vector3 position, float size){
        Transform child = an.GetBoneTransform(bone);
        DynamicBoneCollider collider = child.gameObject.GetComponent<DynamicBoneCollider>();
        if(!collider){
               DynamicBoneCollider dynbonec =  child.gameObject.AddComponent(typeof(DynamicBoneCollider)) as DynamicBoneCollider;
        }else{
            collider.m_Radius = size;
            collider.m_Center = new Vector3 (position.x,position.y,position.z);
        }
    }

    public static void addToEveryDynBone(Animator an){
        DynamicBoneCollider left = an.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<DynamicBoneCollider>();
        DynamicBoneCollider right = an.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<DynamicBoneCollider>();

        DynamicBone[] dyns = BoneHelper.getAllDynBones(GestureDisplay.getVRCSceneAvatar().transform);

        foreach (DynamicBone item in dyns)
        {
            List<DynamicBoneCollider> colliderlist = new List<DynamicBoneCollider>();
            foreach (DynamicBoneCollider collider in item.m_Colliders)
            {
                colliderlist.Add(collider);
            }

            colliderlist.Add(left);
            colliderlist.Add(right);

            item.m_Colliders = colliderlist;
        }
    }
}