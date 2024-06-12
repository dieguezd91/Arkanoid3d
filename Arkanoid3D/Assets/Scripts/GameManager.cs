using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager UIManager => uiManager;
    [SerializeField] UIManager uiManager;
    GameState _currentState;
    float _elapsedTime;
    public float ElapsedTime => _elapsedTime;
    public bool isGameRunning;

    [Header("BALL")]
    [SerializeField] public Transform ballInitPos;
    [SerializeField] List<Ball> _balls;
    public List<Ball> Balls => _balls;

    [Header("PLAYER")]
    [SerializeField] Transform playerInitPos;
    public GameObject Player => _player;
    GameObject _player;
    PlayerController _playerScript;
    public PlayerController PlayerController => _playerScript;

    [Header("BRICKS")]
    [SerializeField] public GameObject[] Bricks;
    [SerializeField] public int bricksLeft;

    [Header("LIFE MANAGEMENT")]
    public int initLives;
    public int Lives => _lives;
    int _lives;

    private void Start()
    {
        instance = this;

        GetActors();
        uiManager.Initialize();
    }

    void Update()
    {
        if (_currentState == GameState.Playing)
        {
            if (isGameRunning)
            {
                _elapsedTime += Time.deltaTime;
                _playerScript.UpdatePlayer();
                if (bricksLeft == 0) Win();

                if (UpgradePool.instance.upgradePool.Count > 0)
                {
                    for (int i = UpgradePool.instance.upgradePool.Count - 1; i >= 0; i--)
                    {
                        UpgradePool.instance.upgradePool[i].GetComponent<Upgrade>().UpdateUpgrade();
                    }
                }

                if (_balls.Count > 0)
                {
                    for (int i = _balls.Count - 1; i >= 0; i--)
                    {
                        _balls[i].UpdateBall();
                    }
                }
                else LoseRound();

                if (Input.GetKeyDown(KeyCode.Escape)) Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) StartGame();

            uiManager.UpdateHUD();
        }
        else if (_currentState == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Play();
        }
    }

    public void Play()
    {
        Time.timeScale = 1;
        _currentState = GameState.Playing;
        uiManager.PlayUI();
    }

    public void Pause()
    {
        _currentState = GameState.Paused;
        Time.timeScale = 0;
        uiManager.Pause();
    }

    public void MainMenu()
    {
        isGameRunning = false;
        _elapsedTime = 0;
        _currentState = GameState.MainMenu;
        uiManager.MainMenuUI();
    }

    public void LoadGame()
    {
        SetGame();
        Play();
    }

    void StartGame()
    {
        _elapsedTime = 0; // Reiniciar el tiempo transcurrido al inicio del juego
        BallPool.instance.AddBallsToPool(BallPool.instance.poolsize);
        CreateInitBall();
        isGameRunning = true;
    }

    void SetGame()
    {
        RestartPositions();
        _lives = initLives;
    }

    public void LoseRound()
    {
        if (_lives > 0)
        {
            RestartPositions();
            _lives--;
        }
        else Lose();
    }

    void Win() => MainMenu();

    void Lose() => MainMenu();

    void GetActors()
    {
        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        _playerScript = _player.GetComponent<PlayerController>();

        //Bricks
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
        bricksLeft = Bricks.Length;
    }

    public void RestartPositions()
    {
        isGameRunning = false;
        Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
    }

    void CreateInitBall()
    {
        Ball initBall = BallPool.instance.RequestBall().GetComponent<Ball>();
        initBall.gameObject.SetActive(true);
        initBall.transform.position = ballInitPos.position;
        _balls.Add(initBall);
        initBall.SetNewDirection();
    }

    public void QuitGame() => Application.Quit();
}
