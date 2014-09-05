using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

    public Button quit;

    private PlayerManager manager;
	
    // Use this for initialization
	void Start () {
        manager = GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            State s = LevelManager.instance.GetState();
            switch(s){
                case State.Running: case State.Pause:
                    LevelManager.instance.Pause();
                    break;
                case State.Menu: case State.Confirm:
                    LevelManager.instance.ConfirmQuit();
                    break;


            }
        }

        if (LevelManager.instance.GetState() != State.Running) return;

        int direction = 0;
        
        if (Input.GetMouseButton(0)) {
            direction = Input.mousePosition.x >= Screen.width * 0.5f ? 1 : -1;            
            //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x ? 1 : -1;
        }

        manager.MoveHorizontally(direction);
	}
}
