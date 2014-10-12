using UnityEngine;
using System.Collections;

public class CameraRotating : MonoBehaviour {

    private float targetRotationLeft = 40;
    private float targetRotationRight;
    private float rotatingMargin;
    private float speed = 4f;
    private int direction = 1;
    private bool rotate;
    private bool stopRotate;
    private float rotationOnStopCall;

    private int startRotatePhase;
    private int endRotatePhase;

	// Use this for initialization
	void Start () {
        rotatingMargin = 180f;
        targetRotationRight = 360f - targetRotationRight;
        StopRotating();
        LevelManager.instance.changePhase += ChangePhase;
        LevelManager.instance.gameOver += StopRotating;
        LevelManager.instance.startGame += GetLevelValues;
        print(Camera.main.ScreenToWorldPoint(Vector3.zero) + "Left " + Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width) + "Right");
	}

    void Update() {
        if (rotate && LevelManager.instance.GetState() == State.Running)
            Rotating();
    }

    /// <summary>
    /// Stops the camera rotating
    /// </summary>
    public void StopRotating() {
        rotate = false;
        transform.eulerAngles = Vector3.zero;
    }

    private void GetLevelValues() {
        speed = LevelSelect.instance.currentLevel.cameraRotatingSpeed;
        startRotatePhase = LevelSelect.instance.currentLevel.cameraRotatingPhaseStart;
        endRotatePhase = LevelSelect.instance.currentLevel.cameraRotatingPhaseEnd;
    }

    /// <summary>
    /// Starts camera rotating
    /// </summary>
    public void ChangePhase() {
        if (speed == 0) return;
        if(LevelManager.instance.gamePhase == startRotatePhase)
            rotate = true;
        else if (LevelManager.instance.gamePhase == endRotatePhase) {
            stopRotate = true;
            rotationOnStopCall = transform.eulerAngles.z;
        }           
    }
	
    /// <summary>
    /// Rotates the camera between angles 50 and 310 through 0
    /// </summary>
    private void Rotating () {        
        if (transform.eulerAngles.z > targetRotationLeft && transform.eulerAngles.z < rotatingMargin && direction > 0)
        {
            direction = -1;
        }
        else if (direction < 0 && transform.eulerAngles.z < targetRotationRight && transform.eulerAngles.z > rotatingMargin)
        {
            direction = 1;
        }       
       
        if(stopRotate) {
            if (rotationOnStopCall < 180f != transform.eulerAngles.z < 180) {
                StopRotating();
            }
        }

        transform.eulerAngles += Vector3.forward * Time.deltaTime * speed * direction;
	}
}
