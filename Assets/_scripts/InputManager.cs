using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {


    private PlayerManager manager;
	
    // Use this for initialization
	void Start () {
        manager = GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            LevelManager.instance.Pause();

        if (LevelManager.instance.GetState() != State.Running) return;

        int direction = 0;
        
        if (Input.GetMouseButton(0)) {
            direction = Input.mousePosition.x >= Screen.width * 0.5f ? 1 : -1;            
        }

        manager.MoveHorizontally(direction);
	}
}
