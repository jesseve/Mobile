using UnityEngine;
using System.Collections;

public class PlayerManager : Block {

    public int maxHits;

    private Movement movement;
    private int hitsTaken;    

	// Use this for initialization
	protected virtual void Awake () {
        base.Awake();
        movement = GetComponent<Movement>();
        hitsTaken = 0;
        Randomize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        SpawnedBlock blockScript = other.GetComponent<SpawnedBlock>();
        if (blockScript == null) return;
        if (ShareColorOrShape(blockScript)) {
            shape = blockScript.shape;
            color = blockScript.color;
            SetShape();
        }
        else
            TakeDamage();
        blockScript.ReturnToSpawner();
    }

    public void MoveHorizontally(int direction)
    {
        movement.MoveHorizontally(direction);
    }

    private void TakeDamage() {
        hitsTaken++;
        if (hitsTaken >= maxHits) {
            LevelManager.instance.GameOver();
        }
    }
}
