using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    VideoPlayer bgVideo;
    [Header("UI COMPONENTS")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _hud;

    [Header("HUD COMPONENTS")]
    [SerializeField] RectTransform _lives;
    [SerializeField] TextMeshProUGUI _timer;

    public void Initialize()
    {
        gameManager = GameManager.instance;
        bgVideo = GetComponentInChildren<VideoPlayer>();
        bgVideo.targetCamera = Camera.main;
        MainMenuUI();
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        _hud.SetActive(false);
    }

    public void PlayUI()
    {
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(false);
        _hud.SetActive(true);
    }

    public void MainMenuUI()
    {
        _mainMenu.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    public void UpdateHUD()
    {
        //TIMER
        int minutes = Mathf.FloorToInt(gameManager.ElapsedTime / 60);
        int seconds = Mathf.FloorToInt(gameManager.ElapsedTime % 60);
        _timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        _lives.rect.Set(_lives.rect.x, _lives.rect.y, 100 * gameManager.Lives, _lives.rect.y);
    }
}
