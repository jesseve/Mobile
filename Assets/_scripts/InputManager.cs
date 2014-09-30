using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

    public GUIHandler gui;
    private PlayerManager manager;
	
    // Use this for initialization
	void Start () {
        manager = GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {

        //Decide what happens when player presses the back/escape button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            State s = LevelManager.instance.GetState();            
            switch(s){
                case State.Running:
                    if (Initializer.instance.hints.HintsEnabled)
                        Initializer.instance.hints.Stop();
                    gui.Pause();
                    break;
                case State.Pause:
                    if (gui.IsConfirming())
                        gui.Confirm(true);
                    else
                        gui.Confirm();
                    break;
                case State.Menu:
                    if (gui.IsConfirming())
                        gui.Confirm(true);
                    else
                        gui.Confirm();
                    break;                
                case State.Shop:
                    gui.Menu();
                    break;

            }
        }
        
        //If the hints are on the screen and player touches the screen, disable them
        if (Input.GetMouseButton(0) && Initializer.instance.hints.HintsEnabled) {
            Initializer.instance.hints.Stop();
        }

        //Detect input only when the game is running
        if (LevelManager.instance.GetState() != State.Running) return;

        int direction = 0;
        
        //Decide whether the player moves to left or right        
        if (Input.GetMouseButton(0)) {
            direction = Input.mousePosition.x >= Screen.width * 0.5f ? 1 : -1;            
        }

        manager.MoveHorizontally(direction);
	}
}
