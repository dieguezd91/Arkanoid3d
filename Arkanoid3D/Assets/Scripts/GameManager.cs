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
    PlayerController _playerScript;
    public PlayerController PlayerController => _playerScript;

    [Header("BRICKS")]
    GameObject[] Spawns;
    public List<GameObject> Bricks => _bricks;
    List<GameObject> _bricks;
    [SerializeField] GameObject BrickPrefab;
    [SerializeField] public int bricksLeft;

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
                if (Input.GetKeyDown(KeyCode.Escape)) Pause(); 
                _elapsedTime += Time.deltaTime;
                _playerScript.UpdatePlayer();
                if (bricksLeft == 0) Win();
                if(_balls.Count <= 0) LoseRound();
                for (int i = 0; i < _upgrades.Count; i++) _upgrades[i].UpdateUpgrade();
                for (int i = 0; i < _balls.Count; i++) _balls[i].UpdateBall(); 

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
        ClearBricks();
        ClearUpgrades();
        ClearBalls();
    }

    public void LoadGame()
    {
        _lives = initLives;
        RestartPositions();
        CreateBricks();
        Play();
    }

    void StartGame()
    {
        _elapsedTime = 0; // Reiniciar el tiempo transcurrido al inicio del juego
        CreateInitBall();
        isGameRunning = true;
    }

    public void LoseRound()
    {
        _lives--;
        if (_lives > 0)
        {
            RestartPositions();
            _audioSource.PlayOneShot(_RoundLostSFX);
        }
        else Lose();
    }

    void Win()
    {
        _audioSource.PlayOneShot(_WinSFX);
        MainMenu();
    }

    void Lose()
    {
        _audioSource.PlayOneShot(_LossSFX);
        MainMenu();
    }

    void GetActors()
    {
        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        _playerScript = _player.GetComponent<PlayerController>();

        //Bricks
        Spawns = GameObject.FindGameObjectsWithTag("BrickSpawn");
        _bricks = new List<GameObject>();

        //Upgrades
        _upgrades = new List<Upgrade>();

        //Ball pool
        _ballPool.Initialize(maxBalls);
    }

    public void RestartPositions()
    {
        isGameRunning = false;
        Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
    }

    void ClearUpgrades()
    {
        foreach (GameObject upgrade in GameObject.FindGameObjectsWithTag("Upgrade")) Destroy(upgrade);
        _upgrades.Clear();
        if (playerScript.IsBarExpanded) playerScript.ManageBarSize(2);
        if (playerScript.IsMagnetActive) playerScript.ManageMagnetState();
    }

    void ClearBalls()
    {
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) ball.SetActive(false);
        _balls.Clear();
    }

    void CreateInitBall()
    {
        Ball initBall = _ballPool.RequestBall().GetComponent<Ball>();
        initBall.transform.position = ballInitPos.position;
        initBall.SetNewDirection();
    }

    void ClearBricks()
    {
        foreach (GameObject brick in _bricks) Destroy(brick);
        _bricks.Clear();
        bricksLeft = 0;
    }

    void CreateBricks()
    {
        foreach (GameObject spawn in Spawns)
        {
            Brick newBrick = Instantiate(BrickPrefab, spawn.transform).GetComponent<Brick>();
            _bricks.Add(newBrick.gameObject);
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
    }

    public void QuitGame() => Application.Quit();
}
