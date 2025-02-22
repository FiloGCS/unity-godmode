﻿//@FiloGCS 2017, all rights reserved. Don't do illegal shit with this.
using System;
using UnityEngine;

public enum BezierControlPointMode { Free, Aligned, Mirrored }

public static class Bezier {
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
}

[AddComponentMenu("Godmode/Spline")]
public class Spline : MonoBehaviour {
    
    [SerializeField]
    private Vector3[] points;
    [SerializeField]
    private BezierControlPointMode[] modes;
    [SerializeField]
    private bool loop;
    public bool Loop {
        get { return loop; }
        set {
            loop = value;
            if(value == true) {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    public int CurveCount {
        get {return (points.Length - 1) / 3;}
    }

    public int ControlPointCount {
        get {return points.Length;}
    }

    public Vector3 GetControlPoint (int index) {
        return points[index];
    }
    public void SetControlPoint (int index, Vector3 point) {
        //If we are a Main point
        if (index % 3 == 0) {
            Vector3 delta = point - points[index];
            //If the loop mode is active
            if (loop) {
                //If we're setting the first point
                if(index == 0) {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = point;
                //If we're setting the last point
                }else if(index == points.Length - 1) {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                //Otherwise the loop does not affect us
                } else {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            //If the loop mode is NOT active
            } else {
                //Move any existing adjacent points with us
                if (index > 0) {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length) {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        //Enforce mode in brother points
        EnforceMode(index);
    }

    public BezierControlPointMode GetControlPointMode(int index) {
        return modes[(index + 1) / 3];
    }
    public void SetControlPointMode(int index, BezierControlPointMode mode) {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop) {
            if (modeIndex == 0) {
                modes[modes.Length - 1] = mode;
            }else if(modeIndex == modes.Length - 1) {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }
    private void EnforceMode(int index) {
        //Get the mode for this point
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        //If the mode is free or we're in an endpoint which isn't looping there is nothing to enforce.
        if(mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex) {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0) {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if(enforcedIndex >= points.Length) {
                enforcedIndex = 1;
            }
        } else {
            fixedIndex = middleIndex + 1;
            if(fixedIndex >= points.Length) {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if(enforcedIndex < 0) {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned) {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }


    public void AddCurve() {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);

        if (loop) {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    public void Reset() {
        loop = false;
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Mirrored,
            BezierControlPointMode.Mirrored
        };
    }


    public Vector3 GetPoint(float t) {
        int i;
        if (t >= 1f) {
            t = 1f;
            i = points.Length - 4;
        } else {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i+1], points[i+2], points[i+3], t));
    }

    public Vector3 GetVelocity(float t) {
        int i;
        if (t >= 1f) {
            t = 1f;
            i = points.Length - 4;
        } else {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(
            Bezier.GetFirstDerivative(points[i], points[i+1], points[i+2], points[i+3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t) {
        return GetVelocity(t).normalized;
    }

    private void OnDrawGizmos() {
        float n = ControlPointCount * 10;
        for (int i = 0; i< n; i++) {
            Gizmos.DrawLine(GetPoint(i / n), GetPoint((i + 1) / n));
        }
    }

}
