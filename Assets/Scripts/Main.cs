using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main S;
    public static bool gamePaused = false;

    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Pause()
    {
        if (!gamePaused)
            Time.timeScale = 0;

    }
    public static void Zoom()
    {

    }
}
