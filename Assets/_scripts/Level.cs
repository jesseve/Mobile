using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level {

    /*
     *
     * Holds all values of a level needed to get the level running
     * Modify the values to make each level different
     * 
     */

    public string name;

    public int colorCount;
    public int shapeCount;
    public int trackCount;

    public int highestCombo = 0;
    public int bestScore = 0;

    public bool unlocked;
    public int costToUnlock;

    public bool defaultBlockDirection = true;

    public int coinsPerBlock;
    public float comboMultiplier;

    public float timeBetweenPhases;

    public float blockStartSpeed;
    public float blockSpeedIncrease;

    public float timeBetweenRowsMin;
    public float timeBetweenRowsMax;

    public int blocksInRowMin;
    public int blocksInRowMax;    
}
