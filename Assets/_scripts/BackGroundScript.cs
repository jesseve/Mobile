using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        ChangeBackground(sprite.sprite);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ChangeBackground(Sprite s) {
        sprite.sprite = s;
        float height = (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        float area = LevelManager.instance.GameAreaWidth;        
        float spriteWidth = s.rect.width;
        float spriteHeight = s.rect.height;
        float newScaleX = area / (spriteWidth * 0.01f);
        float newScaleY = height / (spriteHeight * 0.01f);
        transform.localScale = new Vector3(newScaleX, newScaleY);
    }
}
