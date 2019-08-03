using System;
using UnityEngine;
using VRCSDK2;
using System.IO;
using UnityEditor.SceneManagement;

namespace KatStuff
{
    public class KatProfile{
    public string name;
    public int polys;
    public int boneamount;
    public int dynboneamount;
    public int dynbonecolliders;
    public int meshrenderers;
    public int particle_systems;
    public int cloth;
    public int animators;
    public int lights;
    public int audio_sources;

    public KatProfile(GameObject obj){
        initProfile(obj);
    }
    public void saveFile(){
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.dataPath + "/KATAvatars/" + name + "/" + name + ".katprofile", json);
    }
    public KatProfile fromFile(string name){
        string json = File.ReadAllText(Application.dataPath + "/KATAvatars/" + name + "/" + name + ".katprofile");
        return JsonUtility.FromJson<KatProfile>(json);
    }
    public void openScene(){
        EditorSceneManager.OpenScene("Assets/KATAvatars/" + name + "/" + name + ".unity");
    }
    public void initProfile(GameObject obj){
        this.name = obj.name;
        this.polys = 0;
        this.boneamount = 0;
        this.dynboneamount = 0;
        SkinnedMeshRenderer[] meshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer smr in meshes){
            this.polys = this.polys + smr.sharedMesh.triangles.Length/3;
            foreach(Transform bone in smr.bones){
                this.boneamount = this.boneamount + 1;
                DynamicBone dyn = bone.gameObject.GetComponent<DynamicBone>();
                if(dyn){
                    this.dynboneamount = this.dynboneamount + dyn.m_Root.GetComponentsInChildren<Transform>().Length;
                }
            }
        }
        this.meshrenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>().Length;
        this.particle_systems = obj.GetComponentsInChildren<ParticleSystem>().Length;
        this.dynbonecolliders = obj.GetComponentsInChildren<DynamicBoneCollider>().Length;
        this.cloth = obj.GetComponentsInChildren<Cloth>().Length;
        this.animators = obj.GetComponentsInChildren<Animator>().Length - 1;
        this.audio_sources = obj.GetComponentsInChildren<AudioSource>().Length;
        this.lights = obj.GetComponentsInChildren<Light>().Length;
    }

    public int performance(){
        int polysperf = rate(polys, 32000, 70000, 70000, 70000);
        int boneamountperf = rate(boneamount, 75, 150, 256, 400);
        int dynboneamountperf = rate(dynboneamount, 0, 16, 32, 256);
        int dynbonecollidersperf = rate(dynbonecolliders, 0, 0, 4, 32);
        int meshrenderersperf = rate(meshrenderers, 4, 8, 16, 32);
        int particle_systemsperf = rate(particle_systems, 0, 4, 8, 16);
        int clothperf = rate(cloth, 0, 1, 1, 1);
        int animatorsperf = rate(animators, 1, 4, 16, 32);
        int lightsperf = rate(lights, 0, 0, 0, 1);
        int audio_sourcesperf = rate(audio_sources, 1, 4, 8, 8);

        int result = 
        polysperf+
        boneamountperf+
        dynboneamountperf+
        dynbonecollidersperf+
        meshrenderersperf+
        particle_systemsperf+
        clothperf+
        animatorsperf+
        lightsperf+
        audio_sourcesperf;

        result = result / 10;
        return result;
    }
    public int rate(int i, int excellent, int good, int medium, int poor){
        if(i < excellent){
            return 0;
        }else if(i < good){
            return 1;
        }else if(i < medium){
            return 2;
        }else if(i < poor){
            return 3;
        }else{
            return 4;
        }
    }
}
}