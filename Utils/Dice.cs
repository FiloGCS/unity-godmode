using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice<T> : MonoBehaviour
{
    [System.Serializable]
    public struct DiceFace
    {
        public T value;
        public Vector3 normal;
        public DiceFace(T value, Vector3 normal) {
            this.value = value;
            this.normal = normal;
        }
    }

    public List<DiceFace> faces;

    public T GetValue() {
        T result = default;
        float closestDot = -1f;
        foreach (DiceFace face in faces) {
            float dot = Vector3.Dot(transform.TransformVector(face.normal), Vector3.up);
            if (dot > closestDot) {
                result = face.value;
                closestDot = dot;
            }
        }
        return result;
    }
    public T GetValue(Vector3 up) {
        T result = default;
        float closestDot = -1f;
        foreach (DiceFace face in faces) {
            float dot = Vector3.Dot(transform.TransformVector(face.normal), up);
            if (dot > closestDot) {
                result = face.value;
                closestDot = dot;
            }
        }
        return result;
    }
}
