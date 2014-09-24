using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    /*
     *
     * This script shows the amount of money the player has
     * 
     */

    public Text money;

    private PlayerManager player;

    void Start() {
        money = GetComponent<Text>();
        player = Initializer.instance.player;
    }

    void Update() {
        money.text = player.Money.ToString();
    }

    
}
