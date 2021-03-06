using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SticksController : MonoBehaviour
{
    public LayerMask enemyLayer;

    public Animator animator;
    public Rigidbody rb;
    public bool isSpinning;
    public Transform playerPos;

    public State state;
    
    
    bool faceLeft = false;

    public enum State 
    {
        WithPlayer,
        Thrown,
        Recalling,
    }

    void Start()
    {
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
        }        
    }

    void OnTriggerEnter(Collider col)
    {
       // Debug.Log("yeboyy");
    }

    public void setState(State newState)
    {
        state = newState;
    }

    public State getState()
    {
        return state;
    }

    public bool withPlayer()
    {
        return state == State.WithPlayer;
    }

    public void setPlayerPos(Transform newPlayerPos)
    {
        playerPos = newPlayerPos;
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

    public void throwStick ()
    {
        Vector3 throwDir = (transform.position - playerPos.position).normalized;
        rb.AddForce(throwDir * 7f, ForceMode.Impulse);
    }

    public void recallStick()
    {
        rb.velocity = Vector3.zero;
        Vector3 throwDir = (playerPos.position - transform.position).normalized;
        rb.AddForce(throwDir * 10f, ForceMode.Impulse);

        if (Vector3.Distance(transform.position, playerPos.position) < 2f)
        {
            state = State.WithPlayer;

        }
    }

}

