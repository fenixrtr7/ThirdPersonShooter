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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
