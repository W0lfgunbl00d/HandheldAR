using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private string markerImageName = "marker"; // match name in library
    [SerializeField] private GameObject prefabEffect;

    private bool hasSpawned = false;
    private Vector3 originalRot;
    private Vector3 rotation;
    private float effectCooldown = 1.0f; // seconds between effects
    private float lastEffectTime = -999f;


    private void Start()
    {
        // Check if the gameboard has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("SPAWNER: No collider found on gameboard!");
        else if (!col.isTrigger)
            Debug.LogError("SPAWNER: Collider is NOT set as trigger!");
        else
            Debug.Log("SPAWNER: Gameboard trigger collider is set up correctly.");

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
        originalRot = trackedImage.transform.rotation.eulerAngles;
        hasSpawned = true;
    }

    private void OnTriggerStay(Collider other)
    {
        ARTrackedImage trackedImage = other.GetComponent<ARTrackedImage>();
        if (trackedImage != null && trackedImage.referenceImage.name == markerImageName)
        {
            rotation = trackedImage.transform.rotation.eulerAngles;

            float rotationDelta = Quaternion.Angle(
                Quaternion.Euler(originalRot),
                trackedImage.transform.rotation
            );

            // Only trigger if rotation changed meaningfully AND cooldown has passed
            if (rotationDelta > 10f && Time.time - lastEffectTime > effectCooldown)
            {
                Instantiate(prefabEffect, trackedImage.transform.position + new Vector3(0, 0.5f, 0), trackedImage.transform.rotation);
                Debug.Log($"SPAWNER: Effect instantiated");
                originalRot = rotation;
                lastEffectTime = Time.time;
            }
        }
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