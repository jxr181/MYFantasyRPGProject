using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject shooter; // So can inspect when paused

    const float DESTROY_DELAY = 0.01f;
    float damageCaused = 10f;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    public float GetDefaultLaunchSpeed()
    {
        return projectileSpeed;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        var layerCollidedWith = collision.gameObject.layer;
        if (collision.gameObject.layer != shooter.layer)
        {
            DamageIfDamageable(collision);
        }
        
    }

    void DamageIfDamageable(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }

        Destroy(gameObject, DESTROY_DELAY);
    }
}
