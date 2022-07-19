//@FiloGCS 2022, all rights reserved. Don't do illegal shit with this.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathgm
{
    //Unclamped Lerp
    public static float UnclampedLerp(float a, float b, float t) {
        return t * b + (1 - t) * a;
    }
    public static Vector2 UnclampedLerp(Vector2 a, Vector2 b, float t) {
        return t * b + (1 - t) * a;
    }
    public static Vector3 UnclampedLerp(Vector3 a, Vector3 b, float t) {
        return t * b + (1 - t) * a;
    }
    public static Vector4 UnclampedLerp(Vector4 a, Vector4 b, float t) {
        return t * b + (1 - t) * a;
    }
    //Remap
    public static float Remap(float x, float a0, float b0, float a1, float b1) {
        return (x - a0) / (b0 - a0) * (b1 - a1) + a1;
    }
    public static float RemapAndClamp(float x, float a0, float b0, float a1, float b1) {
        return Mathf.Clamp(((x - a0) / (b0 - a0) * (b1 - a1) + a1), a1, b1);
    }

    //Quadratic Formula
    public static List<float> SolveQuadraticReal(float a, float b, float c) {

        List<float> solutions = new List<float>();

        float sqrtpart = b * b - 4 * a * c;
        float x, x1, x2, img;

        if (sqrtpart > 0) {
            x1 = (-b + Mathf.Sqrt(sqrtpart)) / (2 * a);
            x2 = (-b - Mathf.Sqrt(sqrtpart)) / (2 * a);
            solutions.Add(x1);
            solutions.Add(x2);
        } else if (sqrtpart < 0) {
            sqrtpart = -sqrtpart;
            x = -b / (2 * a);
            img = Mathf.Sqrt(sqrtpart) / (2 * a);
        } else {
            x = (-b + Mathf.Sqrt(sqrtpart)) / (2 * a);
            solutions.Add(x);
        }
        return solutions;
    }
    //TODO Sample Parabola
    //Parabola
    public static float GetParabolaTravelDuration(float h, float v0, float g) {
        List<float> solution = new List<float>();
        solution = SolveQuadraticReal(g, 2 * v0, -2f * h);
        return solution[0];
    }
    public static Vector2 SampleParabola(Vector2 p0, Vector2 v0, Vector2 a, float t) {
        return p0 + v0 * t + a * t * t / 2;
    }
    public static Vector3 SampleParabola(Vector3 p0, Vector3 v0, Vector3 a, float t) {
        return p0 + v0 * t + a * t * t / 2;
    }
    public static Vector3 GetParabolaInitialVelocity(Vector3 p0, Vector3 pt, Vector3 a, float t) {
        Vector3 v0 = Vector3.zero;
        v0.x = (pt.x - p0.x - (a.x * t * t) / 2.0f) / t;
        v0.y = (pt.y - p0.y - (a.y * t * t) / 2.0f) / t;
        v0.z = (pt.z - p0.z - (a.z * t * t) / 2.0f) / t;
        return v0;
    }
    public static Vector3 GetSimpleParabolaInitialVelocity(Vector3 p0, Vector3 pt, float g, float t) {
        Vector3 v0 = Vector3.zero;
        v0.x = (pt.x - p0.x) / t;
        v0.y = (pt.y - p0.y - (g * t * t) / 2.0f) / t;
        v0.z = (pt.z - p0.z) / t;
        return v0;
    }
}
