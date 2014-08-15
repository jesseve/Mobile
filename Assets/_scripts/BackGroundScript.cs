using UnityEngine;
using System.Collections;

public class BackGroundScript : MonoBehaviour {

    private Sprite sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>().sprite;     
        Vector2 extents = sprite.bounds.extents * 2f;
        float ratio = extents.x / extents.y;        
        float area = LevelManager.instance.GameAreaWidth;
        float newScaleY = area / extents.y;
        float newScaleX = newScaleY * ratio; ;
        transform.localScale = new Vector3(newScaleX, newScaleY);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
