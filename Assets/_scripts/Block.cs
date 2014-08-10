using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour {

    public Shape shape;
    public Color color;
    public Sprite[] sprites;
    public static Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public static Shape[] shapes = { Shape.Circle, Shape.Square, Shape.Triangle, Shape.Hexagon, Shape.Diamond };

    protected SpriteRenderer sprite;

    protected virtual void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Randomize() { 
        color = colors[Random.Range(0, colors.Length)];
        shape = shapes[Random.Range(0, shapes.Length)];

        SetShape();
    }

    protected void SetShape() {
        sprite.sprite = GetSprite();
        sprite.color = color;
    }

    protected Sprite GetSprite() {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].texture.name == shape.ToString())
                return sprites[i];
        }
        return sprites[0];
    }

    public bool ShareColor(Block other)
    {
        return (color == other.color);
    }

    public bool ShareShape(Block other)
    {
        return (shape == other.shape);
    }

    public bool ShareColorOrShape(Block other) {
        return (shape == other.shape || color == other.color);
    }

    public bool ShareColorAndShape(Block other)
    {
        return (shape == other.shape && color == other.color);
    }

}
