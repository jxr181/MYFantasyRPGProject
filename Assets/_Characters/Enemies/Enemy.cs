using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;
using RPG.Weapons;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float secondsBetweenShots;

    [SerializeField] float attackRadius;
    [SerializeField] float chaseRadius;
    [SerializeField] float stopChaseRadius;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
    

    AICharacterControl aiCharacterControl = null;
    float currentHealthPoints;
    GameObject player = null;

    bool isAttacking = false;
    bool isChasing = false;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / (float)maxHealthPoints;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
        currentHealthPoints = maxHealthPoints;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);  // TODO Switch to Coroutines
        }

        if (distanceToPlayer > attackRadius && isAttacking)
        {
            isAttacking = false;
            CancelInvoke();
        }


        if (distanceToPlayer <= chaseRadius && !isChasing)
        {
            isChasing = true;
            aiCharacterControl.SetTarget(player.transform);
        }
        else if (distanceToPlayer >= stopChaseRadius && isChasing)
        {
            isChasing = false;
            aiCharacterControl.SetTarget(transform);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // TODO Seperate out character firing logic
    void FireProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity); // Spawns project at the projectile socket 
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        projectileComponent.SetShooter(gameObject);

        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized; // shoots projectile in direction of the player from the projectileSocket
        float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    private void OnDrawGizmos()
    {
        // Draw Move and Attack Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopChaseRadius);
    }
}
