using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlavController : PlayerController
{
    public GameObject wave;

    public override void Start()
    {
        base.Start();
    }

    public void Reset()
    {
        currentCharacter = characterMap["Flav"];
    }

    public override void Attack()
    {
        animator.SetTrigger("is_attacking");
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3.5f, enemyLayer);
        StartCoroutine(WaveTime());

        if (!is_attacking)
        {
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(10);
            }
        }

    }

    IEnumerator WaveTime()
    {
        yield return new WaitForSeconds(0.3f);

        Vector3 position = transform.position;
        position.y = position.y - 1.25f;

        GameObject waveTransform = Instantiate(wave, position, Quaternion.identity);
        waveTransform.GetComponent<WaveController>().animator.SetTrigger("trigger_anim");

        yield return new WaitForSeconds(0.6f);
        Destroy(waveTransform);
    }

}
