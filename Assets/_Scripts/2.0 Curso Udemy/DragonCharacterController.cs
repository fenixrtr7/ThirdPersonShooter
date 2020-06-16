using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCharacterController : MonoBehaviour
{
    private const string ANIM_SPEED = "speed", ANIM_HORI = "horizontal", ANIM_VERT = "vertical", ANIM_DIE = "die", ANIM_HIT = "hit",
    ANIM_ATT = "attack";
    Animator animator;
    
    Rigidbody rigidBody;

    public float speed = 0f;
    public float horizontal = 0f;
    public float vertical = 0f;
    float rotaionSpeed = 90;
    public float speedRun = 10f, fireSpeed = 200f;

    public bool hit, die, dead, attack; // Inicializadas igual a flase
    Transform dragonMouth;
    public GameObject fireball;
    GameObject currentFireball;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        dragonMouth = GameObject.Find("DragonMouth").transform;

        currentFireball = Instantiate(fireball, dragonMouth);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            if (die)
            {
                animator.SetBool(ANIM_DIE, true);
                die = !die;
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            attack = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            attack = false;
        }
        animator.SetBool(ANIM_ATT, attack);

        if (Input.GetMouseButtonDown(1))
        {
            currentFireball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentFireball.GetComponent<Rigidbody>().AddForce(transform.forward * fireSpeed, ForceMode.Impulse);

            currentFireball.gameObject.transform.parent = null;

            currentFireball.GetComponent<AudioSource>().Play();
            Invoke("LoadNewFireBall", 1f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hit = true;
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            hit = false;
        }
        animator.SetBool(ANIM_HIT, hit);

        if (Input.GetKeyUp(KeyCode.I))
        {
            die = true;
            dead = true;
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        speed = new Vector2(horizontal, vertical).sqrMagnitude;
        rigidBody.velocity = transform.forward * vertical * speedRun + new Vector3(0, rigidBody.velocity.y, 0);
        //AddForce(new Vector3(vertical, 0, horizontal));
        this.transform.Rotate(0, horizontal * rotaionSpeed * Time.deltaTime, 0);

        animator.SetFloat(ANIM_SPEED, speed);
        animator.SetFloat(ANIM_HORI, horizontal);
        animator.SetFloat(ANIM_VERT, vertical);
    }

    void LoadNewFireBall()
    {
        currentFireball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        currentFireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentFireball.transform.position = dragonMouth.position;
        currentFireball.gameObject.transform.parent = dragonMouth;
    }
}
