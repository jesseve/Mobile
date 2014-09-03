using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    private Transform t1;
    private Transform t2;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float levelHeight;
    private float levelTop;
    private float levelBottom;

    public float scrollingSpeed = -0.2f;

	// Use this for initialization
	void Start () {
        ChangeBackground(sprite1.sprite);
        t1 = sprite1.transform;
        t2 = sprite2.transform;        
        levelTop = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y;
        levelBottom = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        levelHeight = (levelTop - levelBottom) * (2f / transform.localScale.y);
        t1.position = Vector3.zero;
        t2.position = t1.position + Vector3.up * levelHeight;
        startPosition = t2.position;
        endPosition = -t2.position;
        
	}
	
	// Update is called once per frame
	void Update () {
        t1.Translate(0, -scrollingSpeed * Time.deltaTime, 0);       
        if (t1.position.y <= endPosition.y)
            t1.position = startPosition;
        if (t1.position.y >= 0)
            t2.position = t1.position - Vector3.up * levelHeight;
        else
            t2.position = t1.position + Vector3.up * levelHeight;
	}

    public void ChangeBackground(Sprite s) {
        sprite1.sprite = s;
        sprite2.sprite = s;
        float height = (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        float area = LevelManager.instance.GameAreaWidth;        
        float spriteWidth = s.rect.width;
        float spriteHeight = s.rect.height;
        float newScaleX = area / (spriteWidth * 0.01f);
        float newScaleY = height / (spriteHeight * 0.01f);
        transform.localScale = new Vector3(newScaleX, newScaleY);
    }
}
