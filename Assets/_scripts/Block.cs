using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour {
  
    public Shape shape;
    public Color color;
    public Sprite[] sprites;
    public static Color[] colors = { Color.red, new Color(0,0.5f,1f), Color.green, Color.yellow, Color.red + Color.blue };
    public static Shape[] shapes = { Shape.Circle, Shape.Square, Shape.Triangle, Shape.Hexagon, Shape.Diamond, Shape.Star };


    protected int colorsUsed = colors.Length;
    protected int shapesUsed = shapes.Length;

    protected SpriteRenderer sprite;

    protected virtual void Awake() {
        SetSpriteRenderer();          
    }

    /// <summary>
    /// Inits the reference to the sprite renderer
    /// </summary>
    protected void SetSpriteRenderer() {
        sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Scales the gameobject to fit on a track
    /// Diamond is 75% of the tracks width
    /// </summary>
    protected void ReScale() {        
        float track = Initializer.instance.spawner.trackWidth;
        transform.localScale = new Vector3(track, track, 1f) * 0.75f;
    }

    /// <summary>
    /// Finds the number of shapes and colors to use from current level
    /// </summary>
    private void GetLevelValues() {
        SetShapesAndColorsUsed(LevelSelect.instance.currentLevel.shapeCount, LevelSelect.instance.currentLevel.colorCount);
    }

    /// <summary>
    /// Gives the diamond a random shape and color
    /// </summary>
    public void Randomize() {
        GetLevelValues();
        color = colors[Random.Range(0, colorsUsed)];
        shape = shapes[Random.Range(0, shapesUsed)];
        if (sprite == null) SetSpriteRenderer();        
        SetShape();
    }

    /// <summary>
    /// Set sprite renderers color and shape
    /// </summary>
    protected void SetShape() {
        sprite.sprite = GetSprite();
        sprite.color = color;
    }

    /// <summary>
    /// Returns the sprite thats name matches the shape
    /// </summary>
    /// <returns></returns>
    protected Sprite GetSprite() {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].texture.name == shape.ToString())
                return sprites[i];
        }
        return sprites[0];
    }

    /// <summary>
    /// Sets the number of shapes and colors
    /// to be used in the game
    /// Parameter values come from currentlevel
    /// </summary>
    /// <param name="shapesToUse"></param>
    /// <param name="colorsToUse"></param>
    public void SetShapesAndColorsUsed(int shapesToUse, int colorsToUse) {
        if (shapesToUse > shapes.Length) {
            shapesUsed = shapes.Length;
        } else {
            shapesUsed = shapesToUse;
        }

        if (colorsToUse > colors.Length) {
            colorsUsed = colors.Length;
        } else {
            colorsUsed = colorsToUse;
        }
    }


    /// <summary>
    /// Compares a block to this block and returns true if they have same color
    /// </summary>
    /// <param name="other">Diamond to compare to this diamond</param>
    /// <returns></returns>
    public bool ShareColor(Block other)
    {
        return (other.color.r == color.r && other.color.g == color.g && other.color.b == color.b) ? true : false;
    }

    /// <summary>
    /// Compares a diamond to this diamond and returns true if they have same shape
    /// </summary>
    /// <param name="other">Diamond to compare to this diamond</param>
    /// <returns></returns>
    public bool ShareShape(Block other)
    {
        return (shape == other.shape);
    }

    /// <summary>
    /// Compares another diamond to this and returns true if they share a color or shape
    /// </summary>
    /// <param name="other">Diamond to compare to this diamond</param>
    /// <returns></returns>
    public bool ShareColorOrShape(Block other) {
        return (ShareColor(other) || ShareShape(other));
    }

    /// <summary>
    /// Compares another diamond to this and returns true if they share both color and shape
    /// </summary>
    /// <param name="other">Diamond to compare to this diamond</param>
    /// <returns></returns>
    public bool ShareColorAndShape(Block other)
    {
        return (shape == other.shape && color == other.color);
    }

}
