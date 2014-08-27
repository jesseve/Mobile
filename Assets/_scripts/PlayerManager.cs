using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : Block {

    public int maxHits;
    
    public int score;
    public int combo;
    public int health {
        get {
            return maxHits - hitsTaken;
        }
    }
    public int Money
    {
        get {
            return money;
        }

        set
        {
            if (value < 9999999)
                money = value;
            else
                money = 9999999;
        }
    }
    private int money;
    public int highestCombo;
    private Movement movement;
    private int hitsTaken;
    public ParticleSystem particles;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        movement = GetComponent<Movement>();
        hitsTaken = 0;
        particles.renderer.sortingLayerName = sprite.sortingLayerName;
        Randomize();
        if(PlayerPrefs.HasKey("PlayerMoney"))
            Money = PlayerPrefs.GetInt("PlayerMoney");
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
            score += 100;
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
        if (combo > highestCombo)
            highestCombo = combo;
        combo = 0;
        EmitParticles();
        hitsTaken++;
        if (hitsTaken >= maxHits) {
            StartCoroutine(LevelManager.instance.GameOverCalculator());
        }
    }

    public void AddMoney(int money) {
        this.Money += money;
    }

    public void Reset() {
        transform.position = Vector3.zero;
        rigidbody2D.velocity = Vector2.zero;
        score = combo = hitsTaken = highestCombo = 0;        
    }

    public void Save() {
        PlayerPrefs.SetInt("PlayerMoney", money);
    }
    
    private void EmitParticles() {
        particles.startColor = color;
        particles.Play();
    }
    
}
