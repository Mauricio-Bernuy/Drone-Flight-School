using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followsmooth : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float speed = 4f;

    void Update()
    {
        if(!transform.GetComponent<OVRGrabbable>().isGrabbed){

            // Define a target position above and behind the target transform
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, 0));

            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(Camera.main.transform);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);

        }
    }

    

}

