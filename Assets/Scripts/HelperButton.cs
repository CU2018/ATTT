// Fall 2021 - CS294-137 HW2
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperButton : MonoBehaviour, OnTouch3D
{
    public Image helperImage;
    public bool imageOn = false;

    public void OnTouch()
    {
        imageOn = !imageOn;
        helperImage.gameObject.SetActive(imageOn);
    }
}
