using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelSelect : MonoBehaviour {

    public delegate void LevelChanged();
    public event LevelChanged levelChanged;
    
    public Level[] levels;                      //Array that contains every level of the game. Add more in the inspector
    public Sprite[] levelSprites;               //Contains the sprites of every level. Has to be the same lenght as levels array
    public BlockSpawner spawner;                //Reference to spawner
    private BackGroundScript background;        //Reference to background controller
    private PlayerManager player;               //Reference to player

    private int selectedLevel;                  //integer pointing to the current level and levels sprite    

    public static LevelSelect instance;         //Instance of this script

    public Level currentLevel {
        get {
            return levels[selectedLevel];
        }
    }               //Getter for the current level
    public Sprite currentSprite {
        get {
            return levelSprites[selectedLevel];
        }
    }             //Getter for the current levels sprite

    void Awake() {
        if (instance != null)
            Destroy(instance);

        instance = this;      
    }

    /// <summary>
    /// Finds all the references needed in this script
    /// </summary>
	public void Init () {
        spawner = Initializer.instance.spawner; 
        background = Initializer.instance.background;
        player = Initializer.instance.player;        
        background.ChangeBackground(currentSprite);
	}
	    
    /// <summary>
    /// Called from the arrow buttons in menu canvas
    /// and changes the selected level
    /// </summary>
    /// <param name="direction"></param>
    public void ChangeLevel(int direction) {                
        selectedLevel += direction;
        
        if (selectedLevel >= levels.Length)
            selectedLevel = 0;
        else if (selectedLevel < 0)
            selectedLevel = levels.Length - 1;
        if (levelChanged != null)
            levelChanged();
        background.ChangeBackground(currentSprite);
    }

    /// <summary>
    /// Unlocks the current level and makes it available to play
    /// WARNING! Does not check if the player has enough money!
    /// </summary>
    public void UnlockLevel() {
        currentLevel.unlocked = true;
        player.AddMoney(-currentLevel.costToUnlock);
        SoundHandler.instance.PurchaseSound();
        LevelManager.instance.Save();
    }    
}
