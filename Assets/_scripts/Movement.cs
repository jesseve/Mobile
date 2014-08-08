using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float speed;

    private Vector2 velocity;

	// Use this for initialization
	void Start () {
        velocity = new Vector3(speed, 0);
	}

    /// <summary>
    /// Moves the character on X-axis
    /// </summary>
    /// <param name="direction">negative = left, positive = right</param>
    public void MoveHorizontally(int direction) {
        Vector3 position = transform.position;
        float x = position.x;
        position.x = Mathf.Clamp(position.x, -LevelManager.instance.GameAreaWidthHalf, LevelManager.instance.GameAreaWidthHalf);
        if (position.x == x || CheckDirection(direction))
        {
            rigidbody2D.velocity = velocity * direction;
        }
        else 
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }


    /// <summary>
    /// Check if the direction is away from the gamearea borders
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckDirection(int direction) 
    {
        if (Mathf.Sign(direction) != Mathf.Sign(transform.position.x))
            return true;
        return false;
    }
}
