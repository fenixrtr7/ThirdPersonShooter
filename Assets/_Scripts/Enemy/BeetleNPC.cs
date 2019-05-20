using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleNPC : MonoBehaviour
{
    Animator m_Animator;
    public GameObject nextCucumberToDestroy;

    // Variables para responder el ataque de las cerezas
    public bool cherryHit = false;
    public bool hasReachedThePlayer = false;
    public float smoothTime = 3.0f;
    public Vector3 smoothVelocity = Vector3.zero;

    public HealthManager healthManager;

    private static countManager contadorEmenys;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        healthManager = GameObject.Find("Slider Health").GetComponent<HealthManager>();
        if(contadorEmenys == null)
        {
            contadorEmenys = GameObject.Find("Text Beetles").GetComponent<countManager>();
        }
        contadorEmenys.RecalculateBeetles();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasReachedThePlayer = true;

            healthManager.ReduceHealth();

            if (!cherryHit)
            {
                BeetlePatrol.isAttacking = true;
                GameObject thePlayer = other.gameObject;
                Transform trans = thePlayer.transform;
                this.gameObject.transform.LookAt(trans); // LookAt() => Mira la jugador o X cosa

                m_Animator.Play("Attack_OnGround");
                StartCoroutine(DestroyBeetle());
            }
            else
            {
                m_Animator.Play("Attack_Standing");
                StartCoroutine(DestroyBeetleStanding());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            nextCucumberToDestroy = other.gameObject;
            BeetlePatrol.isEating = true;
            m_Animator.Play("Eat_OnGround");
            StartCoroutine(DestroyBattery());
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            PointsManager.AddPoints(25);
            BeetlePatrol.isAttacking = true;
            cherryHit = true;
            m_Animator.Play("Stand");
        }
    }

    IEnumerator DestroyBattery()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(nextCucumberToDestroy.gameObject);
        BeetlePatrol.isEating = false;
    }

    IEnumerator DestroyBeetle()
    {
        yield return new WaitForSecondsRealtime(4f);
        m_Animator.Play("Die_OnGround");
        Destroy(this.gameObject, 2.0f);
        hasReachedThePlayer = false;

        contadorEmenys.RecalculateBeetles();
    }

    IEnumerator DestroyBeetleStanding()
    {
        yield return new WaitForSecondsRealtime(4f);
        m_Animator.Play("Die_Standing");
        Destroy(this.gameObject, 2f);
        cherryHit = false;
        hasReachedThePlayer = false;

        contadorEmenys.RecalculateBeetles();
    }

    private void Update()
    {
        if (cherryHit)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Transform transPlayer = player.transform;
            this.gameObject.transform.LookAt(transPlayer);

            if (!hasReachedThePlayer)
            {
                m_Animator.Play("Run_Standing");
            }
            // ref => Parametro Entrada/Salida
            transform.position = Vector3.SmoothDamp(transform.position, transPlayer.position, ref smoothVelocity, smoothTime);
        }
    }
}
