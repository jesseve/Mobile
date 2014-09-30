using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : Block {

    public int maxHits;                 //Number of hits the player can take
    public Inventory inventory;         //All items that need to be save goes in the inventory
    public int coins;                   //Coins the player finds during a game
    public int moneyToAdd;              //How much money the player gets when he collects a diamond
    public int combo;                   //Counter to how many correct diamond player collected in a row
    public delegate void Scored();
    public event Scored scored;

    //Getter for player health
    public int health {
        get {
            return maxHits - hitsTaken;
        }
    }
    
    //Accessor for money
    public int Money
    {
        get {
            return money;
        }

        set
        {
            if (value < 999999999)
                money = value;
            else
                money = 999999999;
        }
    }
    
    private int money;                  //Players money
    public int highestCombo;            //Highes combo achieved during a game
    private Movement movement;          //Reference to movement

    private bool hasShield;             //Has the player bought shield
    public bool shieldOn;               //Is the shield on
    public float shieldCooldown;        //Number of seconds it takes for the shield to recharge
    private int hitsTaken;              //Wrong diamonds collected without shield

    private bool takenDamage;           //if this is true, player is blinking and can't take damage
    private float blinkTime = 3f;       //Time the player is immortal after collecting wrong diamond
    
    public ParticleSystem particles;    //Reference to particle system of player
	

    void OnTriggerEnter2D(Collider2D other) {
        if (LevelManager.instance.GetState() != State.Running) return;      //Detect triggers only when game is running
        SpawnedBlock blockScript = other.GetComponent<SpawnedBlock>();      //Get the block script from object colliding with
        if (blockScript == null) return;                                    //If the script is null, return
        if (ShareColorOrShape(blockScript)) {                               //Check if the diamond colliding player has same color or shape
            shape = blockScript.shape;                                      //Set player shape to match the others shape
            color = blockScript.color;                                      //Set player color to match the others color
            combo++;                                                        //Increase combo by one
            SoundHandler.instance.ComboSound(combo);                        //Play the combo sound
            coins += moneyToAdd * combo;                                    //Update coins earned value
            SetShape();                                                     //Set sprite renderes values
            EmitParticles();                                                //Emit particles from particle system
            if (scored != null)
                scored();
        }
        else if(!takenDamage)                                               //Check if already taken damage and if the other diamond hasn't same color or shape
            TakeDamage();                                                   //If yes then take damage
        blockScript.ReturnToSpawner();                                      //Disable the other diamond
    }    

    /// <summary>
    /// Initialization of the player
    /// Get references and values needed to start
    /// </summary>
    public void InitPlayer() {
        movement = GetComponent<Movement>();                            //Get the reference to movement component
        particles.renderer.sortingLayerName = sprite.sortingLayerName;  //Set the sprite rendering order of particle system to same as player
        GetValuesFromInventory();
        Money = inventory.money;                                        //Set Money to match inventory money
        Reset();                                                        //Reset all values to be ready for starting
        LevelSelect.instance.levelChanged += ReScale;
    }

    /// <summary>
    /// Resets all values back to state where
    /// it's possible to start the game
    /// </summary>
    public void Reset()
    {
        rigidbody2D.velocity = Vector2.zero;                            //Make sure player isn't moving
        ReScale();                                                      //Scale player to current levels track
        Randomize();                                                    //Randomize player color and shape
        GetValuesFromInventory();
        shieldOn = hasShield;                                           //Put the shield on if player has it        
        coins = combo = hitsTaken = highestCombo = 0;                   //Reset values to 0
        moneyToAdd = LevelSelect.instance.currentLevel.coinsPerBlock;   //Get the amount of money to add for scoring from current level
    }

    /// <summary>
    /// Get values from inventory
    /// </summary>
    public void GetValuesFromInventory() {
        maxHits = inventory.health;
        movement.UpdateValues(inventory.speed, inventory.acceleration);
        transform.position = Vector3.zero + (Vector3.up * inventory.position) * .5f;
        hasShield = inventory.shield;
        shieldCooldown = inventory.shieldCooldown;

    }

    public void MoveHorizontally(int direction)
    {
        movement.MoveHorizontally(direction);
    }


    /// <summary>
    /// Method for taking damage
    /// </summary>
    private void TakeDamage() {        
        if (combo > highestCombo)
            highestCombo = combo;       //Set highestcombo if got better combo
        combo = 0;                      //Reset combo


        if (shieldOn)                               //Ignore damage if shield is on
        {
            SoundHandler.instance.ShieldSound();    //Play shield sound
            StartCoroutine(ShieldHit());            //Return and call shieldhit method
            return;
        }

        SoundHandler.instance.DamageSound();        //Play damage sound
        hitsTaken++;                                //Increase the hitstaken by 1
        if (hitsTaken >= maxHits) {                 //Call game over is health reaches 0
            StartCoroutine(LevelManager.instance.GameOverCalculator());
        }
        else
            StartCoroutine(DamageEffect());         //If got health left start blinking
    }

    /// <summary>
    /// Method to add money to player inventory
    /// </summary>
    /// <param name="money">The amount of money to add</param>
    public void AddMoney(int money) {
        this.Money += money;
        inventory.money = Money;
        LevelManager.instance.Save();   //***THIS NEEDS TO CHANGE TO ONLY SAVE THE MONEY***//
    }    
    
    /// <summary>
    /// Method to color and launch the particles
    /// </summary>
    private void EmitParticles() {
        particles.startColor = color;   //Make the particles same color as player
        particles.Play();               //Emit the particles
    }

    /// <summary>
    /// Blinking effect method
    /// </summary>    
    private IEnumerator DamageEffect() {
        takenDamage = true;                                         //Player has taken damage
        float blinkStart = Time.time;                               //Time when blinking started
        while (blinkStart + blinkTime > Time.time) {                //While blinking
            color.a = color.a == 0 ? 1f : 0;                        //Change the colors alpha channel between 0 and 1
            sprite.color = color;                                   //And assign it to sprite renderer
            yield return new WaitForSeconds(0.2f);                  //Time between blinks
        }
        color.a = 1f;                                               //Make sure the color is fully visible
        sprite.color = color;
        takenDamage = false;                                        //Player can take damage again
    }

    /// <summary>
    /// Method to enable cooldown of shield
    /// </summary>    
    private IEnumerator ShieldHit() {
        shieldOn = false;                                       //Disable shield
        yield return new WaitForSeconds(shieldCooldown);        //Wait for shield to recharge
        shieldOn = true;                                        //Enable shield
    }
}
