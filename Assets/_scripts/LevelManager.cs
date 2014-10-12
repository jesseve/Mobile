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
    public delegate void GameFinished();
    public event GameFinished gameOver;
    public delegate void GameStarted();
    public event GameStarted startGame;

    public float borderPanelWidth;          //Percentage of how much width the sides will be taking from the game area.

    public float GameAreaWidth{
        get {
            return gameAreaWidth;
        }
    }           //Getter for width of game area
    public float GameAreaWidthHalf
    {
        get
        {
            return gameAreaWidthHalf;
        }
    }       //Getter for the half of game areas width
    private float gameAreaWidth;
    private float gameAreaWidthHalf;    

    public int gamePhase;                   //Container for game phase

    private BlockSpawner spawner;           //Reference to spawner
    private PlayerManager player;           //Reference to player
    public GUIHandler gui;                  //Reference to gui handling

    public int money;
    public int coins;
    public int highestCombo;
    public float timeBetweenPhases;
    private float phaseStartTime;    


    public override void Awake()
    {
        base.Awake();                       
    }

    public override void Update() {
        UpdatePhase();
    }

    /// <summary>
    /// Updates the game phase when needed
    /// </summary>
    private void UpdatePhase() {
        if (GetState() != State.Running) return;                //Only increase the gamephase when actually playing
        if (Time.time - phaseStartTime > timeBetweenPhases)
        {   //Increase phase when enough time has passed
            gamePhase++;                                        //Increase the phase by 1
            phaseStartTime = Time.time;                         //Get the time when phase started
            if (changePhase != null)                             //Need to check if there are no subscribers to event
                changePhase();                                  //Call for the changePhase event
        }
    }

    /// <summary>
    /// Get the component instances from initializer
    /// </summary>
    public void Init() {
        CalculateGamearea();
        spawner = Initializer.instance.spawner;
        player = Initializer.instance.player;        
    }

    /// <summary>
    /// Calculates the area the player is able to move in.    
    /// </summary>
    private void CalculateGamearea() {
        gameAreaWidth = (Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x  //The X coordinate of the right side
            - Camera.main.ScreenToWorldPoint(Vector3.zero).x)                            //substract the X coordinate of the left side
            * (100f - borderPanelWidth * 2f) * 0.01f;                                    //multiply by the percentage the sides will be taking
        gameAreaWidthHalf = gameAreaWidth * 0.5f;
    }

    /// <summary>
    /// Initialize the static instance of level manager
    /// </summary>
    protected override void SetupManager()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }

    public void Pause() {
        SoundHandler.instance.PauseSound();
        if (GetState() == State.Running) {
            SetState(State.Pause);            
            Time.timeScale = 0f;
        }
        else if (GetState() == State.Pause) {
            SetState(State.Running);            
            Time.timeScale = 1;
        }
    }     

    /// <summary>
    /// Is called whenever the player quits the play mode
    /// </summary>
    public void GameOver() {
        SetState(State.Menu);               //Switch state back to menu
        if (gameOver != null)
            gameOver();                     //Call the gameover event
        MusicHandler.instance.Switch();     //Swap from stage music to menu music
        Time.timeScale = 1f;                //Reset timescale to normal
        player.Reset();                     //Reset the player back to middle and stop from moving
        spawner.GetLevelValues();           //Set phase 1 values to the spawner while looking at menu
        gui.Menu();                         //Set menu canvas
        Save();
    }

    /// <summary>
    /// Show only when the player finished the game.
    /// Iterates through 0 to the score the player got 
    /// and updates the values to show from GameOverGUIManager
    /// </summary>
    /// <returns></returns>
    public IEnumerator GameOverCalculator() {
        SetState(State.GameOver);
        money = 0;
        player.AddMoney(player.coins);
        Save();
        coins = player.coins;
        highestCombo = player.highestCombo;
        
        if(highestCombo > LevelSelect.instance.currentLevel.highestCombo)
            LevelSelect.instance.currentLevel.highestCombo = highestCombo;
        if (coins > LevelSelect.instance.currentLevel.bestScore)
            LevelSelect.instance.currentLevel.bestScore = coins;


        int originalScore = coins;
        int scoreToSubstract = coins / 200 + 10;
        player.Reset();
        gui.GameOver();       
        yield return new WaitForSeconds(2f);
        while (coins > 0) {            
            coins -= scoreToSubstract;
            money += scoreToSubstract;
            if (money >= originalScore)
                money = originalScore;
            yield return null;
        }
        coins = 0;
        yield return new WaitForSeconds(4f);
        GameOver();
    }

    /// <summary>
    /// Initialize the gameobjects and launch the game
    /// </summary>
    public void StartGame() {        
        CalculateGamearea();                                                        //Make sure the the game area is correct
        timeBetweenPhases = LevelSelect.instance.currentLevel.timeBetweenPhases;    //get the current levels time between each phase
        spawner.ResetSpawner(6f);                                                   //Reset the spawner and start it up after 6 seconds
        SetState(State.Running);                                                    //Enable the game running state
        player.Reset();                                                             //Reset the players values
        MusicHandler.instance.Switch();                                             //Swap from menu music to stage music
        gamePhase = 0;                                                              //Reset game phase
        gui.StartGame();                                                            //Set the correct canvas
        Initializer.instance.healthBar.ResetValues();                               //Reset the healthbars values
        Time.timeScale = 1f;                                                        //Make sure timescale is correct
        if (startGame != null)
            startGame();
        phaseStartTime = Time.time;                                                 //Record the starting time of the first phase
    }        

    /// <summary>
    /// Save the inventory and levels
    /// </summary>
    public void Save() {
        SaveLoad.Save();
    }

    /// <summary>
    /// Save and quit
    /// </summary>
    public void Quit() {
        Save();
        Application.Quit();
    }    
    

}
