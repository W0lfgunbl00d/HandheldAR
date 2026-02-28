using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private string markerImageName = "marker"; // match name in library

    private bool hasSpawned = false;

    private void Start()
    {
        // Check if the gameboard has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("SPAWNER: No collider found on gameboard!");
        else if (!col.isTrigger)
            Debug.LogError("SPAWNER: Collider is NOT set as trigger!");
        else
            Debug.LogError("SPAWNER: Gameboard trigger collider is set up correctly.");

        // Check if prefab is assigned
        if (prefabToSpawn == null)
            Debug.LogError("SPAWNER: prefabToSpawn is not assigned in the Inspector!");
    }

    private void OnTriggerEnter(Collider other)
    {

        ARTrackedImage trackedImage = other.GetComponent<ARTrackedImage>();

        if (trackedImage == null)
        {
            return;
        }


        if (trackedImage.referenceImage.name != markerImageName)
        {
            return;
        }

        if (hasSpawned)
        {
            return;
        }

        Instantiate(prefabToSpawn, (trackedImage.transform.position + new Vector3(0, 0.5f, 0)), trackedImage.transform.rotation);
        hasSpawned = true;
    }

    private void OnTriggerExit(Collider other)
    {
        ARTrackedImage trackedImage = other.GetComponent<ARTrackedImage>();
        if (trackedImage != null)
        {
            hasSpawned = false;
        }
    }
}