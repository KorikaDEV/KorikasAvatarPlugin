using UnityEngine;
using System.Collections;
using UnityEditor;
using VRCSDK2;
using KATStuff;

[CustomEditor(typeof(ViewpointSetter))]
public class ViewpointSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ViewpointSetter script = (ViewpointSetter)target;
        if(GUILayout.Button("set viewpoint"))
        {
            VRC_AvatarDescriptor vrcad = GestureDisplay.getVRCSceneAvatar().GetComponent<VRC_AvatarDescriptor>();
            vrcad.ViewPosition = script.transform.position;
            DestroyImmediate(script.gameObject);
        }
    }
}