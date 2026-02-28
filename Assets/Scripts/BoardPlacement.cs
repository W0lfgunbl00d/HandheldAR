using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class BoardPlacement : MonoBehaviour
{
    [Header("Setup")]
    public ARRaycastManager arRaycastManager;
    public GameObject boardPrefab;

    private GameObject board;
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        if (arRaycastManager == null)
        {
            arRaycastManager = FindAnyObjectByType<ARRaycastManager>();
        }

        if (arRaycastManager == null)
        {
            Debug.LogError("BoardPlacement: ARRaycastManager is still missing from the scene!");
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }

        if (Touch.activeTouches.Count == 0) return;

        Touch firstActiveTouch = Touch.activeTouches[0];
        if (firstActiveTouch.phase != TouchPhase.Began) return;

        if (boardPrefab == null)
        {
            Debug.LogError("BoardPlacement: Please assign a Board Prefab in the Inspector.");
            return;
        }

        if (arRaycastManager.Raycast(firstActiveTouch.screenPosition, s_Hits,
            UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            if (board == null)
            {
                board = Instantiate(boardPrefab, hitPose.position, hitPose.rotation);
                board.transform.Rotate(-90, 0, 0);
                Debug.Log("Board Spawned!");
            }
            else
            {
                board.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                board.transform.Rotate(-90, 0, 0);
                Debug.Log("Board Repositioned!");
            }
        }
        else
        {
            Debug.Log("Raycast failed: No valid plane detected at this touch point.");
        }
    }
}