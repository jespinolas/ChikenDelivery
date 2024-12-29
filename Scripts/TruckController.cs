using UnityEngine;
using TMPro;

public class TruckController : MonoBehaviour
{
    public TextMeshProUGUI requestText; // Text above the truck to show chicken request
    public int chickenRequest;      // Total chickens required by this truck
    public Transform stopPoint;     // The stopping point for the truck
    public float speed = 10f;       // Speed of the truck
    public float waitTime = 15f;    // Time the truck waits before leaving

    private int chickensDelivered = 0; // Count of chickens delivered
    private bool isAtStopPoint = false; // Check if the truck has reached the stopping point
    private float waitTimer = 0f;       // Timer for waiting at the stop point
    private Rigidbody rb;               // Reference to the truck's Rigidbody

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Generate a random number of chickens (minimum 20)
        chickenRequest = Random.Range(20, 50);

        // Update the text to display the request
        if (requestText != null)
        {
            requestText.text = $"0/{chickenRequest}";
        }
    }

    void Update()
    {
        if (!isAtStopPoint)
        {
            MoveTowardsStopPoint();
        }
        else
        {
            // Wait at the stop point
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                LeaveScene();
            }
        }
    }

    private void MoveTowardsStopPoint()
{
    if (rb != null)
    {
        // Calculate direction to the stop point
        Vector3 direction = (stopPoint.position - transform.position).normalized;

        // Apply movement force (only along X and Z axes)
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

        // Stop the truck when close to the stop point
        if (Vector3.Distance(transform.position, stopPoint.position) < 0.5f)
        {
            rb.linearVelocity = Vector3.zero; // Stop the truck
            isAtStopPoint = true;
        }
    }
}


    public void DeliverChicken()
    {
        if (chickensDelivered < chickenRequest)
        {
            chickensDelivered++;
            if (requestText != null)
            {
                requestText.text = $"{chickensDelivered}/{chickenRequest}";
            }

            // Check if all chickens have been delivered
            if (chickensDelivered >= chickenRequest)
            {
                LeaveScene();
            }
        }
    }

    private void LeaveScene()
    {
        // Despawn the truck
        Destroy(gameObject);
    }
}
