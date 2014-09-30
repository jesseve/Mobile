using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfo : MonoBehaviour {

    public Text levelName, bestScore, combo;    

    void Start() {
        LevelSelect.instance.levelChanged += ChangeValues;
        LevelManager.instance.gameOver += ChangeValues;
        ChangeValues();
    }

    private void ChangeValues() {
        levelName.text = LevelSelect.instance.currentLevel.name;
        bestScore.text = LevelSelect.instance.currentLevel.bestScore.ToString();
        combo.text = LevelSelect.instance.currentLevel.highestCombo.ToString();
    }
}
