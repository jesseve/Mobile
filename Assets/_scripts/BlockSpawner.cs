using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour {

    public int maxBlocks;
    public int tracksCount = 8;
    public GameObject block;
    [HideInInspector]
    public int phase;

    private float blockSpeed = 5f;

    private float trackWidth;    

    private List<Track> tracks; 
    private List<GameObject> blockObjects = new List<GameObject>();
    private List<Block> blockScripts = new List<Block>();

    private void Awake() {        
        GenerateBlockPool();        
        SetupSpawner();        
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            SpawnBlock();
        }
    }

    private void SpawnBlock() {
        GameObject clone = GetFreeBlock();
        int index = blockObjects.IndexOf(clone);
        blockScripts[index].Randomize();
        PlaceBlock(index);
    }

    private void PlaceBlock(int index) {
        blockObjects[index].transform.position = tracks[Random.Range(0, tracks.Count)].position;
        blockObjects[index].SetActive(true);
        blockObjects[index].rigidbody2D.velocity = -Vector2.up * blockSpeed;
    }
    private void DestroyBlock() { 
    
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

    private void GenerateBlockPool() {
        for (int i = 0; i < maxBlocks; i++){
            AddBlock();                       
        }
    }

    private GameObject AddBlock() {
        if (blockObjects.Count != blockScripts.Count) Debug.Log("Lists differ from each other");
        GameObject clone = Instantiate(block, Vector3.zero, Quaternion.identity) as GameObject;
        blockObjects.Add(clone);
        blockScripts.Add(clone.GetComponent<Block>());
        clone.SetActive(false);
        return clone;
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
    }    

    struct Track
    {
        public bool used;
        public Vector3 position;
        public Track(Vector3 pos) {
            position = pos;
            used = false;
        }
    }
    
}
