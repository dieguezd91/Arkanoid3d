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

    [Header("BALLS")]
    [SerializeField] public Transform ballInitPos;
    [SerializeField] int maxBalls;
    public BallPool BallPool => _ballPool;
    [SerializeField] BallPool _ballPool;
    public List<Ball> Balls => _balls;
    [SerializeField] List<Ball> _balls;

    [Header("PLAYER")]
    [SerializeField] Transform playerInitPos;
    public GameObject Player => _player;
    GameObject _player;
    PlayerController playerScript;

    [Header("BRICKS")]
    GameObject[] Spawns;
    [SerializeField] GameObject BrickPrefab;
    [SerializeField] public int bricksLeft;

    //Upgrades
    public List<Upgrade> Upgrades => _upgrades;
    List<Upgrade> _upgrades;

    [Header("LIFE MANAGEMENT")]
    public int initLives;
    public int Lives => _lives;
    int _lives;

    [Header("MUSIC AND SFX")]
    [SerializeField] AudioClip _WinSFX;
    [SerializeField] AudioClip _LossSFX;
    [SerializeField] AudioClip _RoundLostSFX;
    [SerializeField] AudioClip _PlaySFX;
    [SerializeField] AudioClip _PauseSFX;
    AudioSource _audioSource;

    private void Start()
    {
        instance = this;

        GetActors();
        uiManager.Initialize();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_currentState == GameState.Playing)
        {
            if (isGameRunning)
            {
                _elapsedTime += Time.deltaTime;
                playerScript.UpdatePlayer();
                if (bricksLeft == 0) Win();

                if (_upgrades.Count > 0)
                    for (int i = _upgrades.Count - 1; i >= 0; i--)
                        _upgrades[i].UpdateUpgrade(); 

                if (_balls.Count > 0)
                    for (int i = _balls.Count - 1; i >= 0; i--)
                        _balls[i].UpdateBall(); 
                else LoseRound();

                if (Input.GetKeyDown(KeyCode.Escape)) Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) StartGame();

            uiManager.UpdateHUD();
        }
        else if (_currentState == GameState.Paused)
            if (Input.GetKeyDown(KeyCode.Escape)) Play(); 
    }

    public void Play()
    {
        Time.timeScale = 1;
        _currentState = GameState.Playing;
        uiManager.PlayUI();
        _audioSource.PlayOneShot(_PlaySFX);
    }

    public void Pause()
    {
        _currentState = GameState.Paused;
        Time.timeScale = 0;
        uiManager.Pause();
        _audioSource.PlayOneShot(_PauseSFX);
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
            _audioSource.PlayOneShot(_RoundLostSFX);
        }
        else Lose();
    }

    void Win()
    {
        _audioSource.PlayOneShot(_WinSFX);
        SetGame();
        MainMenu();
    }

    void Lose()
    {
        _audioSource.PlayOneShot(_LossSFX);
        SetGame();
        MainMenu();
    }

    void GetActors()
    {
        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        playerScript = _player.GetComponent<PlayerController>();

        //Bricks
        Spawns = GameObject.FindGameObjectsWithTag("BrickSpawn");

        //Upgrades
        _upgrades = new List<Upgrade>();

        //Ball pool
        _ballPool.Initialize(maxBalls);
    }

    public void RestartPositions()
    {
        isGameRunning = false;
        Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);

        bricksLeft = 0;
        foreach (GameObject spawn in Spawns)
        {
            Brick newBrick = Instantiate(BrickPrefab, spawn.transform).GetComponent<Brick>();
            switch (spawn.name)
            {
                case "Red Brick Spawn":
                    newBrick.color = "Red";
                    break;
                case "Orange Brick Spawn":
                    newBrick.color = "Orange";
                    break;
                case "Yellow Brick Spawn":
                    newBrick.color = "Yellow";
                    break;
                default:
                    newBrick.color = "Yellow";
                    break;
            }
            bricksLeft++;
        }

        foreach (Ball ball in _balls)
        {
            ball.gameObject.SetActive(false);
            _balls.Remove(ball);
        }
        _balls.Clear();

        foreach (Upgrade upgrade in _upgrades)
        {
            upgrade.gameObject.SetActive(false);
            _upgrades.Remove(upgrade);
        }
        _upgrades.Clear();
    }

    void CreateInitBall()
    {
        Ball initBall = _ballPool.RequestBall().GetComponent<Ball>();
        initBall.gameObject.SetActive(true);
        initBall.transform.position = ballInitPos.position;
        _balls.Add(initBall);
        initBall.SetNewDirection();
    }

    public void QuitGame() => Application.Quit();
}
