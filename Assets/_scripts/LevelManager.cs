using UnityEngine;
using System.Collections;

public class LevelManager : GameManager {

    public static LevelManager instance
    {
        get;
        private set;
    }

    public float borderPanelWidth;

    public float GameAreaWidth{
        get {
            return gameAreaWidth;
        }
    }
    public float GameAreaWidthHalf
    {
        get
        {
            return gameAreaWidthHalf;
        }
    }
    private float gameAreaWidth;
    private float gameAreaWidthHalf;

    public override void Awake()
    {
        base.Awake();
        gameAreaWidth = (Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x) * (100f - borderPanelWidth * 2f) * 0.01f; // still need to substract the width of player here
        gameAreaWidthHalf = gameAreaWidth * 0.5f;
        Debug.Log(gameAreaWidth);
    }

    protected override void SetupManager()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
}
