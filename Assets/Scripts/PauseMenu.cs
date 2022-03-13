using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public static bool Weapon = false;

    public GameObject pauseUI;
    public GameObject weaponUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Weapon)
            {
                Continue();
            }
            else
            {
                Stop();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    private void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Continue()
    {
        weaponUI.SetActive(false);
        Time.timeScale = 1f;
        Weapon = false;
    }

    private void Stop()
    {
        weaponUI.SetActive(true);
        Time.timeScale = 0f;
        Weapon = true;
    }
}
