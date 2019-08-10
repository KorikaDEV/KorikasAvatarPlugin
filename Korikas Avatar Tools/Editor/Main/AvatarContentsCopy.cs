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
		Animator anSource = source.GetComponent<Animator>();
		Animator anDestination = destination.GetComponent<Animator>();
		//HIPS
		converter(options, anSource, anDestination, HumanBodyBones.Hips, HumanBodyBones.Spine);
		//SPINE
		converter(options, anSource, anDestination, HumanBodyBones.Spine, HumanBodyBones.Chest);
		//CHEST
		converter(options, anSource, anDestination, HumanBodyBones.Chest, HumanBodyBones.Neck);
		//NECK
		converter(options, anSource, anDestination, HumanBodyBones.Neck, HumanBodyBones.Head);
		//HEAD
		converter(options, anSource, anDestination, HumanBodyBones.Head, HumanBodyBones.Head);

		//LEFTUPPERLEG
		converter(options, anSource, anDestination, HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg);
		//LEFTLOWERLEG
		converter(options, anSource, anDestination, HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot);
		//LEFTFOOT
		converter(options, anSource, anDestination, HumanBodyBones.LeftFoot, HumanBodyBones.LeftFoot);

		//RIGHTUPPERLEG
		converter(options, anSource, anDestination, HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg);
		//RIGHTLOWERLEG
		converter(options, anSource, anDestination, HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot);
		//RIGHTFOOT
		converter(options, anSource, anDestination, HumanBodyBones.RightFoot, HumanBodyBones.RightFoot);

		//LEFTUPPERARM
		converter(options, anSource, anDestination, HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm);
		//LEFTLOWERARM
		converter(options, anSource, anDestination, HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand);
		//LEFTHAND
		converter(options, anSource, anDestination, HumanBodyBones.LeftHand, HumanBodyBones.LeftHand);

		//RIGHTUPPERARM
		converter(options, anSource, anDestination, HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm);
		//RIGHTLOWERARM
		converter(options, anSource, anDestination, HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand);
		//RIGHTHAND
		converter(options, anSource, anDestination, HumanBodyBones.RightHand, HumanBodyBones.RightHand);

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
	public static void converter(bool[] kind, Animator from, Animator to, HumanBodyBones bone, HumanBodyBones noAnime){
		if(kind[0] == true){
			if(bone == HumanBodyBones.Hips){
				foreach(Transform child in from.GetBoneTransform(HumanBodyBones.Hips))
				{
					if(child != from.GetBoneTransform(HumanBodyBones.Spine) &&
						child != from.GetBoneTransform(HumanBodyBones.LeftUpperLeg) &&
						child != from.GetBoneTransform(HumanBodyBones.RightUpperLeg))
					{
						string name = child.gameObject.name;
						GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, to.GetBoneTransform(HumanBodyBones.Hips));
						g.name = name;
					}
				}
				removeDoubles(to, HumanBodyBones.Hips);
			}else if(bone == HumanBodyBones.Chest){
				foreach(Transform child in from.GetBoneTransform(HumanBodyBones.Chest))
				{
					if (child != from.GetBoneTransform(HumanBodyBones.Neck) &&
                	child != from.GetBoneTransform(HumanBodyBones.LeftShoulder) &&
                	child != from.GetBoneTransform(HumanBodyBones.RightShoulder))
					{
						string name = child.gameObject.name;
						GameObject g = (GameObject)UnityEngine.Object.Instantiate(child.gameObject, to.GetBoneTransform(HumanBodyBones.Hips));
						g.name = name;
					}
				}
				removeDoubles(to, HumanBodyBones.Chest);
			}else{
				checkWithOneChild(bone, noAnime, from, to);
			}
		}
		if(kind[1] == true){
			checkAndCopyParticleSystems(from, to, bone);
		}
		if(kind[2] == true){
			checkDoublesAndCopyDynamicBones(from, to, bone);
		}
	}
}
