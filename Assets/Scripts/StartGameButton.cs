// Fall 2021 - CS294-137 HW3
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

// Adding OnTouch3D here forces us to implement the 
// OnTouch function, but also allows us to reference this
// object through the OnTouch3D class.
public class StartGameButton : MonoBehaviour, OnTouch3D
{
    // Debouncing is a term from Electrical Engineering referring to 
    // preventing multiple presses of a button due to the physical switch
    // inside the button "bouncing".
    // In CS we use it to mean any action to prevent repeated input. 
    // Here we will simply wait a specified time before letting the button
    // be pressed again.
    // We set this to a public variable so you can easily adjust this in the
    // Unity UI.
    // While this is not so much useful in simulation where we just do click usign a mouse, it is 
    // essential when we dpeloy our app to a phone, where the input is through touch.
    
    public float debounceTime = 0.3f;
    public GameController gameController;
    public TextMesh ButtonText;
    // Stores a counter for the current remaining wait time.
    private float remainingDebounceTime;
    //private bool isStartMode = true;  // show text of "Start!" / "Clear!" [1]

    void Start()
    {
        remainingDebounceTime = 0;
    }

    void Update()
    {
        // Time.deltaTime stores the time since the last update.
        // So all we need to do here is subtract this from the remaining
        // time at each update.
        ButtonText.text = gameController.gameStarted ? "Clear!" : "Start!";   // change from Start to Clear / Clear to Start
        if (remainingDebounceTime > 0)
            remainingDebounceTime -= Time.deltaTime;
    }

    public void OnTouch()
    {
        // If a touch is found and we are not waiting,
        if (remainingDebounceTime <= 0)
        {
            // Move the object up by 10cm and reset the wait counter.
            // this.gameObject.transform.Translate(new Vector3(0, 0.1f, 0));
            // Debug.Log("OHHHHHHHHHHHHH");
            gameController.showStartMessage();
            remainingDebounceTime = debounceTime;
        }
    }
}

