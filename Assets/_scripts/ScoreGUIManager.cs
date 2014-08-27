using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUIManager : MonoBehaviour {

    public Text score;
    public Text combo;
    public Text health;
    private PlayerManager player;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.Running) return;
        score.text = player.score.ToString();
        combo.text = player.combo.ToString();
        health.text = player.health.ToString();
	}
}
