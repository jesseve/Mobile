using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

    /*
     *
     * Handles the init methods of every game object in the scene
     * Also has the references to most used scripts
     * like player manager and blockspawner 
     * 
     */


    public static Initializer instance;

    public PlayerManager player;
    public BlockSpawner spawner;
    public BackGroundScript background;
    public HealthBar healthBar;
    public HintHandler hints;


    void Awake() {
        if (instance != null)
            Destroy(instance);

        instance = this;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if (spawner == null)
            spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        if (background == null)
            background = GameObject.FindGameObjectWithTag("Background").GetComponent<BackGroundScript>();
        if (healthBar == null)
            healthBar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<HealthBar>();
        if (hints == null)
            hints = GameObject.FindGameObjectWithTag("Hints").GetComponent<HintHandler>();
    }

    void Start() {        
        SaveLoad.Load();
        LevelManager.instance.Init();
        LevelSelect.instance.Init();
        spawner.Init();
        player.InitPlayer();
        healthBar.Init();
        
        
    }


    
}
