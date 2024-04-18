using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("BALL")]
    GameObject Ball;
    Ball ballScript;
    [SerializeField] Transform ballInitPos;
    bool isGameRunning;


    [Header("PLAYER")]
    GameObject Player;
    PlayerController playerScript;
    [SerializeField] Transform playerInitPos;


    [Header("BRICKS")]
    GameObject[] Bricks;
    int bricksLeft;

    public int initLives;
    int _lives;
    public int Lives => _lives;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        GetActors();
        SetGame();
    }

    private void Update()
    {
        if(isGameRunning)
        { 
            ballScript.UpdateBall();
            playerScript.UpdatePlayer();
            bricksLeft = Bricks.Length;
            if (bricksLeft == 0) Win();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) isGameRunning = true;
    }

    void GetActors()
    {
        //Ball
        Ball = GameObject.FindGameObjectWithTag("Ball");
        ballInitPos = GameObject.Find("BallInitPos").GetComponent<Transform>();
        ballScript = Ball.GetComponent<Ball>();

        //Player
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = GameObject.Find("PlayerInitPos").GetComponent<Transform>();
        playerScript = Player.GetComponent<PlayerController>();

        //Bricks
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
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

    public void LooseRound()
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

        Ball.transform.SetPositionAndRotation(ballInitPos.position, ballInitPos.rotation);
        ballScript.direction = new Vector3(Random.Range(-1f, 1f), 0f, 1f).normalized;

        Player.transform.SetPositionAndRotation(playerInitPos.position, playerInitPos.rotation);
    }
}
