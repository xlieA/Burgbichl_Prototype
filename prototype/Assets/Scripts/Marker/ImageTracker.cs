// https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/features/image-tracking.html
// https://www.youtube.com/playlist?list=PLPJUho5jpFX-QomGPtAqpEKacSOjUQ5UH
// https://youtu.be/Fpw7V3oa4fs?si=qGqSpgNo4_Rg0vsn
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] prefabs;

    List<GameObject> objects = new List<GameObject>();

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Called everytime camera detects image
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var prefab in prefabs)
            {
                // IMPORTANT: prefab name and image name in ReferenceImageLibrary NEED TO MATCH EXACTLY
                if (trackedImage.referenceImage.name == prefab.name)
                {
                    var newPrefab = Instantiate(prefab, trackedImage.transform);
                    objects.Add(newPrefab);
                }
            }
        }

        // Update tracking position (tracker moved)
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var gameObject in objects)
            {
                if (gameObject.name == trackedImage.name)
                {
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }
}