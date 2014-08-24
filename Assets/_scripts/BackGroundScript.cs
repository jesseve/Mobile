using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    private Sprite sprite;

	// Use this for initialization
	void Start () {
        float height = (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y);  
        float area = LevelManager.instance.GameAreaWidth;
        float newScaleX = area;
        float newScaleY = height;
        transform.localScale = new Vector3(newScaleX, newScaleY);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
