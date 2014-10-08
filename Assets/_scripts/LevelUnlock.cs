using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUnlock : MonoBehaviour {

    public Sprite buttonImageStart;
    public Sprite buttonImageUnlock;
    public PopUpController popup;
    private PlayerManager player;

    private string failedTextStart = "You need D";
    private string failedTextEnd = " more to unlock this level";

    private Image buttonImage;

    public bool canUnlock;
    public bool canStart;

    public Text unlockCostText;
    public Image unlockDollarImage;
    
	// Use this for initialization
	void Start () {
        player = Initializer.instance.player;
        LevelSelect.instance.levelChanged += ChangeLevel;
        buttonImage = GetComponent<Image>();
	}    
	
	// Update is called once per frame
	void Update () {
        canStart = LevelSelect.instance.currentLevel.unlocked;        
        buttonImage.sprite = canStart ? buttonImageStart : buttonImageUnlock;
        SetUnlockCost();
	}

    /// <summary>
    /// Updates the text showing the cost to unlock
    /// and disables the dollar image if unlocked
    /// </summary>
    private void SetUnlockCost() {
        unlockDollarImage.enabled = !canStart;
        if(!canStart) {
            unlockCostText.text = LevelSelect.instance.currentLevel.costToUnlock.ToString();            
        }
        else {
            unlockCostText.text = "";
        }
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
