using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public VariableJoystick joystick;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        SetActiveGameOverMenu(false);
        joystick = VariableJoystick.Instance;
        joystick.SetActive(true);
    }

    public void GameOver()
    {
        SetActiveGameOverMenu(true);
        joystick.SetActive(false);
    }

    public void SetActiveGameOverMenu(bool active)
    {
        gameOverMenu.SetActive(active);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}