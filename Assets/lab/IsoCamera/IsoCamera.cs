//@FiloGCS 2018, all rights reserved. Don't do illegal shit with this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Godmode/Iso Camera")]
public class IsoCamera : MonoBehaviour {

    public float LERP_SPEED = 10;
    private Vector3 targetPosition;
    private Vector3 cameraOffset;

    //Start
    void Start() {
        cameraOffset = this.transform.localPosition;
        targetPosition = this.transform.localPosition;
    }


    //Update
    void Update() {

    }
}
