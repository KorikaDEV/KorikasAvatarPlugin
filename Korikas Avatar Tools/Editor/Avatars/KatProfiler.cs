using UnityEngine;
using UnityEngine.SceneManagement;
using KatStuff;

    [UnityEditor.InitializeOnLoad]
    static class KatProfiler
    {
        static KatProfiler()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnSceneSaved;
        }
     
        static void OnSceneSaved(Scene scene, string s)
        {
            KatProfile kp = new KatProfile(GestureDisplay.getVRCSceneAvatar());
            kp.saveFile();
        }
    }
