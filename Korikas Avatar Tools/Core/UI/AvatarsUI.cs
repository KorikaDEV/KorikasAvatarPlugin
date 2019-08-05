using UnityEngine;
using UnityEditor;
public class AvatarsWindow : EditorWindow
{
    [MenuItem("Korikas Avatar Tools/Avatars")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<AvatarsWindow>("KATAvatars");
        window.minSize = new Vector2(265, 265);
    }
    void OnGUI()
    {
    }
}