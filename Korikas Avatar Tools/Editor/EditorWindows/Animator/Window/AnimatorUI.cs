using UnityEngine;
using UnityEditor;
public class AnimatorsWindow : EditorWindow {
	[MenuItem("Korikas Avatar Tools/Animator")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<AnimatorsWindow>("KATAnimator");
        window.minSize = new Vector2(265, 265);
    }
}