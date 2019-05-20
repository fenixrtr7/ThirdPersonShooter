using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public static CameraFollower sharedInstance;

    public GameObject followTarget;

    public float movementSmoothness = 1.0f;
    public float rotationSmoothness = 1.0f;

    public bool canFollow = true;

    private void Awake() {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate() { // LateUpdate() => Ultimo Frame
        if(followTarget == null || canFollow)
        {
            return;
        }
        // Lerp() => Interpolaciòn lineal
        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * movementSmoothness);
        // Slerp() => Rotaciòn esferica
        transform.rotation = Quaternion.Slerp(transform.rotation, followTarget.transform.rotation, Time.deltaTime * rotationSmoothness);
    }
}
