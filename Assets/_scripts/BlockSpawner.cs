using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour {

    public int maxBlocks;                   //Maximum amount of diamonds to generate at start
    public int tracksCount = 8;             //How many columns of diamonds is in a row
    public float trackWidth;                //Width of a track
    public GameObject block;                //Prefab of the diamond
    [HideInInspector]
    public int phase;                       //Phase of the game


    public float timeBetweenRows = 1f;      //Time to wait after launching a row of diamonds
    private float timeBetweenRowsMin = 1f;  //Minimum value for timeBetweenRows
    private float timeBetweenRowsMax = 2f;  //Maximum value. Actual value randomized between min and max
    private float timeSinceSpawned;         //Keeps the time when a row was launched
    
    public float blockSpeed = 2f;           //Speed of a diamond
    private float blockSpeedIncrease;       //Amount of speed to increase when a phase changes

    public int blocksInRowMin;              //The minimum amount of diamonds to launch at one time
    public int blocksInRowMax;              //The maximum amount of diamonds to launch at one time

    private List<Track> tracks;                                             //List of tracks. A track has a boolean value "used" and position
    private List<GameObject> blockObjects = new List<GameObject>();         //List of diamond prefabs. On launch will take the objects and activate them.
    private List<SpawnedBlock> blockScripts = new List<SpawnedBlock>();     //SpawnedBlock script of each diamond in the blockObjects list.
     



    /// <summary>
    /// Generates the blockpool
    /// and resets values to be ready to start
    /// </summary>
    public void Init() {        
        GenerateBlockPool();                
        ResetSpawner();        
    }

    /// <summary>
    /// Add ChangePhase method to levelmanagers changePhase event
    /// </summary>
    private void OnEnable() {
        LevelManager.instance.changePhase += ChangePhase;
        LevelSelect.instance.levelChanged += ChangeLevel;
    }

    /// <summary>
    /// Unsubscribe ChangePhase method from changePhase event
    /// </summary>
    private void OnDisable() {
        LevelManager.instance.changePhase -= ChangePhase;
    }

    private void Update() {        
        if (Time.time - timeSinceSpawned > timeBetweenRows) {
            timeSinceSpawned = Time.time;                                               //Save the time of current spawn
            timeBetweenRows = Random.Range(timeBetweenRowsMin, timeBetweenRowsMax);     //Get a random value how much time to spawn again
            SpawnRow();                                                                 //Spawn the row
            ReleaseTracks();                                                            //Set all tracks unused so they can be used again
        }
    }

    /// <summary>
    /// Controls what happens when level manager changes phase
    /// </summary>
    private void ChangePhase() {
        phase = LevelManager.instance.gamePhase;        
        int phaseAction = phase % 2;
        if (phase < 11) {
            switch (phaseAction) {
                case 0:
                    blockSpeed += blockSpeedIncrease;
                    timeBetweenRowsMin *= .9f;
                    if(blocksInRowMin < tracksCount / 2)
                        blocksInRowMin++;
                    break;
                case 1:
                    timeBetweenRowsMax *= .9f;
                    if(blocksInRowMax <= tracksCount - 1)
                        blocksInRowMax++;
                    break;
            }
        }
        else if(phaseAction == 0 && blockSpeed < 8)
            blockSpeed += blockSpeedIncrease;
        /*if (blocksInRowMin < tracksCount - 4 && phase % 2 == 0) {
            blocksInRowMin++;
        }
        if (blocksInRowMax < tracksCount - 1) {            
            blocksInRowMax++;            
        }
        if (phase % 3 == 0 && blockSpeed < 8f) {
            if (timeBetweenRowsMin > 0.5f) {
                timeBetweenRowsMin -= 0.1f;
                timeBetweenRowsMax -= 0.15f;
            }
            blockSpeed += blockSpeedIncrease;
        }*/
    }

    #region Block_Methods    

    /// <summary>
    /// Spawn a random number of diamonds
    /// between the min and max values
    /// </summary>
    private void SpawnRow() {
        int blocksAmount = Random.Range(blocksInRowMin, blocksInRowMax);    //Get the amount of diamonds to spawn
        for (int i = 0; i < blocksAmount; i++) {                            //Spawn the diamonds in a loop
            SpawnBlock();
        }
    }

    /// <summary>
    /// Method to spawn a single diamond
    /// </summary>
    private void SpawnBlock()
    {
        GameObject clone = GetFreeBlock();          //First get a free diamond from the blockObject list
        int index = blockObjects.IndexOf(clone);    //Find the index of the diamond in the list
        PlaceBlock(index);                          //Place the diamond on a track
    }

    /// <summary>
    /// Set a diamond on a track and launch it
    /// </summary>
    /// <param name="index">the index of the diamond to launch</param>
    private void PlaceBlock(int index) {
        Track t = GetFreeRandomTrack();                     //Find a free track
        if (t == null) return;                              //Make sure the track isn't null
        blockScripts[index].Launch(blockSpeed, t.position); //Launch the diamond
    }

    /// <summary>
    /// Adds a diamond to the blockObjects list
    /// </summary>
    /// <returns>Gameobject added to the list</returns>
    private GameObject AddBlock()
    {
        if (blockObjects.Count != blockScripts.Count) Debug.Log("Lists differ from each other");
        GameObject clone = Instantiate(block, Vector3.zero, Quaternion.identity) as GameObject;     //Instantiate a clone of the prefab diamond
        blockObjects.Add(clone);                                                                    //Add the clone to list
        SpawnedBlock scriptClone = clone.GetComponent<SpawnedBlock>();                              //Get SpawnedBlock script from the clone
        blockScripts.Add(scriptClone);                                                              //Add the script to scripts list
        scriptClone.ReturnToSpawner();                                                              //Disable the added gameobject
        return clone;                                                                               //Return the clone
    }

    /// <summary>
    /// Finds a unused diamond from blockObjects list
    /// Adds a block to the list if none are available
    /// </summary>
    /// <returns>The diamond found</returns>
    private GameObject GetFreeBlock() {
        for (int i = 0; i < blockObjects.Count; i++) {      //Loop through the list of diamonds
            if (!blockScripts[i].used) {                    //If found a unused diamond                
                return blockObjects[i];                     //Return it
            }
        }
        Debug.Log("Adding a block to the list");
        return AddBlock();                                  //Else add a diamond to the list and return the new diamond
    }
    #endregion

    #region Init_Methods

    /// <summary>
    /// Adds diamonds to the blockobjects list
    /// until the lists lenght is same than maxBlocks
    /// </summary>
    private void GenerateBlockPool() {
        if (blockObjects.Count >= maxBlocks) return;    //Do not add any diamonds if there is enough already
        for (int i = 0; i < maxBlocks; i++){            //Add the diamonds in a loop
            AddBlock();                       
        }
    }

    /// <summary>
    /// Reset the tracks when the game starts.
    /// </summary>
    private void ResetTracks() {
        tracks = new List<Track>();                                     //Make a new empty list of tracks
        Vector3[] trackPositions = new Vector3[tracksCount];            //Create a temporary array for every tracks position. Need the array to calculate next tracks position
        trackWidth = LevelManager.instance.GameAreaWidth / tracksCount; //Calculate the width of 1 track
        for (int i = 0; i < tracksCount; i++) {                         //In a loop calculate the position for every track
            if (i == 0) {
                //Find the most left position of the gamearea and add a half of a trackswidth to it to get the first tracks position
                trackPositions[i] = transform.position - Vector3.right * LevelManager.instance.GameAreaWidthHalf + Vector3.right * 0.5f * trackWidth;
            }
            else {
                //Add a tracks width to the last tracks position to get the rest positions
                trackPositions[i] = trackPositions[i - 1] + new Vector3(trackWidth, 0); 
            }
            tracks.Add(new Track(trackPositions[i]));                   //Add a new track with the calculated position to the list
        }
    }

    /// <summary>
    /// Get the values of the current level
    /// </summary>
    public void GetLevelValues() {         
        blockSpeed = LevelSelect.instance.currentLevel.blockStartSpeed;
        blockSpeedIncrease = LevelSelect.instance.currentLevel.blockSpeedIncrease;
        blocksInRowMin = LevelSelect.instance.currentLevel.blocksInRowMin;
        blocksInRowMax = LevelSelect.instance.currentLevel.blocksInRowMax;
        timeBetweenRowsMin = LevelSelect.instance.currentLevel.timeBetweenRowsMin;
        timeBetweenRowsMax = LevelSelect.instance.currentLevel.timeBetweenRowsMax;
        tracksCount = LevelSelect.instance.currentLevel.trackCount;
    }    

    /// <summary>
    /// Return every diamond to spawner and disable them
    /// </summary>
    private void ResetBlocks() { 
        for(int i = 0; i < blockScripts.Count; i++) {
            if (blockObjects[i].activeSelf)
                blockScripts[i].ReturnToSpawner();
        }
    }

    /// <summary>
    /// Reset the values and set the spawner ready
    /// Has an optional parameter to delay the first spawn
    /// </summary>
    /// <param name="t"></param>
    public void ResetSpawner(float t = 0) {        
        ResetBlocks();                  //Disable every diamond
        GetLevelValues();               //Find the levels values
        ResetTracks();                  //Calculate the new positions for tracks
        timeSinceSpawned = Time.time;   //Reset the spawning timer
        if (t > 0)                      //If got the parameter t then wait longer before starting
        {            
            timeBetweenRows = 5f;       //Wait for 5 seconds before first spawn
        }
    }

    /// <summary>
    /// Updates the values and resizes blocks when player changes level
    /// </summary>
    private void ChangeLevel() {
        GetLevelValues();
        ResetTracks();
    }

    #endregion

    #region Track_Methods

    /// <summary>
    /// Finds a free track in the track list
    /// </summary>
    /// <returns>a free track or null if none are free</returns>
    private Track GetFreeRandomTrack() {
        List<int> freeTracks = new List<int>();     //List of all free tracks with their index

        for (int i = 0; i < tracks.Count; i++ )
        {
            if (!tracks[i].used)                    //If the track is free
            {                
                freeTracks.Add(i);                  //Add it to the freetracks list
            }
        }        
        
        if (freeTracks.Count > 0)
        {
            int returnValue = freeTracks[Random.Range(0, freeTracks.Count)];    //Get a random index of the free tracks

            tracks[returnValue].SetActive(true);                                //Set the tracks state to used.
            return tracks[returnValue];                                         //return track
        }
        else
            return null;                                                        //If no track were free return null
    }

    /// <summary>
    /// Set every tracks used value to false
    /// </summary>
    private void ReleaseTracks()
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            tracks[i].SetActive(false);
        }
    }

    /// <summary>
    /// Holds a boolean value "used" and a position    
    /// </summary>
    class Track
    {
        public bool used;
        public Vector3 position;
        public Track(Vector3 pos) {
            position = pos;
            used = false;
        }
        public void SetActive(bool active)
        {
            used = active;
        }
    }

    #endregion

}
