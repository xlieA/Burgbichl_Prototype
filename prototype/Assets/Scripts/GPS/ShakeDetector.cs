// https://resocoder.com/2018/07/20/shake-detecion-for-mobile-devices-in-unity-android-ios/
using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    // Force behinde shake motion
    public float shakeDetectionThreshold = 3.6f;
    // Time between shakes
    public float minShakeInterval = 0.2f;

    // Work with squared threshold -> avoid root calculations (more efficient)
    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;


    // Start is called before the first frame update
    void Start()
    {
        sqrShakeDetectionThreshold = Mathf.Pow(shakeDetectionThreshold, 2);
    }

    // Detects shake motion of phone
    public bool MotionDetected()
    {
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold && Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
        {
            timeSinceLastShake = Time.unscaledTime;
            return true;
        }

        return false;
    }
}
