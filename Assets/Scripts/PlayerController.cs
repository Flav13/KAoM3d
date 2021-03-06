using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider collider;
    public HealthBar healthBar;

    public Animator animator;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public GameObject sticks;
    SticksController sticksController;

    public bool face_left = false;
    public bool is_attacking = false;
    public bool is_crouching = false; 
    public bool is_dead = false;
    public bool is_hurt = false;

    Vector3 inputVector;
    int currentHealth;
    float currentColHeight;
    Vector3 currentColCenter;

    float speed = 7f;
    float jump_force = 8f;
    int maxHealth = 100;
    //int attackDamage = 40;

    bool attack_running;
    bool hasFallen = false;


    void Start()
    {
        Physics.IgnoreLayerCollision(3, 7);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth();
        currentColHeight = collider.height;
        currentColCenter = collider.center;

        sticksController = sticks.GetComponent<SticksController>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = rb.velocity.y;
        bool is_grounded = isGrounded();


        if (Input.GetKeyDown(KeyCode.X) && is_grounded)
        {
            is_crouching = true;
            rb.velocity = Vector3.zero;
            collider.height = 1.8f;
            collider.center = new Vector3(currentColCenter.x,- 0.65f, currentColCenter.z);
        }

        if (!is_crouching)
        {
            inputVector = new Vector3(x * speed, y, Input.GetAxisRaw("Vertical") * speed);
            rb.velocity = inputVector;
        }

        if (Input.GetAxis("Horizontal") > 0 && face_left)
        {
            Flip();
        }
        else if (Input.GetAxis("Horizontal") < 0 && !face_left)
        {
            Flip();
        }

        if (is_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        }

        if (y <= -15 && !hasFallen && !is_grounded)
        {
            // I've fallen and I can get up. 
            rb.AddForce(Vector3.up * jump_force*4, ForceMode.Impulse);
            hasFallen = true;
            this.TakeDamage(currentHealth);

        }
        if (is_grounded)
        {
            hasFallen = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !attack_running && sticksController.withPlayer())
        {
            is_attacking = true;
            StartCoroutine(AttackWait());
            StartCoroutine(SpinSticks());

        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            is_attacking = false;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            is_crouching = false;
            collider.height = currentColHeight;
            collider.center = currentColCenter;
        }

        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("is_attacking", is_attacking);
        animator.SetBool("is_crouching", is_crouching);
    }

    void Flip()
    {
        face_left = !face_left;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        sticksController.Flip();
        
    }

    IEnumerator AttackWait()
    {
        attack_running = true;
        yield return new WaitForSeconds(0.15f);
        attack_running = false;

        sticksController.setState(SticksController.State.Recalling);
    }

    IEnumerator SpinSticks()
    {
        sticksController.setState(SticksController.State.Thrown);
        sticksController.setSpin(true);

        yield return new WaitForSeconds(0.5f);

        sticksController.setSpin(false);
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
