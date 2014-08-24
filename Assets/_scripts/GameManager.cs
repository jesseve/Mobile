using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    //Constant for scaling ui-elements
    public State startState;
    public static readonly Vector2 screenScale = new Vector2(1920, 1080);
    public delegate void ResolutionChanged();
    public static event ResolutionChanged resolutionChanged = delegate { };
    private int screenWidth;
    private int screenHeight;

    protected State e_state;

    

    public virtual void Awake()
    {        

        screenWidth = Screen.width;
        screenHeight = Screen.height;
        if (GameObject.FindGameObjectsWithTag("Manager").Length > 1)
            Debug.Log("More than 1 manager in the scene!");
        SetupManager();
        SetState(startState);
    }

    public virtual void Update()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            resolutionChanged();
        }
    }
    
    protected virtual void SetupManager(){
        
    }

    /// <summary>
    /// Sets the current state of the GameManager.
    /// </summary>
    /// <param name="state">State.</param>
    public void SetState(State state)
    {
        e_state = state;
    }

    /// <summary>
    /// Adds the state to the current state of the GameManager.
    /// </summary>
    /// <param name="state">State.</param>
    public void AddState(State state)
    {
        e_state |= state;
    }

    /// <summary>
    /// Gets the current state.
    /// </summary>
    /// <returns>The state.</returns>
    public State GetState()
    {
        return e_state;
    }

    /// <summary>
    /// Removes the given state from the current state.If the state is not contain, nothing happens.
    /// </summary>
    /// <param name="state">State.</param>
    public void RemoveState(State state)
    {
        e_state &= ~state;
    }

    /// <summary>
    /// Checks if the state is contained in the current state.
    /// </summary>
    /// <returns><c>true</c>, if the state is contained, <c>false</c> otherwise.</returns>
    /// <param name="state">State.</param>
    public bool CheckForState(State state)
    {
        if ((e_state & state) == state)
        {
            return true;
        }
        return false;
    }
}

[Flags]
public enum State
{
    Menu,
    Pause,
    Running,    
    GameOver
}
public enum RunningState
{ 
    Normal,
    Special
}

public enum Shape
{
    Triangle,
    Square,
    Circle,
    Diamond,
    Hexagon
}