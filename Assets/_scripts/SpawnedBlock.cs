using UnityEngine;
using System.Collections;

public class SpawnedBlock : Block {

    public bool used;

//    private BlockSpawner spawner;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Destroyer"))
            ReturnToSpawner();
    }

    public void SetSpawner(BlockSpawner spawner) {
        //this.spawner = spawner;
    }

    public void ReturnToSpawner()
    {
        rigidbody2D.velocity = Vector2.zero;
        used = false;
        gameObject.SetActive(false);
    }

    public void Launch(float speed, Vector3 position) {        
        used = true;
        gameObject.SetActive(true);
        transform.position = position;
        rigidbody2D.velocity = -Vector2.up * speed;
        Randomize();
    }

    

}
