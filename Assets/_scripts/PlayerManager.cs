using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private Movement movement;

	// Use this for initialization
	void Start () {
        movement = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveHorizontally(int direction)
    {
        movement.MoveHorizontally(direction);
    }
}
