using UnityEngine;
using System.Collections;

public class MenuManager : GameManager {

    public static MenuManager instance
    {
        get;
        private set;
    }    

    protected override void SetupManager()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
}
