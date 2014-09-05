using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelSelect : MonoBehaviour {

    public Level[] levels;

    public BlockSpawner spawner;
    private BackGroundScript background;
    private PlayerManager player;

    private int selectedLevel;

    public GameObject StartGame;
    public GameObject Unlock;


    public static LevelSelect instance;

    public Level currentLevel {
        get {
            return levels[selectedLevel];
        }
    }

	// Use this for initialization
	void Awake () {
        if (instance != null)
            Destroy(instance);

        instance = this;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<BackGroundScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();        
        SetGUI();
	}

    void Start() {
        background.ChangeBackground(currentLevel.levelSprite);
    }
	
    public void InitLevel() { }

    public void ChangeLevel(int direction) {
        selectedLevel += direction;
        if (selectedLevel >= levels.Length)
            selectedLevel = 0;
        else if (selectedLevel < 0)
            selectedLevel = levels.Length - 1;
        background.ChangeBackground(currentLevel.levelSprite);
        SetGUI();
    }

    public void UnlockLevel() {
        currentLevel.unlocked = true;
        player.AddMoney(-currentLevel.costToUnlock);
        SetGUI();
    }

    private void SetGUI()
    {
        if (currentLevel.unlocked)
        {
            StartGame.SetActive(true);
            Unlock.SetActive(false);
        }
        else
        {
            Unlock.SetActive(true);
            StartGame.SetActive(false);
        }
    }
}
