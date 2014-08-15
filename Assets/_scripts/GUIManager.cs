using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GUIText score;
    public GUIText combo;
    public GUIText health;
    private PlayerManager player;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
        score.text = player.score.ToString();
        combo.text = player.combo.ToString();
        health.text = player.health.ToString();
	}
}
