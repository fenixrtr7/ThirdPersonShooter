using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianCharacterController : MonoBehaviour
{
    const string ANIM_SPEED = "speed", ANIM_HORI = "horizontal", ANIM_VERT = "vertical", ANIM_JUMP = "jump", ANIM_S_AT = "superAttack", ANIM_DIE = "die", ANIM_ATT = "attack", ANIM_RUN = "run";

    Animator animator;

    public float speed = 5.0f;
    public float horizontal = 0f;
    public float vertical = 0f;

    public bool attack = false;
    public bool jump = false;
    public bool die = false;
    public bool superAttack = false;

    public bool run = false;
    public bool dead = false;

    public Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (die)
        {
            animator.SetBool(ANIM_DIE, true);
            die = !die;
            return;
        }
        // Aquí el personaje seguro que está vivo
        if(Input.GetKeyDown(KeyCode.C) && !attack)
        {
            attack = true;
            animator.SetBool(ANIM_ATT, attack);
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            attack = false;
            animator.SetBool(ANIM_ATT, attack);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            superAttack = true;
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            superAttack = false;
        }
        animator.SetBool(ANIM_S_AT, superAttack);

        if(Input.GetKeyDown(KeyCode.LeftShift) || speed >= 0.5f)
        {
            run = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || speed < 0.5f)
        {
            run = false;
        }
        animator.SetBool(ANIM_RUN, run);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            jump = false;
        }
        animator.SetBool(ANIM_JUMP, jump);

        if(Input.GetKeyDown(KeyCode.I))
        {
            die = true;
            dead = true;
        }
    }

    private void FixedUpdate() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        speed = new Vector2(horizontal, vertical).sqrMagnitude;

        animator.SetBool(ANIM_RUN, speed >= 0.5f);
        animator.SetFloat(ANIM_SPEED, speed);
        animator.SetFloat(ANIM_HORI, horizontal);
        animator.SetFloat(ANIM_VERT, vertical);
    }
}
