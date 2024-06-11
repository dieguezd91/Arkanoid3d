using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedMovement = 10f;
    public float xRange = 8f;

    private float halfBarWidth;
    private bool isBarExpanded = false;

    private void Start()
    {
        halfBarWidth = GetComponent<Renderer>().bounds.size.x / 2;
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
}
