using UnityEngine;
using System.Collections;

public class PlayerManager : Block {

    public int maxHits;
    
    public int score;
    public int combo;
    public int health {
        get {
            return maxHits - hitsTaken;
        }
    }
    private Movement movement;
    private int hitsTaken;
    public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {
        base.Start();
        movement = GetComponent<Movement>();
        hitsTaken = 0;
        particles.renderer.sortingLayerName = sprite.sortingLayerName;
        Randomize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (LevelManager.instance.GetState() != State.Running) return;
        SpawnedBlock blockScript = other.GetComponent<SpawnedBlock>();
        if (blockScript == null) return;
        if (ShareColorOrShape(blockScript)) {
            shape = blockScript.shape;
            color = blockScript.color;
            combo++;
            score++;
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
        combo = 0;
        EmitParticles();
        hitsTaken++;
        if (hitsTaken >= maxHits) {
            LevelManager.instance.GameOver();
        }
    }

    public void Reset() {
        transform.position = Vector3.zero;
        rigidbody2D.velocity = Vector2.zero;
        score = combo = hitsTaken = 0;        
    }

    private void EmitParticles() {
        particles.startColor = color;
        particles.Emit(15);

    }
}
