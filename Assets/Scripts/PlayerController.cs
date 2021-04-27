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

    // weapons
    public GameObject sticks;
    public GameObject bullet;
    SticksController sticksController;

    //bools
    public bool face_left = false;
    public bool is_attacking = false;
    public bool is_crouching = false; 
    public bool is_dead = false;
    public bool is_hurt = false;
    public bool is_jumping = false;

    bool hasFallen = false;

    // vars
    int currentHealth;
    float currentColHeight;
    Vector3 currentColCenter;
    Vector2 moveDirection;
    Vector2 shootLocation;
    int currentCharacter;

    // constants 
    float jumpForce = 8f;
    float walkSpeed = 7f;
    int maxHealth = 100;
    //int attackDamage = 40;

    Dictionary<string, int> characterMap = 
        new Dictionary<string, int>()
        { {"Mike", 0}, {"Henry", 1}, {"Flav", 2} };

    void Start()
    {
        Physics.IgnoreLayerCollision(3, 7);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth();
        currentCharacter = characterMap["Mike"];

        currentColHeight = collider.height;
        currentColCenter = collider.center;

        sticksController = sticks.GetComponent<SticksController>();

    }

    void Update()
    {
        float h = movementInput.x;
        float v = movementInput.y;

        float y = rb.velocity.y;
        bool is_grounded = isGrounded();


        if (is_crouching && is_grounded)
        {
           rb.velocity = Vector3.zero;
           collider.height = 1.8f;
           collider.center = new Vector3(currentColCenter.x, -0.65f, currentColCenter.z);
        }

        if (!is_crouching)
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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            is_jumping = false;
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
        animator.SetBool("is_attacking", is_attacking);
        animator.SetBool("is_crouching", is_crouching);
    }

    void Move(Vector2 moveDirection, float y)
    {        
        inputVector = new Vector3(moveDirection.x * walkSpeed, y, moveDirection.y * walkSpeed);
        rb.velocity = inputVector;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (currentCharacter == characterMap["Henry"])
        {
            if (context.started)
            {
                GameObject bTransform = Instantiate(bullet, transform.position, Quaternion.identity);           
                bTransform.GetComponent<BulletController>().Setup(shootLocation);
            }
        }
        else if (currentCharacter == characterMap["Mike"])
        {

            if (context.performed)
            {
                if (!is_attacking)
                {
                    StartCoroutine(throwSticks());
                   
                }
                is_attacking = true;

            }
            else if (context.canceled)
            {
                is_attacking = false;
               // Destroy(sticks);
            }
        }
        else if (currentCharacter == characterMap["Flav"])
        {
            if (context.started)
            {
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3.5f, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyController>().TakeDamage(10);
                }
            }
        }
    }

    IEnumerator throwSticks()
    {
        yield return new WaitForSeconds(0.6f);
        sticks.GetComponent<Renderer>().enabled = true;

      //  sticks.SetActive(true);
        sticksController.StartSpin();

    }

    public void onCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            is_crouching = true;
        if (context.canceled)
            is_crouching = false;
    }

    public void onSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentCharacter < 2)
            {
                currentCharacter++;
            }
            else
            {
                currentCharacter = characterMap["Mike"];
            }
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            is_jumping = true;
        if (context.canceled)
            is_jumping = false;
    }

    public void onAim(InputAction.CallbackContext context)
    {
        shootLocation = context.ReadValue<Vector2>();
        float maxY = 907.0f;
        float maxX = 1609.0f;

        if (Gamepad.current != null)
        {

            if (shootLocation.x < 0)
            {
                shootLocation.x = shootLocation.x * maxX;
            }
            else if (shootLocation.x == 0)
            {
                shootLocation.x = maxX / 2;
            }
            else
            {
                shootLocation.x = shootLocation.x * maxX;
            }
            if (shootLocation.y < 0)
            {
                shootLocation.y = shootLocation.y * maxY;
            }
            else if (shootLocation.y == 0)
            {
                shootLocation.y = maxY;
            }
            else
            {
                shootLocation.y = shootLocation.y * maxY;
            }
        }

    }

    void Flip()
    {
        face_left = !face_left;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        sticksController.Flip();

      
    }

    public bool isGrounded()
    {
        return Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z), collider.radius * .9f, groundLayer);
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
                is_dead = true;
                Debug.Log("mikes ded m8");
                //animator.SetBool("is_dead", is_dead);
            }
            else if (livesLeft > 1)
            {
                currentHealth = 100;
                is_hurt = true;
            }
        }
    }
}
