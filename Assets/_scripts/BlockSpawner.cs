using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour {

    public int maxBlocks;
    public int tracksCount = 8;
    public GameObject block;
    [HideInInspector]
    public int phase;
    public float timeBetweenRows = 1f;
    private float timeSinceSpawned;
    public float blockSpeed = 2f;
    public int blocksInRowMin;
    public int blocksInRowMax;
    public float trackWidth;

    private float timeBetweenRowsMin = 1f;
    private float timeBetweenRowsMax = 2f;

    private List<Track> tracks; 
    private List<GameObject> blockObjects = new List<GameObject>();
    private List<SpawnedBlock> blockScripts = new List<SpawnedBlock>();

    private void Awake() {
        //Init();
    }

    public void Init() {
        print("init");
        GenerateBlockPool();        
        SetupSpawner();
        ResetSpawner();
    }

    private void OnEnable() {
        LevelManager.instance.changePhase += ChangePhase;
    }

    private void OnDisable() {
        LevelManager.instance.changePhase -= ChangePhase;
    }

    private void Update() {        
        if (Time.time - timeSinceSpawned > timeBetweenRows) {
            timeSinceSpawned = Time.time;
            timeBetweenRows = Random.Range(timeBetweenRowsMin, timeBetweenRowsMax);
            SpawnRow();
            ReleaseTracks();
        }
    }

    private void ChangePhase() {
        phase = LevelManager.instance.gamePhase;
        if (blocksInRowMin < tracksCount - 4 && phase % 2 == 0) {
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
            blockSpeed += 0.2f;
        }
    }

    #region Block_Methods    

    private void SpawnRow() {
        int blocksAmount = Random.Range(blocksInRowMin, blocksInRowMax);
        for (int i = 0; i < blocksAmount; i++) {
            SpawnBlock();
        }
    }

    private void SpawnBlock()
    {
        GameObject clone = GetFreeBlock();
        int index = blockObjects.IndexOf(clone);        
        PlaceBlock(index);
    }

    private void PlaceBlock(int index) {
        Track t = GetFreeRandomTrack();
        if (t == null) return;
        blockScripts[index].Launch(blockSpeed, t.position);
    }

    private GameObject AddBlock()
    {
        if (blockObjects.Count != blockScripts.Count) Debug.Log("Lists differ from each other");
        GameObject clone = Instantiate(block, Vector3.zero, Quaternion.identity) as GameObject;
        blockObjects.Add(clone);
        SpawnedBlock scriptClone = clone.GetComponent<SpawnedBlock>();
        blockScripts.Add(scriptClone);
        scriptClone.SetSpawner(this);
        scriptClone.ReturnToSpawner();
        return clone;
    }

    private GameObject GetFreeBlock() {
        for (int i = 0; i < blockObjects.Count; i++) {
            if (!blockScripts[i].used) {
                //blockScripts[i].used = true;
                //blockObjects[i].SetActive(true);
                return blockObjects[i];
            }
        }
        Debug.Log("Adding a block to the list");
        return AddBlock();
    }
    #endregion

    #region Init_Methods
    private void GenerateBlockPool() {
        if (blockObjects.Count >= maxBlocks) return;
        for (int i = 0; i < maxBlocks; i++){
            AddBlock();                       
        }
    }

    private void SetupSpawner() {
        tracks = new List<Track>();
        Vector3[] trackPositions = new Vector3[tracksCount];
        trackWidth = LevelManager.instance.GameAreaWidth / tracksCount;
        for (int i = 0; i < tracksCount; i++) {
            if (i == 0)
                trackPositions[i] = transform.position - Vector3.right * LevelManager.instance.GameAreaWidthHalf + Vector3.right * 0.5f * trackWidth;
            else
                trackPositions[i] = trackPositions[i - 1] + new Vector3(trackWidth, 0);
            tracks.Add(new Track(trackPositions[i]));
        }
        timeSinceSpawned = Time.time;
    }

    private void ResetBlocks() { 
        for(int i = 0; i < blockScripts.Count; i++) {
            if (blockObjects[i].activeSelf)
                blockScripts[i].ReturnToSpawner();
        }
    }

    public void ResetSpawner() {
        ResetBlocks();
        timeBetweenRowsMin = 1f;
        timeBetweenRowsMax = 2f;
    }

    #endregion

    #region Track_Methods

    private Track GetFreeRandomTrack() {
        List<int> freeTracks = new List<int>();

        for (int i = 0; i < tracks.Count; i++ )
        {
            if (!tracks[i].used)
            {                
                //tracks[i].SetActive(true);
                freeTracks.Add(i);                
            }
        }        
        int returnValue = freeTracks[Random.Range(0, freeTracks.Count)];

        tracks[returnValue].SetActive(true);

        if (freeTracks.Count > 0)
            return tracks[returnValue];
        else
            return null;
    }

    private void ReleaseTracks()
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            tracks[i].SetActive(false);
        }
    }

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
