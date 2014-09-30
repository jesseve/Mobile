using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverGUIManager : MonoBehaviour {

    /*
     * 
     * This script updates the texts on the game over canvas
     * Finds the values from level manager.
     * The values themselves are updated on level managers GameOverCalculator method
     * 
     */

    //public Text coins;
    public Text combo;
    public Text money;


	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.GameOver) return;
        //coins.text = LevelManager.instance.coins.ToString();
        combo.text = LevelManager.instance.highestCombo.ToString();
        money.text = LevelManager.instance.money.ToString();
	}
}
