using UnityEngine;
using UnityEngine.UI;

public class PhysicsController : MonoBehaviour {

    public float ShakeForceMultiplier;
    public Rigidbody2D[] ShakingRigidbodies;
    public Text messageText;

    public void ShakeRigidbodies(Vector3 deviceAcceleration)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = "Device Shake Detected";
        // foreach (var rigidbody in ShakingRigidbodies)
        // {
        //     rigidbody.AddForce(deviceAcceleration * ShakeForceMultiplier, ForceMode2D.Impulse);
        // }
    }
}