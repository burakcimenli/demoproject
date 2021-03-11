using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutObstacle : MonoBehaviour {

    public Transform movingStick;
    private float pressAnimationTime = 1;
    private float nextPress = 2;

    void Update() {
        if(Time.time > nextPress) {
            Press();
            nextPress += Random.Range(2, 5);
        }
    }

    private void Press() {
        StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation() {
        float counter = 0;

        while(counter < pressAnimationTime / 2) {
            // from 3, 0, 0 to 0.75
            movingStick.localPosition = Vector3.Lerp(Vector3.right * 3, Vector3.right / 1.33f, counter / (pressAnimationTime / 2));
            counter += Time.deltaTime;
            yield return null;
        }

        while(counter > 0) {
            movingStick.localPosition = Vector3.Lerp(Vector3.right * 3, Vector3.right / 1.33f, counter / (pressAnimationTime / 2));
            counter -= Time.deltaTime;
            yield return null;
        }
    }

}
