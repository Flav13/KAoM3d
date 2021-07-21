using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlavController : PlayerController
{

    public void Reset()
    {
        currentCharacter = characterMap["Flav"];
    }

    public override void Attack()
    {
        animator.SetTrigger("is_attacking");
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3.5f, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(10);
        }
    }

}
