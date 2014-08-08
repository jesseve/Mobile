using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public bool used;
    public Shape shape;
    public Color color;
    public static Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public static Shape[] shapes = { Shape.Circle, Shape.Square, Shape.Triangle, Shape.Hexagon, Shape.Diamond };

	// Use this for initialization
	void Start () {      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Randomize() { 
        color = colors[Random.Range(0, colors.Length)];
        shape = shapes[Random.Range(0, shapes.Length)];
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

    public static bool operator ==(Block first, Block second) {
        return (first.shape == second.shape && first.color == second.color);
    }
    public static bool operator !=(Block first, Block second)
    {
        return (first.shape == second.shape && first.color == second.color);         
    }
}
