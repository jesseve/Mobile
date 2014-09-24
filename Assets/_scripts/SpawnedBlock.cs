using UnityEngine;
using System.Collections;

public class SpawnedBlock : Block {

    public bool used;   //The diamond can only be launched only if its not used
    
    /// <summary>
    /// On collision with destroyer send the diamond back to the spawner
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Destroyer"))
            ReturnToSpawner();
    }   

    /// <summary>
    /// Return the diamond to spawner
    /// Resets velocity, set used to false so it can be used again
    /// Disable the gameobject    
    /// </summary>
    public void ReturnToSpawner()
    {
        rigidbody2D.velocity = Vector2.zero;
        used = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Launches the diamond from the given position with the given velocity
    /// Only launches them straight up or down depending on the sign of speed
    /// </summary>
    /// <param name="speed">positive numbers to launch the diamond down, negative up</param>
    /// <param name="position">Position of a track from which the diamond launches</param>
    public void Launch(float speed, Vector3 position) {        
        used = true;
        gameObject.SetActive(true);
        transform.position = position;
        rigidbody2D.velocity = -Vector2.up * speed;
        ReScale();
        Randomize();
    }

    

}
