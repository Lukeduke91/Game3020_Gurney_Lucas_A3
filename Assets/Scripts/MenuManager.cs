using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
    public void Instruction()
    {
        SceneManager.LoadScene("ControllerScreen");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
