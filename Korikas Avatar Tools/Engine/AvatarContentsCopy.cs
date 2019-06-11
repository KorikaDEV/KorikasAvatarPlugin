using UnityEngine;
using System;

public class AvatarContentsCopy {

	public static Component CopyComponent(Component original, GameObject destination)
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields(); 
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy;
	}

	public static void copy(GameObject source, GameObject destination, bool[] options){
		if(options[0] == true)
        {
            options[1] = false;
        }
		if (options [0] == true) {
			copyGameObjects(source, destination);
		}
		if (options [1] == true) {
			copyParticleSystems(source, destination);
		}
		if (options [2] == true) {
			copyDynamicBones (source, destination);
		}
    }
	//COPY PARTICLE SYSTEMS
	public static void copyParticleSystems(GameObject source, GameObject destination){
		Animator anSource = source.GetComponent<Animator>();
		Animator anDestination = destination.GetComponent<Animator>();
		//HIPS
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.Hips);
		//SPINE
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.Spine);
		//CHEST
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.Chest);
		//NECK
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.Neck);
		//HEAD
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.Head);

		//LEFTUPPERLEG
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftUpperLeg);
		//LEFTLOWERELG
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftLowerLeg);
		//LEFTFOOT
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftFoot);

		//RIGHTUPPERLEG
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightUpperLeg);
		//RIGHTLOWERLEG
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightLowerLeg);
		//RIGHTFOOT
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightFoot);

		//LEFTUPPERARM
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftUpperArm);
		//LEFTLOWERARM
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftLowerArm);
		//LEFTHAND
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.LeftHand);

		//RIGHTUPPERARM
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightUpperArm);
		//RIGHTLOWERARM
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightLowerArm);
		//RIGHTHAND
		checkAndCopyParticleSystems(anSource, anDestination, HumanBodyBones.RightHand);
	}
	public static void checkAndCopyParticleSystems(Animator from, Animator to, HumanBodyBones bone)
	{
		foreach (Transform child in from.GetBoneTransform(bone))
		{
			ParticleSystem ps = child.GetComponent<ParticleSystem> ();
			if (ps) {
				string name = child.gameObject.name;
				GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, to.GetBoneTransform(bone));
				g.name = name;
			}
		}
		removeDoubles(to, bone);
	}
	//COPY DYNAMIC BONES
	public static void copyDynamicBones(GameObject source, GameObject destination){
		Animator anSource = source.GetComponent<Animator>();
		Animator anDestination = destination.GetComponent<Animator>();
		//HIPS
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.Hips);
		//SPINE
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.Spine);
		//CHEST
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.Chest);
		//NECK
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.Neck);
		//HEAD
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.Head);

		//LEFTUPPERLEG
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftUpperLeg);
		//LEFTLOWERELG
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftLowerLeg);
		//LEFTFOOT
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftFoot);

		//RIGHTUPPERLEG
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightUpperLeg);
		//RIGHTLOWERLEG
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightLowerLeg);
		//RIGHTFOOT
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightFoot);

		//LEFTUPPERARM
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftUpperArm);
		//LEFTLOWERARM
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftLowerArm);
		//LEFTHAND
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.LeftHand);

		//RIGHTUPPERARM
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightUpperArm);
		//RIGHTLOWERARM
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightLowerArm);
		//RIGHTHAND
		checkDoublesAndCopyDynamicBones(anSource, anDestination, HumanBodyBones.RightHand);

	}

	public static void checkDoublesAndCopyDynamicBones (Animator from, Animator to, HumanBodyBones bone){
		foreach (Transform child in from.GetBoneTransform(bone)) {
			DynamicBone dyn = child.gameObject.GetComponent<DynamicBone> ();
			if (dyn) {
				foreach (Transform childto in to.GetBoneTransform(bone)) {
					DynamicBone dynto = childto.gameObject.GetComponent<DynamicBone> ();
					if (!dynto) {
						if (child.gameObject.name == childto.gameObject.name) {
							CopyComponent (dyn, childto.gameObject);
							DynamicBone dyncopy = childto.gameObject.GetComponent<DynamicBone>();
							dyncopy.m_Root = childto.gameObject.transform;
						}
					}
				}
			}
		}
	}

	//COPY GAMEOBJECTS
    public static void copyGameObjects(GameObject source, GameObject destination)
    {
        Animator anSource = source.GetComponent<Animator>();
        Animator anDestination = destination.GetComponent<Animator>();

        //HIPS
        foreach(Transform child in anSource.GetBoneTransform(HumanBodyBones.Hips))
        {
            if(child != anSource.GetBoneTransform(HumanBodyBones.Spine) &&
                child != anSource.GetBoneTransform(HumanBodyBones.LeftUpperLeg) &&
                child != anSource.GetBoneTransform(HumanBodyBones.RightUpperLeg))
            {
                string name = child.gameObject.name;
                GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, anDestination.GetBoneTransform(HumanBodyBones.Hips));
                g.name = name;
            }
        }
        removeDoubles(anDestination, HumanBodyBones.Hips);
        //SPINE
        checkWithOneChild(HumanBodyBones.Spine, HumanBodyBones.Chest, anSource, anDestination);
        //CHEST
        foreach (Transform child in anSource.GetBoneTransform(HumanBodyBones.Chest))
        {
            if (child != anSource.GetBoneTransform(HumanBodyBones.Neck) &&
                child != anSource.GetBoneTransform(HumanBodyBones.LeftShoulder) &&
                child != anSource.GetBoneTransform(HumanBodyBones.RightShoulder))
            {
                string name = child.gameObject.name;
                GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, anDestination.GetBoneTransform(HumanBodyBones.Chest));
                g.name = name;
            }
        }
        removeDoubles(anDestination, HumanBodyBones.Chest);
        //NECK
        checkWithOneChild(HumanBodyBones.Neck, HumanBodyBones.Head, anSource, anDestination);
        //HEAD
        checkWithOneChild(HumanBodyBones.Head, HumanBodyBones.Neck, anSource, anDestination);

        //LEFTUPPERLEG
        checkWithOneChild(HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, anSource, anDestination);
        //LEFTLOWERLEG
        checkWithOneChild(HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot, anSource, anDestination);
        //LEFTFOOT
        checkWithOneChild(HumanBodyBones.LeftFoot, HumanBodyBones.LeftFoot, anSource, anDestination);

        //RIGHTUPPERLEG
        checkWithOneChild(HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, anSource, anDestination);
        //RIGHTLOWERLEG
        checkWithOneChild(HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot, anSource, anDestination);
        //RIGHTFOOT
        checkWithOneChild(HumanBodyBones.RightFoot, HumanBodyBones.LeftFoot, anSource, anDestination);

        //LEFTUPPERARM
        checkWithOneChild(HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm, anSource, anDestination);
        //LEFTLOWERARM
        checkWithOneChild(HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand, anSource, anDestination);
        //LEFTHAND
        checkWithOneChild(HumanBodyBones.LeftHand, HumanBodyBones.LeftHand, anSource, anDestination);

        //RIGHTUPPERARM
        checkWithOneChild(HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm, anSource, anDestination);
        //RIGHTLOWERARM
        checkWithOneChild(HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand, anSource, anDestination);
        //RIGHTHAND
        checkWithOneChild(HumanBodyBones.RightHand, HumanBodyBones.RightHand, anSource, anDestination);



    }
    public static void checkWithOneChild(HumanBodyBones yesbone, HumanBodyBones nobone, Animator from, Animator to)
    {
        foreach (Transform child in from.GetBoneTransform(yesbone))
        {
            if (child != from.GetBoneTransform(nobone))
            {
                string name = child.gameObject.name;
                GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, to.GetBoneTransform(yesbone));
                g.name = name;
            }
        }
        removeDoubles(to, yesbone);
    }
    public static void removeDoubles(Animator to, HumanBodyBones yesbone)
    {
        string[] namelist = new string[5000];
        foreach (Transform child in to.GetBoneTransform(yesbone))
        {
            string name = child.gameObject.name;
            if (Array.IndexOf(namelist, name) >= 0)
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
            else
            {
                namelist[namelist.Length - 1] = name;
            }
        }

    }
}
