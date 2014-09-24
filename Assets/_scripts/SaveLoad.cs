using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

    /**** THIS STILL NEED METHODS FOR SAVING ONLY INVENTORY OR LEVELS. ALSO NEED TO OPTIMIZE THE CODE ****/

    public static Game current;

    private static string fileName = Application.persistentDataPath + "/progress.hamburger";    //Name of the file to save to

    /// <summary>
    /// Creates a new game and saves it to a file
    /// </summary>
    public static void CreateGame() {
        current = new Game();
        Save();
    }

    /// <summary>
    /// Saves the progress on levels and updates the inventory of the current game with players inventory
    /// </summary>
    public static void Save() {
        for (int i = 0; i < current.unlockedLevels.Length; i++) {
            current.unlockedLevels[i] = LevelSelect.instance.levels[i].unlocked;
        }
        current.inventory.UpdateInventory(Initializer.instance.player.inventory);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(fileName); //you can call it anything you want
        bf.Serialize(file, SaveLoad.current);
        file.Close();
    }

    /// <summary>
    /// Loads the progress from file if the files exists
    /// otherwise it creates a game and the file
    /// </summary>
    public static void Load() {        
        if (File.Exists(fileName)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);            
            try {
                SaveLoad.current = (Game)bf.Deserialize(file);
            } catch (System.Runtime.Serialization.SerializationException e) {
                if (e != null) {
                    file.Close();
                    CreateGame();
                }
            }
            file.Close();
            Initializer.instance.player.inventory.UpdateInventory(current.inventory);
            for (int i = 0; i < current.unlockedLevels.Length; i++) {
                LevelSelect.instance.levels[i].unlocked = current.unlockedLevels[i];
            }
            return;
        }
        else
            CreateGame();        
    }
	
}

//Contains all stuff needed to save
[System.Serializable]
public class Game {
    
    public bool[] unlockedLevels;
    public Inventory inventory;

    public Game() {
        unlockedLevels = new bool[LevelSelect.instance.levels.Length];
        for (int i = 0; i < unlockedLevels.Length; i++) {
            unlockedLevels[i] = LevelSelect.instance.levels[i].unlocked;
        }
        inventory = new Inventory();
    }
}