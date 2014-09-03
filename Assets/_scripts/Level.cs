using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level {

    public int colorCount;
    public int shapeCount;
    public int trackCount;

    public bool unlocked;
    public int costToUnlock;

    public bool defaultBlockDirection = true;

    public int scorePerBlock;
    public float comboMultiplier;

    public float timeBetweenPhases;

    public float blockStartSpeed;
    public float blockSpeedIncrease;

    public float timeBetweenRowsMin;
    public float timeBetweenRowsMax;

    public int blocksInRowMin;
    public int blocksInRowMax;

    public Sprite levelSprite;
}
