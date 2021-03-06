using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{

    public GameObject enemy;
    float xPos;
    float zPos;
    int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 1)
        {
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 13.0f));
            enemy.layer = 7;
            enemy.name = "Enemy";
            Instantiate(enemy, v3Pos, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            enemyCount += 1;

        }
    }
}
