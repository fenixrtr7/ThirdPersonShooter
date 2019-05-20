using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CrossPlatformInput; => Lo convierte a otras plataformas
using UnityStandardAssets.CrossPlatformInput;

// RequireComponent => Elementos Necesarios (SEGURIDAD)
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    public AudioClip jumpClip;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    float horizontal, vertical;
    Rigidbody m_Rigidbody;
    public float jumpForce, moveSpeed, runSpeed;
    float currentJumpForce = 0, currentMoveSpeed = 0;
    Animator m_Animator;

    float turnAmount, forwardAmount; // turnAmount - Cantidad giro | forwardAmount - Cantidad a vertical (Al frente)
    [SerializeField] float stationTurnAround = 180;
    [SerializeField] float movingTurnSpeed = 360;

    public Transform m_camera;
    Vector3 cameraForward;
    Vector3 move;
    bool jump;

    [SerializeField] float groundCheckDistance = 0.1f;
    float m_OrigGroundCheckDistance;
    public float drawLine = 0.1f;
    bool isGrounded;
    Vector3 groundNormal;

    [SerializeField] float moveveSpeedMultiplier = 1.0f;

    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        currentMoveSpeed = moveSpeed;
        m_Animator = GetComponent<Animator>();

        m_OrigGroundCheckDistance = groundCheckDistance;
    }

    private void Update()
    {
        // Comprobamos si estamos o no en el suelo
        CheckGroundStatus();

        // Comprobamos la cantidad de movimiento V/H
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");

        //Debug.Log("H: " + horizontal + ", V: " + vertical);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(jumpClip);
            m_Rigidbody.AddForce(0, jumpForce, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            currentMoveSpeed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (m_camera != null)
        {
            // Calculamos la direción de movimiento relativo a donde mira la cámara
            cameraForward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;
            move = vertical * cameraForward + horizontal * m_camera.right;
        }
        else
        {
            // En casode no tener cámara de movimiento, calculamos las coordenadas absolutas del mundo
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }
        if (move.magnitude > 0 && PlayerManager.hasDead == false)
        {
            Move(move);
        }

        if (isGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
    }

    // Comprueba si el personaje esta en el suelo
    void CheckGroundStatus()
    {
#if UNITY_EDITOR
        Debug.DrawLine(transform.position + Vector3.up * drawLine, transform.position + Vector3.down * drawLine, Color.red);
#endif

        RaycastHit hitInfo;

        // Trazamos el rayo unos 10cm más arriba de la suela del jugador...
        if (Physics.Raycast(transform.position + Vector3.up * drawLine, Vector3.down, out hitInfo, groundCheckDistance))
        { // out => guardar variable
            isGrounded = true;
            groundNormal = hitInfo.normal; // normal => Direccion perpendicular
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
        //Debug.Log(groundNormal);
    }

    void Move(Vector3 movement)
    {
        if (movement.magnitude > 1.0f)
        {
            movement.Normalize(); // Aquí ahora si que tiene longitud 1
        }
        // InverseTransformDirection() => Convertir de coordenadas "Mundo" a "Locales"
        movement = transform.InverseTransformDirection(movement);
        CheckGroundStatus();
        // Modificamos el movimiento según el vector normal a la superficie sobre la que camina...
        // ProjectOnPlane() => Calcular exactamente el vector cabeza de jugador y proyectarlo al suelo
        movement = Vector3.ProjectOnPlane(movement, groundNormal);
        // Atan2() => Arco tangente - Devuelve el angulo
        turnAmount = Mathf.Atan2(movement.x, movement.z);
        forwardAmount = movement.z;
        m_Rigidbody.velocity = transform.forward * currentMoveSpeed;
        ApplyExtraRotation();

        // if (!isGrounded)
        // {
        //     HandleAirborneMovement();
        // }
    }

    void ApplyExtraRotation()
    {
        // turnSpeed => Velocidad de giro
        float turnSpeed = Mathf.Lerp(stationTurnAround, movingTurnSpeed, forwardAmount); // Lerp => Interpolación lineal
        // s = v * t
        transform.Rotate(0, turnSpeed * turnAmount * Time.deltaTime, 0);
    }

    private void OnAnimatorMove()
    {
        if (isGrounded && Time.deltaTime > 0)
        {
            Vector3 vel = m_Animator.deltaPosition * moveveSpeedMultiplier / Time.deltaTime;
            vel.y = m_Rigidbody.velocity.y; // Para que el personaje siga con la misma velocidad de salto.
            m_Rigidbody.velocity = vel;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        groundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }
}
