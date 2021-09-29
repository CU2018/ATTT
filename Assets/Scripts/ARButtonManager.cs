// Fall 2021 - CS294-137 HW2
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using cs294_137.hw2; //TO_COMMENT
using UnityEngine.XR.ARFoundation; //TO_ADD
using UnityEngine.XR.ARSubsystems; //TO_ADD

public class ARButtonManager : MonoBehaviour
{
    private Camera arCamera;
    private PlaceGameBoard placeGameBoard;

    void Start()
    {
        // Here we will grab the AR camera 
        // This camera acts like any other camera in Unity.        
        //arCamera = FindObjectOfType<ARCamera>().GetComponent<Camera>();//TO_COMMENT
        arCamera = GetComponent<ARSessionOrigin>().camera; //TO_ADD
        // We will also need the PlaceGameBoard script to know if
        // the game board exists or not.
        placeGameBoard = GetComponent<PlaceGameBoard>();
    }

    void Update()
    {
        //if (placeGameBoard.Placed() && Input.GetMouseButtonDown(0))//TO_COMMENT
        if (placeGameBoard.Placed() && Input.touchCount > 0) //TO_ADD
        {
            //Vector2 touchPosition = Input.mousePosition; //TO_COMMENT
            Vector2 touchPosition = Input.GetTouch(0).position; //TO_ADD
            // Convert the 2d screen point into a ray.
            Ray ray = arCamera.ScreenPointToRay(touchPosition);
            // Check if this hits an object within 100m of the user.

/*TO_COMMENT
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100.0F);
            for (int i = 0; i < hits.Length; i++)
            {
                // Check that the object is interactable.
                if(hits[i].transform.tag=="Interactable")
                    // Call the OnTouch function.
                    // Note the use of OnTouch3D here lets us
                    // call any class inheriting from OnTouch3D.
                    hits[i].transform.GetComponent<OnTouch3D>().OnTouch();
            }
*/

//TO_ADD ================================================
            RaycastHit hit; 
            if (Physics.Raycast(ray, out hit,100))
            {
                // Check that the object is interactable.
                if(hit.transform.tag=="Interactable")
                    // Call the OnTouch function.
                    // Note the use of OnTouch3D here lets us
                    // call any class inheriting from OnTouch3D.
                    hit.transform.GetComponent<OnTouch3D>().OnTouch();
            }
//================================================
        }
    }
}

