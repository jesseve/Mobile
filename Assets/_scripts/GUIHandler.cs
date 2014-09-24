using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIHandler : MonoBehaviour {

    //References to all the canvases in the scene
    public Canvas scoreCanvas;
    public Canvas menuCanvas;
    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas shopCanvas;
    public Canvas confirmCanvas;

    public HintHandler hints;

    void Awake() {
        SetGUI(menuCanvas);
    }

    /// <summary>
    /// Sets score canvas
    /// and shows the hints on the screen.
    /// </summary>
    public void StartGame() {
        SetGUI(scoreCanvas);
        hints.StartHint();
    }

    /// <summary>
    /// Pauses the game and shows pause canvas
    /// </summary>
    public void Pause() {
        LevelManager.instance.Pause();
        SetGUI(pauseCanvas);
    }
    
    /// <summary>
    /// Continues the game and returns the score canvas
    /// </summary>
    public void UnPause() {
        LevelManager.instance.Pause();
        SetGUI(scoreCanvas);
    }

    /// <summary>
    /// Depending on state of the game
    /// In pause menu calls the quit game
    /// and in main menu quit application method
    /// Called from confirmation windows yes and no buttons
    /// </summary>
    /// <param name="quit">Did the player press yes or no</param>
    public void Confirm(bool quit) {
        switch (LevelManager.instance.GetState()) { 
            case State.Menu:
                QuitApplication(quit);
                break;
            case State.Pause: case State.Running:
                QuitGame(quit);
                break;
        }
    }

    /// <summary>
    /// Quits the game to main menu or shows the pause menu depending on the button pressed on the confirmation window
    /// </summary>
    /// <param name="quit"></param>
    public void QuitGame(bool quit) {
        if (quit) {
            LevelManager.instance.GameOver();
            SetGUI(menuCanvas);
        }
        else
            SetGUI(pauseCanvas);
    }
    
    /// <summary>
    /// Quit the application or show the menu canvas depending on the button pressed on the confirmation window
    /// </summary>
    /// <param name="quit"></param>
    public void QuitApplication(bool quit) {
        if (quit)
            LevelManager.instance.Quit();
        else
            SetGUI(menuCanvas);
    }
    
    /// <summary>
    /// Enables the confrimation window
    /// </summary>
    public void Confirm() {
        SetGUI(confirmCanvas);
    }
    
    /// <summary>
    /// Enables the game over canvas
    /// </summary>
    public void GameOver() {
        SetGUI(gameOverCanvas);
    }
    
    /// <summary>
    /// Enables the main menu
    /// </summary>
    public void Menu() {
        SetGUI(menuCanvas);
    }
    
    /// <summary>
    /// Enables the shop
    /// </summary>
    public void Shop() {
        SetGUI(shopCanvas);
    }

    /// <summary>
    /// Enables the canvas given as a parameter and disables the rest
    /// </summary>
    /// <param name="canvas"></param>
    public void SetGUI(Canvas canvas)
    {
        scoreCanvas.enabled = false;
        menuCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        pauseCanvas.enabled = false;
        shopCanvas.enabled = false;
        confirmCanvas.enabled = false;
        canvas.enabled = true;
    }
}
