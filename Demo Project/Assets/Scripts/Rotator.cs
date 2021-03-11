using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    // 1 for right, -1 for left
    public Vector3 rotatingAxis;

    void Update() {
        transform.Rotate(rotatingAxis);
    }
}
