using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private Slider slider;
    private PlayerManager player;

    public Image healthBar;

	// Use this for initialization
	void Awake () {
        slider = GetComponent<Slider>();
        
        //Init();
	}
	
	// Update is called once per frame
	void Update () {        
        slider.value = player.health;
	}    

    public void Init() {
        player = Initializer.instance.player;
        ResetValues();
    }

    /// <summary>
    /// Changes sliders color depending on players health
    /// Color changes from green to red
    /// </summary>
    public void ChangeColor() {
        healthBar.color = new Color((1f - (slider.value / slider.maxValue)), (slider.value / slider.maxValue), 0);
    }

    /// <summary>
    /// Finds the max value from player and sets the sliders value to max
    /// </summary>
    public void ResetValues() {        
        slider.maxValue = player.maxHits;
        slider.value = slider.maxValue;
        ChangeColor();
    }
}
