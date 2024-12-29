using System.Collections;
using UnityEngine;


public class CannonController : MonoBehaviour
{
    public GameObject chickenPrefab; // The chicken prefab to shoot
    public Transform firePoint;      // Where the chicken is spawned
    public float shootForce = 500f;  // Base force applied to the chicken
    public Camera mainCamera;        // Reference to the main camera

    private Quaternion initialRotation; // To store the cannon's initial rotation

    private bool isRecoiling = false; // Track if recoil animation is happening

    private Vector3 initialPosition; // Original position of the cannon

    public float recoilDistance = 0.5f; // How far the cannon recoils
    public float recoilSpeed = 5f;     // Speed of the recoil animation

    public ParticleSystem smokeEffect;

    public AudioSource audioSource;

    void Start()
    {
        // Ensure a camera is assigned
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Save the initial rotation of the cannon
        initialRotation = transform.rotation;
    }

    void Update()
    {
        RotateCannonToMouse();

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            ShootChicken();
            StartCoroutine(Recoil());
        }
    }

    private void RotateCannonToMouse()
    {
        // Get the mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0)); // Ground plane at cannon height

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            // Calculate direction from cannon to target
            Vector3 direction = targetPosition - transform.position;

            // Clamp the direction to avoid excessive tilting
            float clampedX = Mathf.Clamp(direction.x, -50f, 50f); // Adjust for side tilt
            float clampedZ = Mathf.Clamp(direction.z, -25f, 25f); // Adjust for forward/backward tilt

            // Apply rotation, keeping the muzzle pointed down
            transform.rotation = Quaternion.Euler(
                initialRotation.eulerAngles.x + clampedZ, // Tilt slightly forward/back
                initialRotation.eulerAngles.y,           // Lock Y-axis rotation
                initialRotation.eulerAngles.z + clampedX // Tilt slightly left/right
            );
        }
    }

    private void ShootChicken()
    {
        if (chickenPrefab != null && firePoint != null)
        {
            // Spawn the chicken prefab at the fire point
            GameObject chicken = Instantiate(chickenPrefab, firePoint.position, firePoint.rotation);

            // Get Rigidbody of the chicken
            Rigidbody rb = chicken.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Shoot the chicken in the forward direction of the firePoint
                rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);
            }

            // Trigger the smoke effect
            if (smokeEffect != null)
            {
                smokeEffect.Play();
            }

            // Play the sound
            if (audioSource != null)
            {
                audioSource.Play();
            }

        }
    }


    private IEnumerator Recoil()
{
    if (isRecoiling) yield break;
    isRecoiling = true;

    // Store the original local position
    Vector3 originalPosition = transform.localPosition;

    // Define the recoil position relative to the original position
    Vector3 recoilPosition = originalPosition - transform.forward * recoilDistance;

    // Move to the recoil position
    while (Vector3.Distance(transform.localPosition, recoilPosition) > 0.01f)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPosition, Time.deltaTime * recoilSpeed);
        yield return null;
    }

    // Return to the original position
    while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
        yield return null;
    }

    // Ensure the cannon stays exactly at the original position
    transform.localPosition = originalPosition;
    isRecoiling = false;
}


}                                                                              
