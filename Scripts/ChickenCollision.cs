using UnityEngine;

public class ChickenCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Truck"))
        {
            // Notify the truck about the chicken delivery
            TruckController truck = collision.gameObject.GetComponent<TruckController>();
            if (truck != null)
            {
                truck.DeliverChicken();
            }

            // Disable the chicken's Rigidbody to stop any physical force on the truck
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics interaction
            }

            // Optionally disable further collisions for the chicken
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false; // Prevent further collisions
            }

            // Destroy the chicken after a short delay (optional)
            Destroy(gameObject, 1f);
        }
    }
}
