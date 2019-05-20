using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Rigidbody cherryRB;
    public float throwDistance = 10000f;
    public float timeToDestroy = 4f;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (PlayerManager.currentCherryCount > 0)
            {
                ThrowCherry();
            }
        }
    }

    void ThrowCherry()
    {
        // Trazar rayo de pantalla a Mundo 3D
        Ray cameraToWorldRay = cam.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(cameraToWorldRay, out hit)) // out => Si choca obtenemos dirección
        {
            Debug.DrawLine(transform.position, hit.point);

            Vector3 directionToFire = hit.point - this.transform.position;

            Rigidbody cherryClone = (Rigidbody)Instantiate(cherryRB, transform.position, transform.rotation);
            //cherryClone.useGravity = true;
            cherryClone.constraints = RigidbodyConstraints.None; // .constrains || RigidbodyConstraints.None; => Elimina restricciones
            cherryClone.AddForce(directionToFire.normalized * throwDistance);
            Destroy(cherryClone.gameObject, timeToDestroy);

            PlayerManager.currentCherryCount--;
        }
    }
}
