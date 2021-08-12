using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{

    public LayerMask groundLayer;
    public LayerMask characterLayer;

    public Animator animator;
    public Rigidbody rb;

    public Transform attackPoint;
    public float attackRange = 1.0f;

    public bool face_left = false;
    public bool is_attacking = false;
    public bool is_dead = false;
    public bool is_hurt = false;

    Transform flav;
    Transform mike;
    Transform Player;


    int currentHealth;

    int speed = 4;
    float jump_force = 8f;
    int maxHealth = 100;
    float MinDist = 1.5f;
    int attackDamage = 200;

    bool attack_running;

    void Start()
    {
        currentHealth = maxHealth;
        Physics.IgnoreLayerCollision(7, 3);

    }

    Transform GetActivePlayer()
    {
        GameObject parentClass = GameObject.Find("Player");
        mike = parentClass.transform.Find("mike");
        flav = parentClass.transform.Find("flav");

        if (mike.GetComponent<PlayerController>().IsActive())
        {
            return mike;
        }
        else 
        {
            return flav;
        }

    }

    void Update()
    {
        Player = GetActivePlayer();

        is_attacking = false;

        if (!IsFacingRight(transform, Player) && !face_left)
        {
            Flip();
        }
        else if (IsFacingRight(transform, Player) && face_left)
        {
            Flip();
        }
        if (Vector3.Distance(transform.position, Player.position) <= MinDist && !is_dead && !attack_running)
        {
            is_attacking = true;
            StartCoroutine(AttackWait());
        }
        else if (!is_dead && !attack_running)
        {
            Vector3 moveDir = (Player.position - transform.position).normalized;
            var step = speed * Time.deltaTime;
            transform.position += moveDir * step;
        }

        animator.SetBool("is_attacking", is_attacking);
    }

    void Attack()
    {
        Collider[] hitCharacters = Physics.OverlapSphere(attackPoint.position, attackRange, characterLayer);

        foreach (Collider character in hitCharacters)
        {
            character.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    IEnumerator AttackWait()
    {
        attack_running = true;
        yield return new WaitForSeconds(1);
        Attack();
        attack_running = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Stick" && col.gameObject.GetComponent<SticksController>().getState() != SticksController.State.WithPlayer)
        {
            this.TakeDamage(10);
        }
        else if (col.gameObject.name.Contains("Bullet"))
        {
            this.TakeDamage(5);
            col.gameObject.GetComponent<BulletController>().Remove();
        }
        else if (col.gameObject.name.Contains("Wave"))
        {
          //  this.TakeDamage(10);
          //  col.gameObject.GetComponent<WaveController>().Remove();
        }
        Debug.Log(currentHealth);
    }

    void OnCollisionEnter(Collision collision)
    {
      
        Vector3 normal = collision.GetContact(0).normal;

        if (normal == transform.forward)
        {
            rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        }
        else if (normal == -(transform.forward))
        {
            rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        }

        else if (normal == transform.right)
        {
            rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        }

        else if (normal == -(transform.right))
        {
            rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        } 
    }

    bool IsFacingRight(Transform enemy, Transform player)
    {
        return enemy.position.x < player.position.x;
    }

    void Flip()
    {
        face_left = !face_left;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = currentHealth - amount;

        if (currentHealth <= 0)
        {
            is_dead = true;   
            animator.SetBool("is_dead", is_dead);
            Destroy(gameObject, 0.70f);
        }
        else
        {
            is_hurt = true;
            animator.SetTrigger("is_hurt");
        }
    }
}