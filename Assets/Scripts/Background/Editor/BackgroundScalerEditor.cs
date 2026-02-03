#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BackgroundAspectFilter))]
public class BackgroundScalerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BackgroundAspectFilter scaler = (BackgroundAspectFilter)target;
        if (GUILayout.Button("Resize"))
        {
            scaler.OnEditorResize();
        }
    }
}
#endif
