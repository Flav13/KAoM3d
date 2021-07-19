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
    bool faceLeft = false;

    Vector3 throwDirection;
    Camera mainCamera;
    Vector3 playerSpeed;
    Vector3 throwingPosition;

    public State state;
    public enum State 
    {
        WithPlayer,
        Thrown,
        Recalling,
    }

    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();

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
            GetComponent<Renderer>().enabled = true;
            throwStick();
        }
        else if (state == State.Recalling)
        {
            recallStick();
            
        }
        if (state == State.WithPlayer)
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

            GetComponent<Renderer>().enabled = false;
        }
    }


    IEnumerator AttackWait()
    {
        setState(State.Thrown);

        yield return new WaitForSeconds(0.2f);
        setState(State.Recalling);
    }


    void throwStick()
    {
       // throwingPosition = playerPos.position;
        Vector3 throwDir = (throwDirection - playerPos.position).normalized;
        rb.AddForce((throwDir *5f), ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider col)
    {
        rb.velocity = Vector3.zero;
    }

    void recallStick()
    {
      rb.velocity = Vector3.zero;
      Vector3 throwDir = (playerPos.position - throwDirection).normalized;
      rb.AddForce((throwDir * 10f), ForceMode.Impulse);

        if (Vector3.Distance(transform.position, playerPos.position) < 3.0f)
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

    public void setPlayerSpeed(Vector3 speed)
    {
        playerSpeed = speed;
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


    public void StartSpin(Vector2 throwDir)
    {
        throwDirection = mainCamera.ScreenToWorldPoint(new Vector3(throwDir.x, throwDir.y, transform.position.z));
        throwDirection.z = transform.position.z;
        StartCoroutine(AttackWait());
    }

}

