using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SticksController : MonoBehaviour
{
    //layers
    public LayerMask enemyLayer;

    // components
    public Animator animator;
    public Rigidbody rb;
    public bool isSpinning;
    public Transform playerPos;

    // bools
    bool attack_running;
    bool faceLeft = false;

    public State state;
    public enum State 
    {
        WithPlayer,
        Thrown,
        Recalling,
    }

    void Start()
    {
        attack_running = false;
        isSpinning = false;
        state = State.WithPlayer;
        transform.position = playerPos.position;
        transform.Translate(1, 0, 0);


    }

    void FixedUpdate()
    { 
        animator.SetBool("isSpinning", isSpinning);

        if (state==State.Thrown)
        {
            throwStick();
        }
        else if (state == State.Recalling)
        {
            recallStick();
            
        }
        else if (state == State.WithPlayer)
        {
            attack_running = false;
            rb.velocity = Vector3.zero;
            transform.position = playerPos.position;

            if (faceLeft)
            {
                transform.Translate(-1, 0, 0);
            }
            else
            {
                transform.Translate(1, 0, 0);
            }
            GetComponent<Renderer>().enabled = false;

        }
    }


    IEnumerator AttackWait()
    {
        attack_running = true;
        setState(State.Thrown);

        yield return new WaitForSeconds(0.14f);

        setState(State.Recalling);

    }


    void throwStick()
    {
       Vector3 throwDir = (transform.position - playerPos.position).normalized;
       rb.AddForce(throwDir *10f, ForceMode.Impulse);

    }

    void OnTriggerEnter(Collider col)
    {
        rb.velocity = Vector3.zero;

    }

    void recallStick()
    {
      rb.velocity = Vector3.zero;
      Vector3 throwDir = (playerPos.position - transform.position).normalized;
       rb.AddForce(throwDir * 20f, ForceMode.Impulse);

        if (Vector3.Distance(transform.position, playerPos.position) < 2f)
        {
            state = State.WithPlayer;

        }
    }

    // public methods

    public void setState(State newState)
    {
        state = newState;
    }

    public void setPlayerPos(Transform player)
    {
        playerPos = player;
    }

    public State getState()
    {
        return state;
    }

    public bool withPlayer()
    {
        return state == State.WithPlayer;
    }

    public void setSpin(bool spin)
    {
        isSpinning = spin;
    }

    public void Flip()
    {
        faceLeft = !faceLeft;
        if (state == State.WithPlayer)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            if (faceLeft)
            {
                transform.Translate(-1f, 0, 0);
            }
            else
            {
                transform.Translate(1f, 0, 0);
            }
        }
    }

    public void StartSpin()
    {
        if (!attack_running)
        {
            StartCoroutine(AttackWait());
        }
    }

}

