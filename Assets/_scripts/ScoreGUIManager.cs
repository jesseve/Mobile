using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUIManager : MonoBehaviour {

    /*
     *
     * Shows the players coins, combo and health on screen when the game is running
     * 
     */

    public Text coins;
    public Text combo;
    public Text health;
    private PlayerManager player;    


	// Use this for initialization
	void Start () {
        player = Initializer.instance.player;        
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.Running) return;
        coins.text = player.coins.ToString();
        combo.text = player.combo.ToString();
        health.text = player.health.ToString();
	}
    
}
