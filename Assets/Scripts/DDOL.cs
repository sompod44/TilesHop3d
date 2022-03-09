using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL instance;
    public bool gamePause;
    public bool gameStart;
    private void Awake()
    {
        InitVariables();
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void InitVariables()
    {
        gameStart = false;
        gamePause = false;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            instance = null;
            instance = this;
        }
        // Here I set the game's frame gate. 
        Application.targetFrameRate = 60;
    }

}
