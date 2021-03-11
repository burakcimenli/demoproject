using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class HorizontalObstacle : MonoBehaviour {

    public float horizontalAnimTime = 2;
    public bool startFromRight;

    [HideInInspector]
    public bool goingRight;

    private NavMeshObstacle obstacle;

    void Start() {
        obstacle = GetComponent<NavMeshObstacle>();
        StartCoroutine(HorizontalAnimation());
        goingRight = !startFromRight;
    }

    IEnumerator HorizontalAnimation() {
        int sideFactor = startFromRight ? 1 : -1;
        Vector3 a = new Vector3(5 * sideFactor, transform.position.y, transform.position.z);
        Vector3 b = new Vector3(-5 * sideFactor, transform.position.y, transform.position.z);

        SwitchNMCenter(sideFactor > 0);

        float counter = 0;        
        do {
            transform.position = Vector3.Lerp(a, b, counter / horizontalAnimTime);
            counter += Time.deltaTime;
            yield return null;
        } while (counter <= horizontalAnimTime);

        SwitchNMCenter(sideFactor < 0);
        goingRight = !goingRight;

        do {
            transform.position = Vector3.Lerp(a, b, counter / horizontalAnimTime);
            counter -= Time.deltaTime;
            yield return null;
        } while (counter >= 0);

        goingRight = !goingRight;
        StartCoroutine(HorizontalAnimation());
    }

    // Modify navmesh center for this obstacle
    private void SwitchNMCenter(bool goingLeft) {
        obstacle.center = new Vector3(goingLeft ? -1 : 1, 0, 0);
    }

}
