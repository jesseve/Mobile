using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour {
  
    public Shape shape;
    public Color color;
    public Sprite[] sprites;
    public static Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.red + Color.blue };
    public static Shape[] shapes = { Shape.Circle, Shape.Square, Shape.Triangle, Shape.Hexagon, Shape.Diamond };


    protected int colorsUsed = colors.Length;
    protected int shapesUsed = shapes.Length;

    protected SpriteRenderer sprite;

    protected virtual void Start() {
        SetSpriteRenderer();
        float track = LevelManager.instance.trackWidth; //GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>().trackWidth;       
        transform.localScale = new Vector3(track, track, 1f) * 0.5f;
        //if (shapesUsed == 0 || colorsUsed == 0)
          //  SetShapesAndColorsUsed(shapes.Length, colors.Length);
    }

    protected void SetSpriteRenderer() {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void GetLevelValues() {
        SetShapesAndColorsUsed(LevelSelect.instance.currentLevel.shapeCount, LevelSelect.instance.currentLevel.colorCount);
    }

    public void Randomize() {
        GetLevelValues();
        color = colors[Random.Range(0, colorsUsed)];
        shape = shapes[Random.Range(0, shapesUsed)];
        if (sprite == null) SetSpriteRenderer();        
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

    public void SetShapesAndColorsUsed(int shapesToUse, int colorsToUse) {
        if (shapesToUse > shapes.Length) {
            shapesUsed = shapes.Length;
        }
        else {
            shapesUsed = shapesToUse;
        }
        if (colorsToUse > colors.Length)
        {
            colorsUsed = colors.Length;
        }
        else
        {
            colorsUsed = colorsToUse;
        }
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
