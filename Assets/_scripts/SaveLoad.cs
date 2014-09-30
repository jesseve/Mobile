using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {    

    public static Game current;

    public static bool firstTime;

    private static string fileName = Application.persistentDataPath + "/progress.hamburger";    //Name of the file to save to

    /// <summary>
    /// Creates a new game and saves it to a file
    /// </summary>
    public static void CreateGame() {
        firstTime = true;
        current = new Game();
        Save();
    }

    /// <summary>
    /// Saves the progress on levels and updates the inventory of the current game with players inventory
    /// </summary>
    public static void Save() {
        if (current.unlockedLevels.Length != LevelSelect.instance.levels.Length)
        {
            current = new Game();
        }
        for (int i = 0; i < current.unlockedLevels.Length; i++) {
            current.unlockedLevels[i] = LevelSelect.instance.levels[i].unlocked;
            current.highestCombos[i] = LevelSelect.instance.levels[i].highestCombo;
            current.bestScores[i] = LevelSelect.instance.levels[i].bestScore;
        }
        current.musicMuted = MusicHandler.instance.muted;
        current.soundMuted = SoundHandler.instance.muted;
        current.inventory.UpdateInventory(Initializer.instance.player.inventory);
        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream file = File.Create(fileName);
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
            if (MusicHandler.instance.muted != current.musicMuted)
                MusicHandler.instance.Mute();
            if (SoundHandler.instance.muted != current.soundMuted)
                SoundHandler.instance.Mute();
            Initializer.instance.player.inventory.UpdateInventory(current.inventory);
            for (int i = 0; i < current.unlockedLevels.Length; i++) {
                LevelSelect.instance.levels[i].unlocked = current.unlockedLevels[i];
                LevelSelect.instance.levels[i].highestCombo = current.highestCombos[i];
                LevelSelect.instance.levels[i].bestScore = current.bestScores[i];
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
    public int[] highestCombos;
    public int[] bestScores;
    public Inventory inventory;

    public bool soundMuted = false;
    public bool musicMuted = false;

    public Game() {
        unlockedLevels = new bool[LevelSelect.instance.levels.Length];
        highestCombos = new int[LevelSelect.instance.levels.Length];
        bestScores = new int[LevelSelect.instance.levels.Length];

        for (int i = 0; i < unlockedLevels.Length; i++) {
            unlockedLevels[i] = LevelSelect.instance.levels[i].unlocked;            
            highestCombos[i] = LevelSelect.instance.levels[i].highestCombo;
            bestScores[i] = LevelSelect.instance.levels[i].bestScore;            
        }
        inventory = new Inventory();
    }
}