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
    public GameObject gameOverGUI;
    
    public Sprite[] backgrounds;
    private int backgroundIndex;

    public int money;
    public int score;
    public int highestCombo;
    public float timeBetweenPhases;
    private float phaseStartTime;
    private BlockSpawner spawner;
    private BackGroundScript background;


    public override void Awake()
    {
        base.Awake();        
        gameAreaWidth = (Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x) * (100f - borderPanelWidth * 2f) * 0.01f; 
        gameAreaWidthHalf = gameAreaWidth * 0.5f;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<BackGroundScript>();
        spawner.Init();
        trackWidth = spawner.trackWidth;
        SetGUI(menuGUI);
        //StartGame();
        
    }

    public override void Update() {
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

    public void GameOver() {
        SetState(State.Menu);
        SetGUI(menuGUI);
    }

    public IEnumerator GameOverCalculator() {
        SetState(State.GameOver);
        money = 0;
        player.AddMoney(player.score);
        player.Save();
        score = player.score;
        highestCombo = player.highestCombo;
        int originalScore = score;
        int scoreToSubstract = score / 200 + 10;
        player.Reset();
        SetGUI(gameOverGUI);
        yield return new WaitForSeconds(2f);
        while (score > 0) {            
            score -= scoreToSubstract;
            money += scoreToSubstract;
            if (money >= originalScore)
                money = originalScore;
            yield return null;
        }
        score = 0;
        yield return new WaitForSeconds(4f);
        SetGUI(menuGUI);
    }

    public void StartGame() {
        spawner.Init();
        player.Reset();
        SetGUI(scoreGUI);
        SetState(State.Running);
        phaseStartTime = Time.time;
        gamePhase = 0;
        Time.timeScale = 1f;
    }

    public void Quit() {
        player.Save();
        Application.Quit();
    }

    private void SetGUI(GameObject gui) {
        scoreGUI.SetActive(false);
        pauseGUI.SetActive(false);
        menuGUI.SetActive(false);
        gameOverGUI.SetActive(false);
        gui.SetActive(true);
    }

    public void ChangeBackground(int i) {
        print(i);
        backgroundIndex += i;
        if (backgroundIndex >= backgrounds.Length)
            backgroundIndex = 0;
        else if(backgroundIndex < 0)
            backgroundIndex = backgrounds.Length - 1;
        background.ChangeBackground(backgrounds[backgroundIndex]);
    }

}
