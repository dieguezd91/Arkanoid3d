using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public BallPool BallPool => _ballPool;
    [SerializeField] BallPool _ballPool;
    public List<Ball> Balls => _balls;
    [SerializeField] List<Ball> _balls;

    [Header("PLAYER")]
    [SerializeField] Transform playerInitPos;
    public GameObject Player => _player;
    GameObject _player;
    public PlayerController PlayerScript => playerScript;
    PlayerController playerScript;

    [Header("BRICKS")]
    GameObject[] Spawns;
    public List<GameObject> Bricks => _bricks;
    List<GameObject> _bricks;
    [SerializeField] GameObject BrickPrefab;
    [SerializeField] public int bricksLeft;

    //UPGRADES
    public List<Upgrade> Upgrades => _upgrades;
    List<Upgrade> _upgrades;
    public UpgradePool UpgradePool => _upgradePool;
    [SerializeField] UpgradePool _upgradePool;

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
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip mainMenuSong;
    [SerializeField] AudioClip gameplaySong;
    [SerializeField] AudioSource _musicSource;

    [SerializeField] Light _light;
    private Color _originalLightColor;
    bool _missedPoint;
    float _restoreLightTimer;

    private void Start()
    {
        instance = this;
        uiManager.Initialize();
        _originalLightColor = _light.color;
    }

    void Update()
    {
        if (_currentState == GameState.Playing)
        {
            if (isGameRunning)
            {
                if (Input.GetKeyDown(KeyCode.Escape)) Pause();
                _elapsedTime += Time.deltaTime;
                if (playerScript != null)
                {
                    playerScript.UpdatePlayer();
                }

                if (bricksLeft == 0) Win();
                if (_balls.Count <= 0) LoseRound();
                for (int i = 0; i < _upgrades.Count; i++) _upgrades[i].UpdateUpgrade();
                for (int i = 0; i < _balls.Count; i++) _balls[i].UpdateBall();

            }
            else if (Input.GetKeyDown(KeyCode.Space) && !_missedPoint) isGameRunning = true;

            uiManager.UpdateHUD();
        }
        else if (_currentState == GameState.Paused)
            if (Input.GetKeyDown(KeyCode.Escape)) Play();

        if (_missedPoint)
        {
            _restoreLightTimer += Time.deltaTime;
            if (_restoreLightTimer >= 1f)
            {
                _light.color = _originalLightColor;
                _missedPoint = false;
                _restoreLightTimer = 0f;
            }
        }

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
        ChangeSong(mainMenuSong);
        isGameRunning = false;
        _currentState = GameState.MainMenu;
        uiManager.MainMenuUI();
        ClearBricks();
        ClearUpgrades();
        ClearBalls();
        RemoveActors();
    }

    public void LoadGame()
    {
        GetActors();
        ChangeSong(gameplaySong);
        _lives = initLives;
        _elapsedTime = 0;
        RestartPositions();
        CreateBricks();
        Play();
    }

    public void LoseRound()
    {
        _lives--;
        _light.color = Color.red;
        _missedPoint = true;
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
        isGameRunning = false;
        uiManager._hud.SetActive(false);
        uiManager._winScreen.SetActive(true);
    }

    void Lose()
    {
        _audioSource.PlayOneShot(_LossSFX);
        isGameRunning = false;
        uiManager._hud.SetActive(false);
        uiManager._gameOverScreen.SetActive(true);
    }

    void GetActors()
    {
        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
            playerScript = _player.GetComponent<PlayerController>();
        }

        //Bricks
        Spawns = GameObject.FindGameObjectsWithTag("BrickSpawn");
        _bricks = new List<GameObject>();

        //Upgrades
        _upgrades = new List<Upgrade>();

        //Ball pool
        _ballPool.Initialize();
        _upgradePool.Initialize();
    }

    void RemoveActors()
    {
        //Player
        _player = null;
        playerInitPos = null;
        playerScript = null;

        //Bricks
        Spawns = null;
        _bricks = null;

        //Upgrades
        _upgrades = null;
    }

    public void RestartPositions()
    {
        //_missedPoint = false;
        isGameRunning = false;
        if (Player != null && playerInitPos != null)
            Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
        CreateInitBall();
    }

    void ClearUpgrades()
    {
        foreach (GameObject upgrade in GameObject.FindGameObjectsWithTag("Upgrade")) upgrade.SetActive(false);
        _upgrades.Clear();
        if (playerScript != null)
        {
            if (playerScript.IsBarExpanded) playerScript.ManageBarSize(2);
            if (playerScript.IsMagnetActive) playerScript.ManageMagnetState();
        }
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
        Spawns.ToList().Clear();
    }

    private void ChangeSong(AudioClip newSong)
    {
        _musicSource.Stop();
        _musicSource.PlayOneShot(newSong);
    }

    public void QuitGame() => Application.Quit();
}
