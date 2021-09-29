// Fall 2021 - CS294-137 HW3
// Siyu Zhang

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
// and overlays a cube on each of the tracked image
// Note that this code assumes that all tracked images are unique and named differently


[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoManager : MonoBehaviour
{
        
    public ToggleSwitch toggleSwitch;
    public GameObject gameBoard;
    public GameController gameController;

    private Animator gameBoardAnimator;

    ARTrackedImageManager m_TrackedImageManager;
    Dictionary<string,GameObject> gameobjectDict = new Dictionary<string,GameObject>();

    void Awake()
    {
        //This gets a reference to the AR Tracked Image Manager attached to the 'AR session Origin' gameobject
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void Start()
    {
        gameBoardAnimator = gameBoard.GetComponent<Animator>();
        gameBoardAnimator.enabled = false;
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }


    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        
        //eventArgs.added contains all the newly trackedImages that were found this frame
        foreach (var trackedImage in eventArgs.added)
        {

            //Write code here to deploy stuff whenever a new image is found in the tracking
            //e.g. Create a new virtual object and/or attach it to the tracked image
            //trackedImage.referenceImage.name -> Name of the tracked image
            //trackedImage.transform.position -> Position of the tracked image in the real world 
            //trackedImage.transform.rotation -> Rotation of the tracked image in the real world 

            //For example, here when we find a new tracked Image, we create a cube, and place it at the location of the tracked image. We add the gameobject to a dictionary, using the name of the image as a key.
            
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // cube.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
            // cube.transform.position = trackedImage.transform.position;
            // cube.transform.rotation = trackedImage.transform.rotation;
            // gameobjectDict.Add(trackedImage.referenceImage.name,cube);

            if(!gameController.AtttEnabled)
            {   
                // gameBoard.transform.position = trackedImage.transform.position;
                // gameBoard.transform.rotation = trackedImage.transform.rotation;
                gameBoardAnimator.Play("flipBoard");
                toggleSwitch.OnImageDetected();
               
            }
        }

        //eventArgs.removed contains all the trackedImages that were not found by the AR camera, either because it was removed from the camera's views or becuase the camera could not detect it.
        foreach (var trackedImage in eventArgs.removed)
        {
            //If we loose tracking of the image, we destroy the conrresponding cube and remove the image name's entry from the dictionary
            //Destroy(gameobjectDict[trackedImage.referenceImage.name]);
            //gameobjectDict.Remove(trackedImage.referenceImage.name);
        }
        //eventArgs.updated contains all the trackedImages which are currently being tracked, but its position and/or rotation changed
        foreach (var trackedImage in eventArgs.updated)
        {
            //trackedImage.transform.position -> Updated Position of the tracked image in the real world 
            //trackedImage.transform.rotation -> Updated Rotation of the tracked image in the real world 

            //if tracked image moves, we move the corresponding gameobject to match it's position.
            //gameobjectDict[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            //gameobjectDict[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        }
    }
}

