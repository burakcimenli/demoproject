using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour {

    public Transform fpCamTransform;
    private Transform tpCamTransform;

    public Transform player;
    private Vector3 diff;
    private bool firstPersonEnabled;

    public void Init() {
        tpCamTransform = transform;
        firstPersonEnabled = false;
        diff = transform.position - player.transform.position;
    }

    void Update() {
        if (!firstPersonEnabled) {
            transform.position = player.transform.position + diff; 
        }
    }

    public void SwitchMode(bool firstPerson) {
        if (firstPerson) {
            StartCoroutine(SwitchTransition(tpCamTransform, fpCamTransform));
        }

        else {
            StartCoroutine(SwitchTransition(fpCamTransform, tpCamTransform));
        }

        firstPersonEnabled = firstPerson;
    }

    private IEnumerator SwitchTransition(Transform a, Transform b, float duration = 1) {
        float counter = 0;

        while(counter < duration) {        
            transform.position = Vector3.Lerp(a.position, b.position, counter / duration);
            transform.rotation = Quaternion.Lerp(a.rotation, b.rotation, counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }

        // safe check
        transform.position = b.position;
        transform.rotation = b.rotation;
    }

}
