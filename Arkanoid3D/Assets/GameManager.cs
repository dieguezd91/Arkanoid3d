using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("BALL")]
    [SerializeField] Transform ballInitPos;
    [SerializeField] GameObject ballPrefab;

    public GameObject Ball => _ball;
    GameObject _ball;
    Ball ballScript;
    bool isGameRunning;
    [SerializeField] List<GameObject> _balls;
    public List<GameObject> Balls => _balls;
    [SerializeField] int maxBalls;

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

    //UPGRADES MANAGEMENT
    List<GameObject> extraBalls = new List<GameObject>();
    public List<GameObject> ExtraBalls => extraBalls;

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
        }
        else if (Input.GetKeyDown(KeyCode.Space)) StartGame();
    }

    void StartGame()
    {
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
        GameObject initBall = Instantiate(ballPrefab, ballInitPos.position, ballInitPos.rotation);
        _balls.Add(initBall);
        initBall.GetComponent<Ball>().SetNewDirection();
    }
}