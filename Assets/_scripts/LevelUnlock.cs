using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUnlock : MonoBehaviour {
    
    public Button button;
    public PopUpController popup;
    private PlayerManager player;

    private string failedTextStart = "You need to earn $";
    private string failedTextEnd = " to unlock this level";


    public bool canUnlock;
    public bool canStart;

    public Text buttonText;
    
	// Use this for initialization
	void Start () {
        player = Initializer.instance.player;
        LevelSelect.instance.levelChanged += ChangeLevel;
	}    
	
	// Update is called once per frame
	void Update () {
        canStart = LevelSelect.instance.currentLevel.unlocked;
        buttonText.text = canStart ? "Start" : "Unlock";        
	}
    

    /// <summary>
    /// Disable the pop up window when the player pushed the level change arrow buttons
    /// </summary>
    private void ChangeLevel() {
        popup.Stop();
    }

    /// <summary>
    /// Decides what happens when the Start/Unlock button is pressed
    /// If the current level is unlocked, start the game
    /// if the current level is locked and player has money to unlock it, unlock the level
    /// if neither is true, show a pop up window that shows how much money is needed to unlock
    /// </summary>
    public void Pushed() { 
        canStart = LevelSelect.instance.currentLevel.unlocked;
        canUnlock = LevelSelect.instance.currentLevel.costToUnlock <= player.Money;
        if (canStart)
        {
            LevelManager.instance.StartGame();
        }
        else if (canUnlock)
        {
            LevelSelect.instance.UnlockLevel();
        }
        else
        {
            int moneyNeeded = LevelSelect.instance.currentLevel.costToUnlock - player.Money;
            popup.UpdateText(failedTextStart + moneyNeeded.ToString() + failedTextEnd);
            popup.ShowPopup();
        }
    }
}
