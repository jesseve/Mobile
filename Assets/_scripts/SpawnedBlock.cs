﻿using UnityEngine;
using System.Collections;

public class SpawnedBlock : Block {

    public bool used;

    private BlockSpawner spawner;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Destroyer"))
            ReturnToSpawner();
    }

    public void ReturnToSpawner()
    {
        spawner.ReturnBlock(gameObject);
    }

    

}
