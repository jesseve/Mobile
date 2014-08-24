using UnityEngine;
using System.Collections;
using System;

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

    private PlayerManager player;
    public GameObject scoreGUI;
    public GameObject pauseGUI;
    public GameObject menuGUI;
    public float timeBetweenPhases;
    private float phaseStartTime;
    private BlockSpawner spawner;


    public override void Awake()
    {
        base.Awake();        
        gameAreaWidth = (Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x) * (100f - borderPanelWidth * 2f) * 0.01f; 
        gameAreaWidthHalf = gameAreaWidth * 0.5f;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        spawner.Init();
        trackWidth = spawner.trackWidth;
        //StartGame();
        
    }

    private void Update() {
        if (GetState() != State.Running) return;
        if (Time.time - phaseStartTime > timeBetweenPhases) {
            gamePhase++;
            phaseStartTime = Time.time;
            if(changePhase != null)
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
            pauseGUI.SetActive(true);
            Time.timeScale = 0;
        }
        else if (GetState() == State.Pause) {
            SetState(State.Running);
            pauseGUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public IEnumerator GameOver() {
        SetState(State.GameOver);        
        int score = player.score / 100;
        player.AddMoney(player.score);
        yield return new WaitForSeconds(2f);
        while (player.score > 0) {            
            player.score -= score;
            yield return new WaitForSeconds(0.01f);
        }        
        player.Reset();
        scoreGUI.SetActive(false);
        pauseGUI.SetActive(false);
        menuGUI.SetActive(true);
    }

    public void StartGame() {
        spawner.Init();
        player.Reset();
        scoreGUI.SetActive(true);
        menuGUI.SetActive(false);
        SetState(State.Running);
        phaseStartTime = Time.time;
        gamePhase = 0;
        Time.timeScale = 1f;
    }

    public void Quit() {
        Application.Quit();
    }

}
