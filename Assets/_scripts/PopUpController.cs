using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpController : MonoBehaviour {

    public bool condition;          //DELETE THIS?
    public Text textToShow;         //Reference to the text to show in the popup
    private Animator popup;         //Reference to this game objects animator
    public RectTransform t;         //Reference to the transform the popup will show on

	// Use this for initialization
	void Start () {
        popup = GetComponent<Animator>();
        popup.Play("default");
        //transform.position = t.position;                
        

	}
		
    /// <summary>
    /// Updates the text shown in the popup window
    /// </summary>
    /// <param name="text"></param>
    public void UpdateText(string text) {
        textToShow.text = text;
    }

    /// <summary>
    /// Hides the popup window immeadiately
    /// </summary>
    public void Stop() {
        popup.Play("default");
    }

    /// <summary>
    /// Plays the disable popup window animation
    /// </summary>
    public void HidePopup()
    {
        popup.Play("hidepopup");
    }

    /// <summary>
    /// Enables the popup window animation
    /// </summary>
    public void ShowPopup()
    {
        if(condition)
            popup.Play("popup");
    }

}
