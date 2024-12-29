using UnityEngine;

public class TruckManager : MonoBehaviour
{
    public GameObject truckPrefab; // Reference to the truck prefab
    public Transform spawnPoint; // Where trucks spawn
    public Transform stopPoint; // Where trucks stop

    void Start()
    {
        SpawnTruck();
    }

    private void SpawnTruck()
    {
        // Instantiate the truck at the spawn point
        GameObject truck = Instantiate(truckPrefab, spawnPoint.position, spawnPoint.rotation);

        // Assign the stop point to the truck
        TruckController controller = truck.GetComponent<TruckController>();
        if (controller != null)
        {
            controller.stopPoint = stopPoint;
        }
    }
}
