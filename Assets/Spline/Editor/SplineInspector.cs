using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor {

    private const int lineSteps = 10;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        new Color(1.0f,0.5f,0f)
    };

    private Spline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    float previewT = 0f;
    bool showPreview = false;

    public bool showStress = false;
    float minStress = Mathf.Infinity;
    float maxStress = 0.0f;
    float stressTolerance = 3f;

    bool showTangents = false;

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
            
            p0 = p3;
        }

        UpdateStress();
        float n = spline.ControlPointCount * 10;
        for (int i = 0; i < n; i++) {
            float v = spline.GetVelocity(i / n).magnitude;
            if (showStress) {
                Handles.color = Color.Lerp(Color.green, Color.red, Mathf.Clamp01(v/minStress * 1/stressTolerance));
            } else {
                Handles.color = Color.white;
            }
            Handles.DrawLine(spline.GetPoint(i / n), spline.GetPoint((i + 1) / n));
        }
        if (showTangents) {
            ShowDirections();
        }

        if (showPreview) {
            Vector3 point = spline.GetPoint(previewT);
            float size = HandleUtility.GetHandleSize(point);
            Handles.color = Color.red;
            Handles.DotHandleCap(0, point, Quaternion.identity,size*0.1f, EventType.Repaint);
        }
    }
    
    public void UpdateStress() {
        float n = spline.ControlPointCount * 10;
        if (showStress) {
            for (int i = 0; i < n; i++) {
                float v = spline.GetVelocity(i / n).magnitude;
                if (v < minStress) {
                    minStress = v;
                }
                if (v > maxStress) {
                    maxStress = v;
                }
            }
        }
    }

    bool showControls = true;
    public override void OnInspectorGUI() {
        spline = target as Spline;

        // VISUALIZING OPTIONS
        //Preview position
        EditorGUI.BeginChangeCheck();
        showPreview = GUILayout.Toggle(showPreview, "Preview value");
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(spline);
        }
        if (showPreview) {
            EditorGUI.BeginChangeCheck();
            previewT = EditorGUILayout.Slider(previewT, 0, 1);
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(spline);
            }
        }
        //Stress
        EditorGUI.BeginChangeCheck();
        showStress = GUILayout.Toggle(showStress, "Show Velocity");
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(spline);
        }
        if (showStress) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Max Velocity Ratio","Points with velocity this times faster than the lowest velocity in the spline will be coloured red"));
            EditorGUI.BeginChangeCheck();
            stressTolerance = EditorGUILayout.Slider(stressTolerance, 1, 10);
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(spline);
            }
            EditorGUILayout.EndHorizontal();
        }
        //Tangents
        EditorGUI.BeginChangeCheck();
        showTangents = GUILayout.Toggle(showTangents, "Show Tangents");
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(spline);
        }
        if (showTangents) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tangents per segment");
            EditorGUI.BeginChangeCheck();
            showTangentsDensity = EditorGUILayout.IntSlider(showTangentsDensity, 1, 25);
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(spline);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tangent Lenght");
            EditorGUI.BeginChangeCheck();
            showTangentsLength = EditorGUILayout.FloatField(showTangentsLength);
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(spline);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Force Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }
        EditorGUILayout.Separator();
        for (int i=0; i<spline.ControlPointCount; i+=3) {
            GUIStyle style = new GUIStyle();
            if(i == selectedIndex || i == selectedIndex+1 || i == selectedIndex-1) {
                GUI.backgroundColor = Color.yellow;
            } else {
                GUI.backgroundColor = Color.white;
            }
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            // Point Label
            GUILayout.Label("Point " + i/3, EditorStyles.boldLabel);
            // Point Mode
            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup(spline.GetControlPointMode(i));
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Change Point Mode");
                spline.SetControlPointMode(i, mode);
                EditorUtility.SetDirty(spline);
            }
            GUILayout.EndHorizontal();
            // Point Position
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(i));
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(i, point);
            }
            //Point controllers
            if (i > 0) {
                EditorGUI.BeginChangeCheck();
                Vector3 handler1 = EditorGUILayout.Vector3Field("Arriving Tangent", spline.GetControlPoint(i - 1));
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Move Handler");
                    EditorUtility.SetDirty(spline);
                    spline.SetControlPoint(i - 1, handler1);
                }
            }
            if (i < spline.ControlPointCount - 1) {
                EditorGUI.BeginChangeCheck();
                Vector3 handler2 = EditorGUILayout.Vector3Field("Leaving Tangent", spline.GetControlPoint(i + 1));
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Move Handler");
                    EditorUtility.SetDirty(spline);
                    spline.SetControlPoint(i + 1, handler2);
                }
            }

            GUI.backgroundColor = Color.white;
            GUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Segment")) {
            Undo.RecordObject(spline, "Add Spline Segment");
            EditorUtility.SetDirty(spline);
            spline.AddCurve();
        }
        if (GUILayout.Button("Remove Segment")) {
            Undo.RecordObject(spline, "Remove Spline Segment");
            EditorUtility.SetDirty(spline);
            //spline.RemoveCurve();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        if (GUILayout.Button("Reset Spline")) {
            Undo.RecordObject(spline, "Reset Spline");
            EditorUtility.SetDirty(spline);
            spline.Reset();
        }

    }


    private int showTangentsDensity = 3;
    private float showTangentsLength = 0.5f;
    private void ShowDirections() {
        Handles.color = Color.cyan;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * showTangentsLength);
        int steps = showTangentsDensity * spline.CurveCount;
        for (int i = 1; i <= steps; i++) {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * showTangentsLength);
        }
    }

    private const float handleSize = 0.05f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;

    private Vector3 ShowPoint(int index) {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index % 3 == 0) {
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
