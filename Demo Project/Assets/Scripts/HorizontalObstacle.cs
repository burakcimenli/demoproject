using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorizontalObstacle : MonoBehaviour {

    public float horizontalAnimTime = 2;
    public bool startFromRight;

    void Start() {
        StartCoroutine(HorizontalAnimation());
    }

    IEnumerator HorizontalAnimation() {
        int sideFactor = startFromRight ? 1 : -1;
        Vector3 a = new Vector3(5 * sideFactor, transform.position.y, transform.position.z);
        Vector3 b = new Vector3(-5 * sideFactor, transform.position.y, transform.position.z);

        float counter = 0;        
        do {
            transform.position = Vector3.Lerp(a, b, counter / horizontalAnimTime);
            counter += Time.deltaTime;
            yield return null;
        } while (counter <= horizontalAnimTime);

        do {
            transform.position = Vector3.Lerp(a, b, counter / horizontalAnimTime);
            counter -= Time.deltaTime;
            yield return null;
        } while (counter >= 0);

        StartCoroutine(HorizontalAnimation());
    }
}
