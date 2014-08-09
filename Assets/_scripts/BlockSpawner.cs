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
    private float blockSpeed = 5f;
    public int blocksInRow;
    private float trackWidth;    

    private List<Track> tracks; 
    private List<GameObject> blockObjects = new List<GameObject>();
    private List<Block> blockScripts = new List<Block>();

    private void Awake() {        
        GenerateBlockPool();        
        SetupSpawner();        
    }

    private void Update() {        
        if (Time.time - timeSinceSpawned > timeBetweenRows) {
            timeSinceSpawned = Time.time;
            SpawnRow();
            ReleaseTracks();
        }
    }

    private void ChangePhase() {
        if (blocksInRow < tracksCount)
            blocksInRow++;
    }

    #region Block_Methods
    public void ReturnBlock(GameObject block) {
        int index = blockObjects.IndexOf(block);
        blockObjects[index].rigidbody2D.velocity = Vector2.zero;
        blockScripts[index].used = false;
        blockObjects[index].SetActive(false);
    }

    private void SpawnRow() {
        for (int i = 0; i < blocksInRow; i++) {
            SpawnBlock();
        }
    }

    private void SpawnBlock()
    {
        GameObject clone = GetFreeBlock();
        int index = blockObjects.IndexOf(clone);
        blockScripts[index].Randomize();
        PlaceBlock(index);
    }

    private void PlaceBlock(int index) {
        Track t = GetFreeRandomTrack();
        if (t == null) return;
        blockObjects[index].transform.position = t.position;
        blockObjects[index].SetActive(true);
        blockObjects[index].rigidbody2D.velocity = -Vector2.up * blockSpeed;
    }

    private GameObject AddBlock()
    {
        if (blockObjects.Count != blockScripts.Count) Debug.Log("Lists differ from each other");
        GameObject clone = Instantiate(block, Vector3.zero, Quaternion.identity) as GameObject;
        blockObjects.Add(clone);
        blockScripts.Add(clone.GetComponent<Block>());
        clone.SetActive(false);
        return clone;
    }

    private GameObject GetFreeBlock() {
        for (int i = 0; i < maxBlocks; i++) {
            if (!blockScripts[i].used) {
                blockScripts[i].used = true;
                blockObjects[i].SetActive(true);
                return blockObjects[i];
            }
        }
        Debug.Log("Adding a block to the list");
        return AddBlock();
    }
    #endregion

    #region Init_Methods
    private void GenerateBlockPool() {
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
            //Instantiate(block, trackPositions[i], Quaternion.identity);
        }
        LevelManager.instance.changePhase += ChangePhase;
        timeSinceSpawned = Time.time;
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
