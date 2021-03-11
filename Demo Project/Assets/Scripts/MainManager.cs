using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {

    public Text countdownText;
    public PlayerController playerController;
    public PaintableWall wall;
    public CameraTracker camTracker;

    void Start() {
        Init();
    }

    // First init
    void Init() {
        // Set up the camera
        camTracker.Init();

        // Start countdown
        StartCoroutine(StartCountdown());

        // Init paintable wall
        wall.Init();
    }

    void InitAfterCountdown() {
        // Init player controls
        playerController.Init(wall, camTracker);

        // Init bots
        // The AI I programmed became self aware and ran from the simulation :/
    }

    private IEnumerator StartCountdown() {
        for(int i = 3; i > 0; i--) {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        countdownText.text = "0";
        InitAfterCountdown();

        yield return new WaitForSeconds(1);
        countdownText.text = "";
    }
}
