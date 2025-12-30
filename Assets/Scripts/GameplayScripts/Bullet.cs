using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool hasHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.CompareTag("Enemy"))
        {
            hasHit = true;

            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(50);
            }

            Destroy(gameObject);
        }
    }
}
