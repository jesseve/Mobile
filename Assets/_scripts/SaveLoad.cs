using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
#if UNITY_ANDROID //|| UNITY_EDITOR
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.IO;

public static class SaveLoad {    

    public static Game current;

    public static bool firstTime;

    private static string fileName = Application.persistentDataPath + "/progress.hamburger";    //Name of the file to save to
#if UNITY_ANDROID //|| UNITY_EDITOR
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
#endif

#if UNITY_WP8 || UNITY_METRO
    private static string sound = "sound", music = "music", unlockedLevel = "unlock", combo = "combo", score = "score", money = "money";


    private static void CreateGame() {
        firstTime = true;
        current = new Game();
        Save();
    }

    public static void Save() {
        if (current == null) {
            CreateGame();
        }
        if (current.unlockedLevels.Length != LevelSelect.instance.levels.Length) {
            current = new Game();
        }
        current.inventory.UpdateInventory(Initializer.instance.player.inventory);
        for (int i = 0; i < LevelSelect.instance.levels.Length; i++) {
            current.inventory.UpdateInventory(Initializer.instance.player.inventory);
            PlayerPrefs.SetInt(score + i.ToString(), LevelSelect.instance.levels[i].bestScore);
            PlayerPrefs.SetInt(combo + i.ToString(), LevelSelect.instance.levels[i].highestCombo);
            if(i > 0)
                SetBool(unlockedLevel + i.ToString(), LevelSelect.instance.levels[i].unlocked);            
        }
        SetGame();
    }
    public static void Load() {
        if (current == null)
        {
            CreateGame();
        }
        GetGame();
        Initializer.instance.player.inventory.UpdateInventory(current.inventory);
        for (int i = 0; i < LevelSelect.instance.levels.Length; i++) {
            if (PlayerPrefs.HasKey(score + i.ToString())) {
                LevelSelect.instance.levels[i].bestScore = PlayerPrefs.GetInt(score + i.ToString());
            }
            else {
                CreateGame();
                return;
            }
            if (PlayerPrefs.HasKey(combo + i.ToString())) {
                LevelSelect.instance.levels[i].highestCombo = PlayerPrefs.GetInt(combo + i.ToString());
            }
            if (PlayerPrefs.HasKey(unlockedLevel + i.ToString()) && i > 0) {
                LevelSelect.instance.levels[i].unlocked = GetBool(unlockedLevel + i.ToString());
            }            
        }

    }

    private static void SetBool(string name, bool value) {
        PlayerPrefs.SetInt(name, value ? 1 : 0);
    }
    private static bool GetBool(string name) {
        if (PlayerPrefs.HasKey(name))
        {
            return PlayerPrefs.GetInt(name) == 1;
        }
        return false;
    }

    private static void SetGame() {
        PlayerPrefs.SetInt(money, current.inventory.money);
        PlayerPrefs.SetFloat("speed", current.inventory.speed);
        PlayerPrefs.SetFloat("shieldCooldown", current.inventory.shieldCooldown);
        PlayerPrefs.SetFloat("position", current.inventory.position);
        PlayerPrefs.SetFloat("acceleration", current.inventory.acceleration);
        PlayerPrefs.SetInt("health", current.inventory.health);
        SetBool("shield", current.inventory.shield);
        current.musicMuted = MusicHandler.instance.muted;
        current.soundMuted = SoundHandler.instance.muted;
        SetBool(sound, current.soundMuted);
        SetBool(music, current.musicMuted);
    }

    private static void GetGame() {
        if(PlayerPrefs.HasKey(money))
            current.inventory.money = PlayerPrefs.GetInt(money);
        if (PlayerPrefs.HasKey("shield"))
            current.inventory.shield = GetBool("shield");
        if (PlayerPrefs.HasKey("speed"))
            current.inventory.speed = PlayerPrefs.GetFloat("speed");
        if (PlayerPrefs.HasKey("shieldCooldown"))
            current.inventory.shieldCooldown = PlayerPrefs.GetFloat("shieldCooldown");
        if (PlayerPrefs.HasKey("position"))
            current.inventory.position = PlayerPrefs.GetFloat("position");
        if (PlayerPrefs.HasKey("acceleration"))
            current.inventory.acceleration = PlayerPrefs.GetFloat("acceleration");
        if (PlayerPrefs.HasKey("health"))
            current.inventory.health = PlayerPrefs.GetInt("health");

        if(PlayerPrefs.HasKey(music)) {
            current.musicMuted = GetBool(music);
            if (MusicHandler.instance.muted != current.musicMuted)
                MusicHandler.instance.Mute();
        }
        if(PlayerPrefs.HasKey(sound)) {
            current.soundMuted = GetBool(sound);
            if (SoundHandler.instance.muted != current.soundMuted)
                SoundHandler.instance.Mute();
        
        }              

    }

#endif
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
