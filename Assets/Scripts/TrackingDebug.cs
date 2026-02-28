using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackingDebug : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var trackedImage in args.added)
            Debug.Log($"TRACKING_DEBUG: Image detected! Name: {trackedImage.referenceImage.name}");

        foreach (var trackedImage in args.updated)
            Debug.Log($"TRACKING_DEBUG: Image updated. Name: {trackedImage.referenceImage.name}, State: {trackedImage.trackingState}");

        foreach (var trackedImage in args.removed)
            Debug.Log("TRACKING_DEBUG: Image lost.");
    }
}