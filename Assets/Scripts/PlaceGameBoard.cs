// Fall 2021 - CS294-137 HW
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This allows us to user the AR Foundation simulator functions
// using cs294_137.hw2;
using UnityEngine.XR.ARFoundation; 
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation.Samples;

public class PlaceGameBoard : MonoBehaviour
{
    // Public variables can be set from the unity UI.
    // We will set this to our Game Board object.
    public GameObject gameBoard;
    // These will store references to our other components.
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private PlaneDetectionController planeDetectionController;
    // This will indicate whether the game board is set.
    private bool placed = false;

    // Start is called before the first frame update.
    void Start()
    {
        // GetComponent allows us to reference other parts of this game object.
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        planeDetectionController = GetComponent<PlaneDetectionController>();
    }

    // Update is called once per frame.
    void Update()
    {
        if (!placed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPosition = Input.mousePosition;

                // Raycast will return a list of all planes intersected by the
                // ray as well as the intersection point.
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                //if (raycastManager.Raycast(//TO_COMMENT
                //    touchPosition, ref hits, TrackableType.PlaneWithinPolygon)) //TO_COMMENT
                if (raycastManager.Raycast(//TO_ADD
                    touchPosition, hits, TrackableType.PlaneWithinPolygon)) //TO_ADD
                {
                    // The list is sorted by distance so to get the location
                    // of the closest intersection we simply reference hits[0].
                    //var hitPose = hits[0].pose; //TO_COMMENT
                    var hitPosition = hits[0].pose.position; //TO_ADD
                    // Now we will activate our game board and place it at the
                    // chosen location.
                    gameBoard.SetActive(true);
                    //gameBoard.transform.position = hitPose.position;//TO_COMMENT
                    gameBoard.transform.position = hitPosition; //TO_ADD
                    placed = true;
                    // After we have placed the game board we will disable the
                    // planes in the scene as we no longer need them.
                    
                    planeManager.detectionMode = PlaneDetectionMode.None;

                    planeDetectionController.TogglePlaneDetection();
                }
            }
        }
        else
        {
            // The plane manager will set all detected planes to active by 
            // default so we will continue to disable these.
            //planeManager.SetTrackablesActive(false); //For older versions of AR foundation
            planeManager.detectionMode = PlaneDetectionMode.None;
        }
    }

    // If the user places the game board at an undesirable location we 
    // would like to allow the user to move the game board to a new location.
    public void AllowMoveGameBoard()
    {
        placed = false;
        //planeManager.SetTrackablesActive(true);
        gameBoard.SetActive(false);
        planeDetectionController.TogglePlaneDetection();
        //planeManager.detectionMode = PlaneDetectionMode.Horizontal;
    }

    // Lastly we will later need to allow other components to check whether the
    // game board has been places so we will add an accessor to this.
    public bool Placed()
    {
        return placed;
    }
}