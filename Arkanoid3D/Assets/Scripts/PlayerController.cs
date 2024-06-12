using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Renderer _renderer;

    public float speedMovement;
    public float xRange;

    private float halfBarWidth;
    float currentXRange;

    //MAGNET
    public bool IsMagnetActive => isMagnetActive;
    bool isMagnetActive;
    Ball catchedBall;
    Transform magnetPos;

    //EXPAND
    public bool IsBarExpanded => isBarExpanded;
    private bool isBarExpanded;

    public bool magnetActive = false;
    public bool catchedBall = false;
    public Transform magnetPos;

    private Ball _ball;

    private void Start()
    {
        halfBarWidth = GetComponent<Renderer>().bounds.size.x / 2;

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            _ball = ball.GetComponent<Ball>();
        }

        GameObject magnetPosObject = GameObject.Find("MagnetPos");
        if (magnetPosObject != null)
        {
            magnetPos = magnetPosObject.transform;
        }
    }

    public void UpdatePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * speedMovement * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -currentXRange, currentXRange);
        transform.position = newPosition;
        if (catchedBall != null) MagnetLogic();
    }

    public void ManageBarSize(float scaleFactor)
    {
        if (isBarExpanded) scaleFactor = 1 / scaleFactor;
        transform.localScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y, transform.localScale.z);
        halfBarWidth = _renderer.bounds.size.x / 2;
        currentXRange = xRange - halfBarWidth;
        isBarExpanded = !isBarExpanded;
    }

    public void ManageMagnetState() => isMagnetActive = !isMagnetActive; 

    public void CatchBall(Ball ball) => catchedBall = ball;

    public void MagnetLogic()
    {
        catchedBall.transform.position = magnetPos.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            catchedBall.SetNewDirection();
            catchedBall = null;
        }
    }

    public void ShrinkBar(float scaleFactor)
    {
        if (isBarExpanded)
        {
            transform.localScale = new Vector3(transform.localScale.x / scaleFactor, transform.localScale.y, transform.localScale.z);

            halfBarWidth = GetComponent<Renderer>().bounds.size.x / 2;

            isBarExpanded = false;
        }
    }

    public bool IsBarExpanded()
    {
        return isBarExpanded;
    }


    public void EnableMagnet()
    {
        magnetActive = true;
    }

    public void DisableMagnet()
    {
        magnetActive = false;
    }

    public bool IsMagnetEnabled()
    {
        return magnetActive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && magnetActive)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                ball.RB.velocity = Vector3.zero;
                ball.speed = 0f;
                catchedBall = true;
                _ball = ball;
            }
        }
    }

    public void MagnetLogic()
    {
        if (catchedBall && magnetPos != null)
        {
            _ball.transform.position = magnetPos.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ball.SetNewDirection();
                _ball.speed = _ball.maxSpeed;
                catchedBall = false;
            }
        }
    }
}
