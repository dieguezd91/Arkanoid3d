using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager UIManager => uiManager;
    [SerializeField] UIManager uiManager;


    [Header("BALL")]
    [SerializeField] public Transform ballInitPos;

    public bool isGameRunning;
    [SerializeField] List<GameObject> _balls;
    public List<GameObject> Balls => _balls;

    [Header("PLAYER")]
    [SerializeField] Transform playerInitPos;
    public GameObject Player => _player;
    GameObject _player;
    PlayerController playerScript;


    [Header("BRICKS")]
    [SerializeField] public GameObject[] Bricks;
    [SerializeField] public int bricksLeft;


    [Header("LIFE MANAGEMENT")]
    public int initLives;
    public int Lives => _lives;
    int _lives;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        GetActors();
        bricksLeft = Bricks.Length;
    }

    void Update()
    {
        if (isGameRunning)
        {
            playerScript.UpdatePlayer();
            if (bricksLeft == 0) Win();
            if (_balls.Count > 0) foreach (GameObject ball in _balls) ball.GetComponent<Ball>().UpdateBall();
            else LoseRound();
            if (Input.GetKeyDown(KeyCode.Escape)) uiManager.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) StartGame();
    }

    void StartGame()
    {
        BallPool.instance.AddBallsToPool(BallPool.instance.poolsize);
        CreateInitBall();
        isGameRunning = true;
    }

    void SetGame()
    {
        RestartPositions();

        _lives = initLives;
        Debug.Log("Game Set");
    }

    void Win()
    {
        Debug.Log("You won");
        SetGame();
    }
    public void LoseRound()
    {
        if (_lives > 0)
        {
            Debug.Log("You lost");
            RestartPositions();
            _lives--;
        }
        else EndGame();
    }
    void EndGame()
    {
        Debug.Log("Game lost");
        SetGame();
    }

    void GetActors()
    {
        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        playerScript = _player.GetComponent<PlayerController>();

        //Bricks
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
    }

    public void RestartPositions()
    {
        isGameRunning = false;

        Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
    }

    void CreateInitBall()
    {
        GameObject initBall = BallPool.instance.RequestBall();
        initBall.SetActive(true);
        initBall.transform.position = ballInitPos.position;
        _balls.Add(initBall);
        initBall.GetComponent<Ball>().SetNewDirection();
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Quit");
#endif
    }
}