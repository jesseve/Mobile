using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverGUIManager : MonoBehaviour {

    public Text score;
    public Text combo;
    public Text money;


	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.GameOver) return;
        print("GameOver");
        score.text = LevelManager.instance.score.ToString();
        combo.text = LevelManager.instance.highestCombo.ToString();
        money.text = LevelManager.instance.money.ToString();
	}
}
