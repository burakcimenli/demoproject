using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Animator playerAnimator;
    public Rigidbody playerRB;
    public Collider playerCol;
    public float maxSpeed;

    private bool playerControllerEnabled;
    private bool paintingConrollerEnabled;
    private PaintableWall paintableWall;
    private CameraTracker camTracker;

    // Paint percentage related fields
    private float lastUpdateTime = 0;
    private float percentageUpdateInterval = .1f;

    void Start() {
        SetRagdollState(false);
    }

    public void Init(PaintableWall paintableWall, CameraTracker camTracker) {
        playerControllerEnabled = true;
        paintingConrollerEnabled = false;

        this.paintableWall = paintableWall;
        this.camTracker = camTracker;
    }

    void Update() {
        if (playerControllerEnabled) {
            CheckInputForPlayerController(); 

            if(transform.position.y < -6) {
                Fall();
            }
        }

        if (paintingConrollerEnabled) {
            CheckInputForPaintingController();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    // Player controller
    private void CheckInputForPlayerController() {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePos = GetMousePosition();
            Vector3 moveDirection = (mousePos - transform.position).normalized;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            transform.forward = moveDirection * Time.deltaTime;
            transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime);
            
            // Safe checking
            if(playerAnimator.GetBool("run") == false) {
                playerAnimator.SetBool("run", true);
            }

            if (Input.GetMouseButtonDown(0))
                playerAnimator.SetBool("run", true);
        }

        if (Input.GetMouseButtonUp(0))
            playerAnimator.SetBool("run", false);
    }

    // Painting controller
    private void CheckInputForPaintingController() {
        if (Input.GetMouseButton(0)) {
            if (Input.GetMouseButtonDown(0)) {
                // First click
                paintableWall.brush.SetActive(true);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Wall") {
                    paintableWall.brush.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.1f);
                }
            }

            // Show how much of the wall is painted
            if (Time.time > lastUpdateTime + percentageUpdateInterval) {
                paintableWall.UpdatePercentageText();
                lastUpdateTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            paintableWall.brush.SetActive(false);
        }
    }

    Vector3 GetMousePosition() {
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance)) {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    private void ResetPlayer() {
        transform.forward = Vector3.forward;
        playerAnimator.SetBool("run", false);
        transform.position = Vector3.zero;
        playerControllerEnabled = true;
        SetRagdollState(false);
        playerAnimator.enabled = true;
    }

    private void Fall() {
        playerAnimator.enabled = false;
        SetRagdollState(true);
        playerControllerEnabled = false;

        StartCoroutine(ResetTimer(2));
    }

    private IEnumerator ResetTimer(float sec) {
        yield return new WaitForSeconds(sec);
        ResetPlayer();
    }

    private void SetRagdollState(bool state) {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = !state;
        }
        playerRB.isKinematic = state;

        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols) {
            col.enabled = state;
        }
        playerCol.enabled = !state;
    }


    // COLLISIONS

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "RotatingStick") {
            playerRB.velocity += transform.forward * 1000;
        }

        if(other.transform.tag == "FinishTrigger") {
            camTracker.SwitchMode(true);
            paintableWall.ActivatePercentageText(true);

            paintingConrollerEnabled = true;
            playerControllerEnabled = false;
            playerAnimator.SetBool("run", false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.tag == "Obstacle") {
            Fall();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.tag == "RotatingPlatformRight") {
            playerRB.AddForce(Vector3.right * 20);
        }
        else if (other.transform.tag == "RotatingPlatformLeft") {
            playerRB.AddForce(Vector3.left * 20);
        }

        playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, 8);
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.tag == "RotatingPlatformRight" || other.transform.tag == "RotatingPlatformLeft")
            playerRB.velocity = Vector3.zero;
    }
}
