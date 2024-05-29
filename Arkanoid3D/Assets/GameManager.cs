using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int lifes;

    [Header("BALL")]
    GameObject _ball;
    public GameObject Ball => _ball;
    Ball ballScript;
    [SerializeField] Transform ballInitPos;
    bool isGameRunning;

    [Header("PLAYER")]
    GameObject _player;
    public GameObject Player => _player;
    PlayerController playerScript;
    [SerializeField] Transform playerInitPos;


    [Header("BRICKS")]
    GameObject[] Bricks;
    int bricksLeft;

    [Header("LIFE MANAGEMENT")]
    public int initLives;
    int _lives;
    public int Lives => _lives;

    //UPGRADES MANAGEMENT
    List<GameObject> extraBalls = new List<GameObject>();
    public List<GameObject> ExtraBalls => extraBalls;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        //Ball
        _ball = GameObject.FindGameObjectWithTag("Ball");
        ballInitPos = GameObject.Find("BallInitPos").GetComponent<Transform>();
        ballScript = _ball.GetComponent<Ball>();

        //Player
        _player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        playerScript = _player.GetComponent<PlayerController>();

        //Bricks
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
    }

    void Update()
    {
        if(isGameRunning)
        { 
            ballScript.UpdateBall();
            playerScript.UpdatePlayer();
            bricksLeft = Bricks.Length;
            if (bricksLeft == 0) Win();
            if(extraBalls.Count > 0) foreach(GameObject extraBall in extraBalls) extraBall.GetComponent<Ball>().UpdateBall();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) isGameRunning = true;
    }

    public void Lose()
    {
        Debug.Log("Perdiste");
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
        isGameRunning = false;
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

    public void RestartPositions()
    {
        isGameRunning = false;

        _ball.transform.SetPositionAndRotation(ballInitPos.position, ballInitPos.rotation);
        ballScript.SetNewDirection();

        _player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
    }
}