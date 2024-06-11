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

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        halfBarWidth = _renderer.bounds.size.x / 2;
        currentXRange = xRange - halfBarWidth;

        magnetPos = GameObject.Find("MagnetPos").GetComponent<Transform>();
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
}
