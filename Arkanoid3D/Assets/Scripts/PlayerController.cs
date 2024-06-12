using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedMovement = 10f;
    public float xRange = 8f;

    private float halfBarWidth;
    private bool isBarExpanded = false;

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

        float currentXRange = xRange - halfBarWidth;

        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * speedMovement * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -currentXRange, currentXRange);
        transform.position = newPosition;
    }

    public void ExpandBar(float scaleFactor)
    {
        if (!isBarExpanded)
        {
            transform.localScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y, transform.localScale.z);

            halfBarWidth = GetComponent<Renderer>().bounds.size.x / 2;

            isBarExpanded = true;
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
