﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;

    float damageCaused = 10f;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }


    
    void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));

        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
            Destroy(gameObject, 0.1f);
        }
    }
}
