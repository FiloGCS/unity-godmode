using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public Spline spline;
    public bool lookForward = true;
    public float duration;
    public float progress;
    private void Update() {
        progress += Time.deltaTime / duration;
        if(progress > 1f) {
            progress -= Mathf.Floor(progress);
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward) {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}
