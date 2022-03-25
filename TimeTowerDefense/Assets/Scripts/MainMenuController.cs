using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnStartPressed() {
        if (!PlayerPrefs.HasKey("FurthestLevel"))
            PlayerPrefs.SetInt("FurthestLevel", 0);
        SceneManager.LoadScene("GameScene");
    }

    public void OnSelectPressed() {

    }

    public void OnQuitPressed() {
        Application.Quit();
    }
}
