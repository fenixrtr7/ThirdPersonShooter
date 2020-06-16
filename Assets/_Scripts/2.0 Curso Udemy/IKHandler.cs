using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandler : MonoBehaviour
{
    Animator animator;
    public Transform leftFoot, rightFoot;

    #region  UTILIZANDO PARA TESTING MANUAL
    public Transform leftIKTarget, rightIKTarget;
    public Transform hintLeft, hitRight;

    public float ikWeight = 1f;
    #endregion
    [Header("Valores dinámicos de IK")]
    public Vector3 leftFootPosition, rightFootPosition;
    public Quaternion leftFootRotation, rightFootRotation;
    public float leftFootWeight, rightFootWeight;

    [Header("Más propiedades para IK")]
    public float offsetY;
    public float lookIKWeight = 1f;
    public float bodyWeight = 0.7f;
    public float headWeight = 0.9f;
    public float eyesWeight = 1f;
    public float clampWeight = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        leftFootPosition = leftFoot.position;
        leftFootRotation = leftFoot.rotation;

        rightFootPosition = rightFoot.position;
        rightFootRotation = rightFoot.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Me sirve para saber donde está mirando el personaje
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 20f, Color.cyan);

        RaycastHit leftHit, rightHit;

        Vector3 lPos = leftFoot.TransformPoint(Vector3.zero); // TrasformPoint() => Transformar un punto a coordenadas locales
        Vector3 rPos = rightFoot.TransformPoint(Vector3.zero); // TransformPoint() => Transformar un punto a coordenadas locales

        if(Physics.Raycast(lPos, Vector3.down, out leftHit, 1f))
        {
            leftFootPosition = leftHit.point;
            leftFootRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }
        Debug.DrawRay(lPos, Vector3.down, Color.red);

        if(Physics.Raycast(rPos, Vector3.down, out rightHit, 1f))
        {
            rightFootPosition = rightHit.point;
            rightFootRotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }
        Debug.DrawRay(lPos, Vector3.down, Color.green);
    }
}
