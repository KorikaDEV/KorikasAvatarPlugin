using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace KatStuff
{
public class PerformanceProfile {
	public int polysperf;
	public int boneamountperf;
	public int dynboneamountperf;
	public int dynbonecollidersperf;
	public int meshrenderersperf;
	public int particle_systemsperf;
	public int clothperf;
	public int animatorsperf;
	public int lightsperf;
	public int audio_sourcesperf;
	
	public PerformanceProfile(KatProfile kp){
		polysperf = rate(kp.polys, 32000, 70000, 70000, 70000);
        boneamountperf = rate(kp.boneamount, 75, 150, 256, 400);
        dynboneamountperf = rate(kp.dynboneamount, 0, 16, 32, 256);
        dynbonecollidersperf = rate(kp.dynbonecolliders, 0, 0, 4, 32);
        meshrenderersperf = rate(kp.meshrenderers, 4, 8, 16, 32);
        particle_systemsperf = rate(kp.particle_systems, 0, 4, 8, 16);
        clothperf = rate(kp.cloth, 0, 1, 1, 1);
        animatorsperf = rate(kp.animators, 1, 4, 16, 32);
        lightsperf = rate(kp.lights, 0, 0, 0, 1);
        audio_sourcesperf = rate(kp.audio_sources, 1, 4, 8, 8);
	}
	
	public float performance(){
        float result = 
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
	public static string doubleToPerfName(double d){
		if(d <= 0){
            return "excellent";
        }else if(d <= 1){
            return "good";
        }else if(d <= 2){
            return "medium";
        }else if(d <= 3){
            return "poor";
        }else{
            return "unoptimized";
        }
    }
    public static Color doubleToPerfColor(double d){
        int i = (int) d;
        switch(i){
            case 0:
            return MainUI.fromRGB(177, 255, 140);
            case 1:
            return MainUI.fromRGB(207, 255, 140);
            case 2:
            return MainUI.fromRGB(255, 253, 140);
            case 3:
            return MainUI.fromRGB(255, 203, 140);
            case 4:
            return MainUI.fromRGB(255, 155, 140);
        }
        return MainUI.fromRGB(255, 155, 140);
    }
	public int rate(int i, int excellent, int good, int medium, int poor){
        if(i <= excellent){
            return 0;
        }else if(i <= good){
            return 1;
        }else if(i <= medium){
            return 2;
        }else if(i <= poor){
            return 3;
        }else{
            return 4;
        }
    }
	public void saveFile(string path){
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(path + "performanceProfile.performance", json);
    }
    public static PerformanceProfile fromFile(string path){
        string json = File.ReadAllText(path + "performanceProfile.performance");
        return JsonUtility.FromJson<PerformanceProfile>(json);
    }
}
}
