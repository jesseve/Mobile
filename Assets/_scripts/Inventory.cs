using UnityEngine;
using System.Collections;

[System.Serializable]
public class Inventory {

    public int money;

    public float speed = 6f;            //how fast the player moves horizontally
    public float acceleration = 0.025f; //how fast the player accelerates to topspeed
    public float position = 0;          //number of unity units the player is from middle towards the bottom  
    public bool shield;                 //Have the player purchased the shield
    public float shieldCooldown = 10;   //how many seconds it takes for the shield to block again
    public int health = 3;              //health of the player

    //Current level of upgradeable items
    private int speedLevel = 1;
    private int accelerationLevel = 1;
    private int positionLevel = 1;
    private int shieldCooldownLevel = 1;
    private int healthLevel = 1;         

    //Maximum level of the upgrades
    private int maxSpeed = 12;
    private int maxAcc = 12;
    private int maxPosition = 12;
    private int maxShield = 12;
    private int maxHealth = 12;

    /// <summary>
    /// Upgrades the item given as parameter if player has enough money for it
    /// </summary>
    /// <param name="u"></param>
    /// <returns>True if upgrade was succesful</returns>
    public bool UpgradeItem(Upgrade u) {
        Debug.Log("bought " + u.ToString());
        PlayerManager p = Initializer.instance.player;
        int price = GetUpgradePrice(u);
        switch (u) { 
            case Upgrade.Acceleration:
                if (p.Money >= price) {
                    if (accelerationLevel < maxAcc)
                    {
                        p.AddMoney(-price);
                        SoundHandler.instance.PurchaseSound();
                        accelerationLevel++;
                        acceleration += 0.03f;
                    }
                }
                else {
                    return false;
                }
                break;
            case Upgrade.MaxHits:
                if (p.Money >= price) {
                    if (healthLevel < maxHealth)
                    {
                        p.AddMoney(-price);
                        SoundHandler.instance.PurchaseSound();
                        healthLevel++;
                        health++;
                    }
                }
                else {
                    return false;
                }
                break;
            case Upgrade.Position:
                if (p.Money >= price) {
                    if (positionLevel < maxPosition)
                    {
                        p.AddMoney(-price);
                        SoundHandler.instance.PurchaseSound();
                        positionLevel++;
                        position -= (Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y)*0.5f/maxPosition;
                    }
                }
                else {
                    return false;
                }
                break;
            case Upgrade.Shield:
                if (!shield) {
                    shield = true;
                    SoundHandler.instance.PurchaseSound();
                    break;
                }
                if (p.Money >= price) {
                    if (shieldCooldownLevel < maxShield)
                    {
                        p.AddMoney(-price);
                        SoundHandler.instance.PurchaseSound();
                        shieldCooldownLevel++;
                        shieldCooldown -= 0.5f;
                    }
                }
                else {
                    return false;
                }
                break;
            case Upgrade.Speed:
                if (p.Money >= price) {
                    if (speedLevel < maxSpeed)                        
                    {
                        p.AddMoney(-price);
                        SoundHandler.instance.PurchaseSound();
                        speedLevel++;
                        speed += 0.5f;
                    }
                }
                else {
                    return false;
                }
                break;
        }
        SaveLoad.Save();
        p.GetValuesFromInventory();
        return true;
    }

    /// <summary>
    /// Updates the players inventory from save file when loading the game
    /// or updates the inventory in save class when saving the game
    /// </summary>
    /// <param name="inv"></param>
    public void UpdateInventory(Inventory inv) {
        money = inv.money;
        speed = inv.speed;
        acceleration = inv.acceleration;
        position = inv.position;
        shield = inv.shield;
        shieldCooldown = inv.shieldCooldown;
        health = inv.health;
    }

    /// <summary>
    /// Calculates the price of an upgrade
    /// </summary>
    /// <param name="u"></param>
    /// <returns>The price or 0 if the upgrade is fully upgraded</returns>
    public int GetUpgradePrice(Upgrade u) {
        switch (u)
        {
            case Upgrade.Acceleration:
                if(accelerationLevel < maxAcc)
                    return accelerationLevel * accelerationLevel * 1000;
                break;
            case Upgrade.MaxHits:
                if(healthLevel < maxHealth)
                    return healthLevel * healthLevel * 1000;
                break;
            case Upgrade.Position:
                if(positionLevel < maxPosition)
                    return positionLevel * positionLevel * 1000;
                break;
            case Upgrade.Shield:
                if (shield)
                {
                    if (shieldCooldownLevel < maxShield)
                        return shieldCooldownLevel * shieldCooldownLevel * 1000;
                    else
                        return 0;
                }
                else
                    return 1000000;
                break;
            case Upgrade.Speed:
                if(speedLevel < maxSpeed)
                    return speedLevel * speedLevel * 1000;
                break;
        }
        return 0;
    }

    /// <summary>
    /// Gets the upgrades current level
    /// in a form of "currentlevel"/"maxlevels"
    /// and return it as a string
    /// </summary>
    /// <param name="u"></param>
    /// <returns>the upgrade level or a space if the upgrade given was faulty</returns>
    public string GetUpgradeLevel(Upgrade u)
    {
        switch (u)
        {
            case Upgrade.Acceleration:
                return accelerationLevel.ToString() + "/" + maxAcc.ToString();
            case Upgrade.MaxHits:
                return healthLevel.ToString() + "/" + maxHealth.ToString();
            case Upgrade.Position:
                return positionLevel.ToString() + "/" + maxPosition.ToString();
            case Upgrade.Shield:
                if (!shield)
                    return "No shield";
                else
                    return shieldCooldownLevel.ToString() + "/" + maxShield.ToString();
               
            case Upgrade.Speed:
                return speedLevel.ToString() + "/" + maxSpeed.ToString();
        }
        return " ";
    }
}

public enum Upgrade { 
    Speed,
    Acceleration,
    Position,
    Shield,
    MaxHits
}