using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private Slider slider;
    private PlayerManager player;

    public Image healthBar;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = player.health - 1;
	}

    public void ChangeColor() {
        healthBar.color = new Color(1f - (slider.value / slider.maxValue), (slider.value / slider.maxValue), 0);
    }

    public void Init() {        
        slider.maxValue = player.maxHits - 1;
        slider.value = slider.maxValue;
        ChangeColor();
    }
}
