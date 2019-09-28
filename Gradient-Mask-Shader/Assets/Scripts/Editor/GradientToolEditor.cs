using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GradientTool))]
public class GradientToolEditor : Editor
{

    GradientTool gradientTool;
    float size = 0.1f;
    protected virtual void OnSceneGUI()
    {
        gradientTool = (GradientTool)target;

        EditorGUI.BeginChangeCheck();

        Handles.color = Color.white;
        Handles.RectangleHandleCap(0, gradientTool.EndPt, Camera.current.transform.rotation, HandleUtility.GetHandleSize(gradientTool.StartPt) * size * 1.2f, EventType.Repaint);
        Vector3 newStartPtPos = Handles.FreeMoveHandle(gradientTool.StartPt, Quaternion.identity, HandleUtility.GetHandleSize(gradientTool.StartPt) * size, Vector3.zero, Handles.DotHandleCap);
        Handles.color = Color.black;
        Handles.RectangleHandleCap(0, gradientTool.StartPt, Camera.current.transform.rotation, HandleUtility.GetHandleSize(gradientTool.StartPt) * size * 1.2f, EventType.Repaint);
        Vector3 newEndPtPos = Handles.FreeMoveHandle(gradientTool.EndPt, Quaternion.identity, HandleUtility.GetHandleSize(gradientTool.StartPt) * size, Vector3.zero, Handles.DotHandleCap);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(gradientTool, "Change Gradient Start Point Position");
            gradientTool.StartPt = newStartPtPos;

            Undo.RecordObject(gradientTool, "Change Gradient End Point Position");
            gradientTool.EndPt = newEndPtPos;
        }
    }

    public override void OnInspectorGUI()
    {
        GUIContent headerContent = new GUIContent("I AM HERE!");
        
        if (GUILayout.Button("Reset"))
        {
            gradientTool.Reset();
        }
    }

}

