using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    private Sprite sprite;

	// Use this for initialization
	void Start () {
        ChangeBackground(sprite);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ChangeBackground(Sprite s) {
        float height = (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        float area = LevelManager.instance.GameAreaWidth;
        sprite = GetComponent<SpriteRenderer>().sprite;
        float spriteWidth = sprite.rect.width;
        float spriteHeight = sprite.rect.height;
        float newScaleX = area / (spriteWidth * 0.01f);
        float newScaleY = height / (spriteHeight * 0.01f);
        transform.localScale = new Vector3(newScaleX, newScaleY);
    }
}
