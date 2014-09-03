using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUnlock : MonoBehaviour {

    public Text text;
    public Button button;
    private PlayerManager player;

    private string textStart = "It will cost $";
    private string textEnd = " to unlock. Unlock?";

    private string failedTextStart = "You need to earn $";
    private string failedTextEnd = " to unlock this level";

    public GameObject OKButton;
    public GameObject YesButton;
    public GameObject NoButton;

    private bool canUnlock;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
	}    
	
	// Update is called once per frame
	void Update () {
        SetInteractable();
        if (canUnlock)
        {
            text.text = textStart + LevelSelect.instance.currentLevel.costToUnlock.ToString() + " " + textEnd;
            YesButton.SetActive(true);
            NoButton.SetActive(true);
            OKButton.SetActive(false);
        }
        else
        {
            int moneyNeeded = LevelSelect.instance.currentLevel.costToUnlock - player.Money;
            text.text = failedTextStart + moneyNeeded.ToString() + failedTextEnd;
            YesButton.SetActive(false);
            NoButton.SetActive(false);
            OKButton.SetActive(true);
        }
	}

    public void SetInteractable()
    {
        canUnlock = LevelSelect.instance.currentLevel.costToUnlock <= player.Money;
    }
}
