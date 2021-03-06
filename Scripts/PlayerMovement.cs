using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody rb;
    public Animator animator;
    public bool face_left = false;
    public bool jump = false;


    Vector2 movement;
    KeyCode last_key_pressed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey("left"))
        {
            face_left = true;
            last_key_pressed = KeyCode.LeftArrow;
        }
        else if (Input.GetKey("right"))
        {
            face_left = false;
            last_key_pressed = KeyCode.RightArrow;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            jump = true;
        }
        else if (last_key_pressed == KeyCode.LeftArrow)
        {
            face_left = true;
        }
        else if (last_key_pressed == KeyCode.RightArrow)
        {
            face_left = false;
        }




        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);

        animator.SetFloat("speed", movement.sqrMagnitude);
        animator.SetBool("face_left", face_left);


    }

    void FixedUpdate()
    {
        // Movement 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
