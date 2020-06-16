using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Barbarian : MonoBehaviour
{
    Animator animator;
    NavMeshAgent nav;
    SphereCollider col;
    GameObject player;

    public float speed = 0.0f, h = 0f, v = 0f;
    public bool attack, jump, die;

    public bool DEBUG, DEBUG_DRAW;
    public Vector3 direction; // Donde esta el player en relación NPC
    public float distance = 0f, angle = 0f; // Distancia entre jugador y NPC || Angulo entre jugador y NPC
    public bool playerInSight;
    public float fieldOfViewAngle = 120;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInSight)
        {
            this.transform.rotation = // Girar al NPC
                Quaternion.Slerp(this.transform.rotation, // desde donde mira ahora 
                                                          // Slerp -> Interpolacion lineal (Angulos)
                                Quaternion.LookRotation(direction), // hacia la direccion del player
                                0.1f); // en un tiempo 
        }

        if (player.transform.GetComponent<DragonCharacterController>().die)
        {
            animator.SetBool("attack", false);
            animator.SetFloat("speed", 0);
            animator.SetFloat("angularSpeed", 0);
        }
    }

    private void FixedUpdate()
    {
        h = angle;
        v = distance;
        speed = distance / Time.deltaTime;
        if (DEBUG)
        {
            Debug.Log(string.Format("H:{0} - V:{1}, S:{2}", h, v, speed));
        }
        animator.SetFloat("speed", speed);
        animator.SetFloat("angularSpeed", h);
        animator.SetBool("attack", attack);

        if (playerInSight)
        {
            if (animator.GetFloat("attack2") > 0.5f || animator.GetFloat("attack3") > 0.5f)
            {
                player.GetComponent<PlayerAgent>().playerCharacterData.health -= 1.0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            Debug.Log("Entrando en el TRIGGER");
            // vector = destino - origen
            direction = player.transform.position - this.transform.position;
            distance = Vector3.Magnitude(direction) - 1f;
            angle = Vector3.Dot(this.transform.forward, player.transform.position);
            // Dot => Producto de 2 vectores

            if (DEBUG_DRAW)
            {
                Debug.DrawLine(this.transform.position + Vector3.up, direction * 50, Color.red);
                Debug.DrawLine(player.transform.position, this.transform.position, Color.blue);
            }

            playerInSight = false;
            float calculateAngle = Vector3.Angle(direction, transform.forward);
            // Si el player está en el campo de visión
            if (calculateAngle < 0.5f * fieldOfViewAngle)
            {
                RaycastHit hit;
                if (DEBUG_DRAW)
                {
                    Debug.DrawRay(this.transform.position + transform.up, direction.normalized, Color.green);
                }
                // Trazo un rayo desde NPC hasta player
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    // Si lo primero que localiza el rayo es el jugador
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        if (DEBUG)
                        {
                            Debug.Log("Jugador en el campo de visión!!!");
                        }
                    }
                }
            }
            // Si después de toda la comprobación anterior, el player está en FoV del NPC
            if (playerInSight)
            {
                nav.SetDestination(player.transform.position);
                CalculatePathLenght(player.transform.position);
                // Si estoy muy cerca, puedo atacar...
                if (distance < 1.1f)
                {
                    attack = true;
                }
                else
                {
                    attack = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            distance = 0;
            angle = 0;
            attack = false;
            playerInSight = false;
        }
    }

    float CalculatePathLenght(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
            Vector3[] allTheWaypoints = new Vector3[path.corners.Length + 2];
            allTheWaypoints[0] = this.transform.position;
            allTheWaypoints[allTheWaypoints.Length - 1] = targetPosition;

            for (int i = 0; i < path.corners.Length; i++)
            {
                allTheWaypoints[i + 1] = path.corners[i];
            }

            float pathLength = 0;
            for (int i = 0; i < allTheWaypoints.Length - 1; i++)
            {
                pathLength += Vector3.Distance(allTheWaypoints[i], allTheWaypoints[i + 1]);
                if (DEBUG_DRAW)
                {
                    Debug.DrawLine(allTheWaypoints[i], allTheWaypoints[i + 1], Color.gray);
                }
            }
            return pathLength;
        }
        else
        {
            return 0;
        }
    }
}
