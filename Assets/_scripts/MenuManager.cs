using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public Text money;

    private PlayerManager player;

    void Start() {
        money = GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    void Update() {
        money.text = player.Money.ToString();
    }
}
