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
    public Text score;
    private Animator scoreAnimator;
    private PlayerManager player;    


	// Use this for initialization
	void Start () {
        player = Initializer.instance.player;
        player.scored += ScoreSplash;
        scoreAnimator = score.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.Running) return;
        coins.text = player.coins.ToString();
        combo.text = player.combo.ToString();        
	}

    /// <summary>
    /// Shows the amount of coins got on pickup
    /// </summary>
    private void ScoreSplash() {
        score.text = "+ " + (player.combo * player.moneyToAdd).ToString();
        int anim = Random.Range(1, 3);
        if (player.combo % 10 != 0)
            scoreAnimator.Play("scoresplash" + anim.ToString());
        else
            scoreAnimator.Play("scoresplash3");
    }
    
}
