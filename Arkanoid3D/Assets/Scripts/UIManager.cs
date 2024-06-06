using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _hud;
    GameManager gameManager;
    VideoPlayer bgVideo;

    public void Initialize()
    {
        gameManager = GameManager.instance;
        bgVideo.targetCamera = Camera.main;
        MainMenu();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _hud.SetActive(false);
    }

    public void Play()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(false);
        _hud.SetActive(true);
    }
    public void MainMenu()
    {
        _mainMenu.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        GameManager.instance.QuitGame();
    }

    public void UpdateHUD() { }
}
