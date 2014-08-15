﻿using UnityEngine;
using System.Collections;

public class LevelManager : GameManager {

    public static LevelManager instance
    {
        get;
        private set;
    }

    public delegate void ChangePhase();
    public event ChangePhase changePhase;

    public float borderPanelWidth;

    public float GameAreaWidth{
        get {
            return gameAreaWidth;
        }
    }
    public float GameAreaWidthHalf
    {
        get
        {
            return gameAreaWidthHalf;
        }
    }
    private float gameAreaWidth;
    private float gameAreaWidthHalf;

    public float trackWidth;

    public int gamePhase;

    public float timeBetweenPhases;
    private float phaseStartTime;

    public override void Awake()
    {
        base.Awake();        
        gameAreaWidth = (Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x) * (100f - borderPanelWidth * 2f) * 0.01f; 
        gameAreaWidthHalf = gameAreaWidth * 0.5f;
        Debug.Log(gameAreaWidth);        

        BlockSpawner spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        spawner.Init();
        trackWidth = spawner.trackWidth;        

        phaseStartTime = Time.time;
    }

    private void Update() {
        if (Time.time - phaseStartTime > timeBetweenPhases) {
            gamePhase++;
            phaseStartTime = Time.time;
            changePhase();
        }
    }

    protected override void SetupManager()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }

    public void Pause() {
        if (GetState() == State.Running) {
            SetState(State.Pause);
            Time.timeScale = 0;
        }
        else if (GetState() == State.Pause) {
            SetState(State.Running);
            Time.timeScale = 1;
        }
    }

    public void GameOver() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
