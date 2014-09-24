using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer sprite3;

    private Transform t1;
    private Transform t2;
    private Transform t3;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float levelHeight;
    private float levelTop;
    private float levelBottom;

    public float scrollingSpeed = -0.2f;
    private float targetSpeed;

	// Use this for initialization
	void Start () {
        ChangeBackground(sprite1.sprite);
        t1 = sprite1.transform;
        t2 = sprite2.transform;
        t3 = sprite3.transform;
        levelTop = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y;
        levelBottom = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        levelHeight = (levelTop - levelBottom);
        t1.position = Vector3.zero;
        t2.position = t1.position + Vector3.up * levelHeight;
        t3.position = t1.position - Vector3.up * levelHeight;
        startPosition = t2.position * 0.5f;
        endPosition = -t2.position *0.5f;
        targetSpeed = scrollingSpeed;
        scrollingSpeed = 0;
        
	}
	
	/// <summary>
	/// Manages the positions of the backgrounds
	/// </summary>
	void FixedUpdate () {
        if (LevelManager.instance.GetState() != State.Running) return;
        AccelerateScrolling();
        t1.Translate(0, -scrollingSpeed, 0);       
        if (t1.position.y <= endPosition.y)
            t1.position = startPosition;
        /*if (t1.position.y >= 0)
            t2.position = t1.position - Vector3.up * levelHeight;
        else
            t2.position = t1.position + Vector3.up * levelHeight;*/
        t2.position = t1.position - Vector3.up * levelHeight;
        t3.position = t1.position + Vector3.up * levelHeight;
	}

    void OnEnable() { 
        LevelManager.instance.changePhase += AddSpeed;
        LevelManager.instance.gameOver += Reset;
    }

    /// <summary>
    /// Calculates a new scale for the backgrounds based on the sprite given as a parameter
    /// Background has the same width as game area and same height as the screen
    /// </summary>
    /// <param name="s"></param>
    public void ChangeBackground(Sprite s) {
        sprite1.sprite = s;
        sprite2.sprite = s;
        sprite3.sprite = s;
        float height = (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        float area = LevelManager.instance.GameAreaWidth;        
        float spriteWidth = s.rect.width;
        float spriteHeight = s.rect.height;
        float newScaleX = area / (spriteWidth * 0.01f);
        float newScaleY = height / (spriteHeight * 0.01f);
        transform.localScale = new Vector3(newScaleX, newScaleY);
    }

    /// <summary>
    /// Accelerate the scrolling speed of backgrounds
    /// </summary>
    private void AccelerateScrolling() {       
        if (Mathf.Abs(scrollingSpeed) < Mathf.Abs(targetSpeed))
            scrollingSpeed += 0.0001f * Mathf.Sign(targetSpeed);      
    }

    /// <summary>
    /// Adds speed to backgrounds scrolling
    /// The longer the game lasts the faster background scrolls
    /// </summary>
    private void AddSpeed() {
        targetSpeed += Mathf.Sign(targetSpeed) * 0.001f;
    }

    /// <summary>
    /// Resets the scrolling speed when game is over
    /// </summary>
    private void Reset() {
        targetSpeed = scrollingSpeed;
        scrollingSpeed = 0;
    }

}
