using UnityEngine;
using UnityEngine.SceneManagement;
using KATStuff;

[UnityEditor.InitializeOnLoad]
static class KatProfiler
{
    static KatProfiler()
    {
        UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnSceneSaved;
    }

    static void OnSceneSaved(Scene scene, string s)
    {
        if(GestureDisplay.getVRCSceneAvatar() != null){
            KatProfile kp = new KatProfile(GestureDisplay.getVRCSceneAvatar());
            kp.saveFile();
        }else{
            Debug.LogWarning("your avatar is hidden! you might make him visible again, so that KAT can save his performance statistics...");
        }
    }
}
