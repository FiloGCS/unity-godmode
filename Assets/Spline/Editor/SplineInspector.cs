using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor {

    private const int lineSteps = 10;
    private const float directionScale = 0.5f;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        new Color(1.0f,0.5f,0f)
    };

    private Spline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI() {
        spline = target as Spline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for(int i = 1; i<spline.ControlPointCount; i+= 3) {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        //ShowDirections();
    }

    public override void OnInspectorGUI() {
        spline = target as Spline;
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }
        if(selectedIndex >= 0 && selectedIndex < spline.ControlPointCount) {
            DrawSelectedPointInspector();
        }
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Segment")) {
            Undo.RecordObject(spline, "Add Spline Segment");
            EditorUtility.SetDirty(spline);
            spline.AddCurve();
        }
        if(GUILayout.Button("Reset Spline")) {
            Undo.RecordObject(spline, "Reset Spline");
            EditorUtility.SetDirty(spline);
            spline.Reset();

        }
        GUILayout.EndHorizontal();
    }

    private void DrawSelectedPointInspector() {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }


    private const int stepsPerCurve = 3;
    private void ShowDirections() {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++) {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private const float handleSize = 0.05f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;

    private Vector3 ShowPoint(int index) {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0) {
            Handles.color = Color.green;
        } else if (index % 3 == 0) {
            Handles.color = Color.white;
            Handles.ArrowHandleCap(0, point, Quaternion.identity, 1f, EventType.Layout);
        } else {
            Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        }
        float s = index % 3 == 0 ? 1f : 0.65f;
        if (Handles.Button(point, handleRotation, s* size * handleSize, s* size * pickSize, Handles.DotHandleCap)) {
            selectedIndex = index;
            Repaint();
        }
        if(selectedIndex == index) {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index,handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
    
}
