// Fall 2021 - CS294-137 HW2
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellButton : MonoBehaviour, OnTouch3D
{
    public Text messageText;
    public bool isInteractable = false;
    public int CellButtonID;
    public GameObject cellMarkCube;
    public GameController gameController;
    public PlaceGameBoard placeGameBoard;
    
    public float debounceTime = 0.3f;
    // Stores a counter for the current remaining wait time.
    private float remainingDebounceTime;

    void Start()
    {
        remainingDebounceTime = 0;
        isInteractable = false;
    }

    void Update()
    {
        // Time.deltaTime stores the time since the last update.
        // So all we need to do here is subtract this from the remaining
        // time at each update.
        if (remainingDebounceTime > 0)
            remainingDebounceTime -= Time.deltaTime;
    }

    public void OnTouch()
    {
        
        // If a touch is found and we are not waiting,
        if (remainingDebounceTime <= 0 && placeGameBoard.Placed() && isInteractable)
        {
            // messageText.gameObject.SetActive(true);
            string message = "CellButtonID: " + CellButtonID + "is pressed";
            Debug.Log(message);
            // messageText.text = message;
            gameController.TicTacToeButton(CellButtonID);
            remainingDebounceTime = debounceTime;
        }
    }
}
