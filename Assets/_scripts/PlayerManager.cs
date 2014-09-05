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

    public HealthBar healthBar;

    private bool takenDamage; //if this is true, player is blinking and can't take damage
    private float blinkTime = 3f;
    
    public ParticleSystem particles;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        movement = GetComponent<Movement>();
        hitsTaken = 0;
        particles.renderer.sortingLayerName = sprite.sortingLayerName;
        InitPlayer();
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
            EmitParticles();
        }
        else if(!takenDamage)
            TakeDamage();
        blockScript.ReturnToSpawner();
    }

    public void InitPlayer() {
        Randomize();
        Reset();
        healthBar.Init();
    }

    public void MoveHorizontally(int direction)
    {
        movement.MoveHorizontally(direction);
    }

    private void TakeDamage() {
        if (combo > highestCombo)
            highestCombo = combo;
        combo = 0;        
        hitsTaken++;
        healthBar.ChangeColor();
        if (hitsTaken >= maxHits) {
            StartCoroutine(LevelManager.instance.GameOverCalculator());
        }
        else
            StartCoroutine(DamageEffect());
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

    private IEnumerator DamageEffect() {
        takenDamage = true;
        float blinkStart = Time.time;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();      
        while (blinkStart + blinkTime > Time.time) {
            color.a = color.a == 0 ? 1f : 0;
            renderer.color = color;
            yield return new WaitForSeconds(0.2f);
        }
        color.a = 1f;
        renderer.color = color;
        takenDamage = false;
    }
}
