using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movement
    Vector2 movementInput;
    Vector3 inputVector;

    // components
    public Rigidbody rb;
    public CapsuleCollider collider;
    public HealthBar healthBar;
    public Animator animator;

    // layers 
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    //bools
    public bool face_left = false;
    public bool is_attacking = false;
    public bool is_crouching = false; 
    public bool is_dead = false;
    public bool is_hurt = false;
    public bool is_jumping = false;
    public bool is_locked = false;

    bool hasFallen = false;
    bool is_active = false;

    // vars
    int currentHealth;
    float currentColHeight;
    Vector3 currentColCenter;

    public Vector2 moveDirection;
    public int currentCharacter;
    public Gamepad gamepad;

    // constants 
    float jumpForce = 8f;
    float walkSpeed = 7f;
    int maxHealth = 100;

    //int attackDamage = 40;

    public Dictionary<string, int> characterMap = 
        new Dictionary<string, int>()
        { {"Mike", 0}, {"Flav", 1}, {"Henry", 2} };

    public virtual void Start()
    {
        Physics.IgnoreLayerCollision(3, 7);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth();

        currentColHeight = collider.height;
        currentColCenter = collider.center;

        gamepad = Gamepad.current;
    }

    void Update()
    {
        float h = movementInput.x;
        float v = movementInput.y;

        float y = rb.velocity.y;
        bool is_grounded = isGrounded();
        animator.SetBool("is_grounded", is_grounded);


        if (is_crouching && is_grounded)
        {
           rb.velocity = Vector3.zero;
           collider.height = 2.2f;
           collider.center = new Vector3(currentColCenter.x, -1.0f, currentColCenter.z);
        }

        if (is_locked && is_grounded && !is_jumping)
        {
            rb.velocity = Vector3.zero;
        }

        if (!is_crouching && !is_locked)
        {
            Move(moveDirection, y);
            collider.height = currentColHeight;
            collider.center = currentColCenter;
        }
    
        if (moveDirection.x > 0 && face_left)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && !face_left)
        {
            Flip();
        }

        if (is_grounded && is_jumping)
        {
            animator.SetBool("is_jumping", true);
            rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
            is_jumping = false;
            //  StartCoroutine(jump());     

        }
        else if (is_grounded)
        {
            animator.SetBool("is_jumping", false);
        }
        if (y <= -15 && !hasFallen && !is_grounded)
        {
            // I've fallen and I can get up. 
            rb.AddForce(Vector3.up * jumpForce*4, ForceMode.Impulse);
            hasFallen = true;
            this.TakeDamage(currentHealth);
        }
        if (y > -15)
        {
            hasFallen = false;
        }

        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("is_crouching", is_crouching);
    }

    void FixedUpdate()
    {
        bool is_grounded = isGrounded();

        if (is_attacking && is_grounded && !is_crouching)
        {
            Attack();
        }
        //sticksController.setPlayerPos(transform);
    }

    public virtual void Attack() { }

    
    void Move(Vector2 moveDirection, float y)
    {        
        inputVector = new Vector3(moveDirection.x * walkSpeed, y, moveDirection.y * walkSpeed);
        rb.velocity = inputVector;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public bool IsActive()
    {
        return is_active;
    }

    public void MakeActive()
    {
        is_active = true;
    }

    public void MakeInactive()
    {
        is_active = false;
    }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (currentCharacter == characterMap["Henry"] || currentCharacter == characterMap["Flav"])
        {
            if (context.performed)
            {
                is_attacking = true;
            }
            else if (context.canceled)
            {
                is_attacking = false;
            }
        }
        else
        {
            if (context.started)
            {
                is_attacking = true;
            }
            else if (context.canceled)
            {
                is_attacking = false;
            }
        }
    }

    IEnumerator jump()
    {
        yield return new WaitForSeconds(0.6f);
        rb.AddForce(Vector3.up * 0.06f, ForceMode.Impulse);
        is_jumping = false;
    }


    public void onCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            is_crouching = true;
        if (context.canceled)
            is_crouching = false;
    }


    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            is_jumping = true;
        if (context.canceled)
            is_jumping = false;
    }

    public void onLock(InputAction.CallbackContext context)
    {
        if (context.performed)
            is_locked = true;
        if (context.canceled)
            is_locked = false;     
    }

    public virtual void Flip()
    {
        face_left = !face_left;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public bool isGrounded()
    {
        return Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z), collider.radius * .9f, groundLayer);
    }

    public void switchCharacter()
    {
        currentCharacter++;
    }

    public Vector2 getShootDirection()
    {
        if (moveDirection.y == 1.0f && is_locked)
        {
            return transform.up;

        } else if (moveDirection.y == -1.0f && is_locked)
        {
            return transform.up * -1.0f;
        }
        else if (moveDirection.x == 1.0f)
        {
            return transform.right;
        }
        else if (moveDirection.x == -1.0f)
        {
            return transform.right * -1.0f;
        }
        else if ((moveDirection.x > 0 || moveDirection.x < 0) && is_locked) 
        {
            return moveDirection;
        }
        else if (face_left)
        {
            return transform.right * -1.0f;
        }
        return transform.right;  
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = currentHealth - amount;
        healthBar.SetHealth(currentHealth);
        int livesLeft = healthBar.LivesLeft();
        animator.SetTrigger("is_hurt");

        if (currentHealth <= 0)
        {
            if (livesLeft == 1) {
                animator.SetBool("is_dead", true);
                StartCoroutine(Remove());
            }
            else if (livesLeft > 1)
            {
                currentHealth = 100;
                is_hurt = true;
            }
        }
    }
}
