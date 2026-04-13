using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main S { get; private set; }
    private static bool gamePaused = false;
    public static Player p;

    [Header("Inscribed")]
    [SerializeField]
    private GameObject pauseScreen;

    public static bool GamePaused
    {
        get {  return gamePaused; }
        private set { gamePaused = value; }
    }

    public static float CheckRotation(float rotation)
    {
        while (rotation < -180)
            rotation += 360;
        while (rotation >= 180)
            rotation -= 360;
        return rotation;
    }

    void Awake()
    {
        S = this;
        p = FindAnyObjectByType<Player>();
        pauseScreen.SetActive(GamePaused);
    }

    // Update is called once per frame
    void Update()
    {
        if (p.playAs.status.HasFlag(Character.Status.IsDefeated))
            GameOver();

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        pauseScreen.SetActive(GamePaused);
    }

    public static void Pause()
    {
        if (!GamePaused)
        {
            Time.timeScale = 0;
            GamePaused = true;
        }
        else
        {
            Time.timeScale = 1;
            GamePaused = false;
        }
    }

    public static void GameOver()
    {

    }
}
