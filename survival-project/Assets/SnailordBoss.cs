using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnailordBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject targetPlayer;
    [SerializeField] private float chargeAttackPower;
    [SerializeField] private GameObject snailProjectile;
    private bool canDamage; //Flag for if enemy can hurt player. False by default.
    private int damage = 5;
    public SnailordState snailordState;
    [SerializeField] private TMP_Text stateDebugText;
    [SerializeField] private Animator animator;
    public BossHealth bossHealth;
    public GameObject bossHealthbarObject;

    private float jumpSpeed = 20f;

    private bool canAttack; //Flag for if snailord can launch an attack.

    public enum SnailordState
    {
        Sleeping,
        Phase1,
        Phase2,
        Phase3,
        Final
    }

    public void EnableHealthbar()
    {
        bossHealthbarObject.SetActive(true);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get rb reference
        canDamage = false; //Snail can not damage at start.
        snailordState = SnailordState.Sleeping; //Set state to sleeping
        bossHealth.canBossBeHurt = false;
        //bossHealthbarObject.SetActive(false);
        canAttack = true;
    }

    private void Update()
    {
        switch (snailordState)
        {
            case SnailordState.Sleeping:
                //Do nothing except sleeping animation, snailord is waiting to be awoken.
                stateDebugText.text = "Sleeping";
                break;

            case SnailordState.Phase1:
                bossHealth.canBossBeHurt = true;
                stateDebugText.text = "Phase1";
                
                Phase1Logic();
                break;

            case SnailordState.Phase2:
                break;

            case SnailordState.Phase3:
                break;

            case SnailordState.Final:
                break;

            default:
                break;
        }
    }

    private void Phase1Logic()
    {
        if (canAttack == true) //If snailord can attack...
        {
            canAttack = false;
            var num = Random.Range(0, 3); //Get either 0, 1, or 2
            if (num == 0)
            {
                StartCoroutine(ChargeAttack());
            }
            else if (num == 1)
            {
                StartCoroutine(JumpAttack());
            }
            else //If num is 2
            {
                StartCoroutine(SpinProjectileAttack());
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        Debug.Log("Cooldown Start");
        yield return new WaitForSeconds(2f); //Pause for 1 seconds
        Debug.Log("Cooldown End");
        canAttack = true; //Set can attack to true
    }    

    //Snailord picks a random attack
    //After executing, snailord waits a set time (cooldown), before picking another attack.

    //Phase 1
    //Attack 1 - Snailord revs and charges like killer snail
    private IEnumerator ChargeAttack()
    {
        Debug.Log("CHARGE!");
        rb.velocity = Vector3.zero; //Stop his movement
        yield return new WaitForSeconds(1f); //Wait 1 seconds       

        var targetChargePosition = targetPlayer.transform.position;
        Vector2 direction = (targetChargePosition - transform.position).normalized; //Get direction of player
        rb.AddForce(direction * chargeAttackPower, ForceMode2D.Impulse); //Charge at the player
        canDamage = true;

        yield return new WaitForSeconds(1f); //Charge for 1 second

        rb.velocity = Vector3.zero; //Disable movement 
        canDamage = false;

        yield return new WaitForSeconds(.5f); //Pause for half a second
        StartCoroutine(AttackCooldown());
    }

    //Attack 2 - Snailord jumps and tries to land on player. Projectiles around on land
    private IEnumerator JumpAttack()
    {
        Debug.Log("JUMP");

        animator.Play("SnailordLand");
        canDamage = false; //Cant damage during jump

        var targetPosition = targetPlayer.transform.position;
        Vector2 direction = (targetPosition - transform.position).normalized; //Get direction of player
        rb.AddForce(direction * jumpSpeed, ForceMode2D.Impulse); //Jump at player
        
        yield return new WaitForSeconds(2f); //Wait for jump to end

        canDamage = true; //Can damage on land.       
        for (int i = 0; i < 10; i++)
        {
            var direction2 = Random.insideUnitCircle.normalized;
            LaunchSnailProjectile(direction2, 15f);
        }
        animator.Play("Snailord");


        yield return new WaitForSeconds(0.5f); //Wait HALF a second
        canDamage = false;
        StartCoroutine(AttackCooldown());
    }


    //Attack 3 - Snailord spins in place, launching projectiles everywhere.
    private IEnumerator SpinProjectileAttack()
    {
        Pattern1();
        yield return new WaitForSeconds(0.5f); //Wait between volleys
        Pattern2();
        yield return new WaitForSeconds(0.5f); //Wait between volleys
        Pattern1();
        yield return new WaitForSeconds(0.5f); //Wait between volleys
        Pattern2();

        StartCoroutine(AttackCooldown());
    }

    private void Pattern1()
    {
        LaunchSnailProjectile(Vector2.up, 8f); //Up
        LaunchSnailProjectile(Vector2.down, 8f); //Down
        LaunchSnailProjectile(Vector2.left, 8f); //Left
        LaunchSnailProjectile(Vector2.right, 8f); //Right
        LaunchSnailProjectile(new Vector2(1f, 1f).normalized, 8f); //Up Right
        LaunchSnailProjectile(new Vector2(-1f, 1f).normalized, 8f); //Up Left
        LaunchSnailProjectile(new Vector2(1f, -1f).normalized, 8f); //Down Right
        LaunchSnailProjectile(new Vector2(-1f, -1f).normalized, 8f); //Down Left
    }
    private void Pattern2()
    {
        LaunchSnailProjectile(new Vector2(0.5f, 1f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(-0.5f, 1f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(1f, 0.5f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(-1f, 0.5f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(1f, -0.5f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(-1f, -0.5f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(-0.5f, -1f).normalized, 8f);
        LaunchSnailProjectile(new Vector2(0.5f, -1f).normalized, 8f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canDamage == false) return; //Don't do anything if damage can not be done.

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealthScript = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealthScript.invincibile == true) return; //If player is invincible, return and do not take damage
            playerHealthScript.TakeDamage(damage);
        }
    }

    private void LaunchSnailProjectile(Vector2 projectileDirection, float projectileSpeed)
    {
        var go = Instantiate(snailProjectile, transform.position, Quaternion.identity); //Spawn projectile on snailord
        var rb = go.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
        var projScript = go.GetComponent<EnemyProjectile>();
        projScript.enemyProjectileLifetime = 3f;
        projScript.enemyProjectileDamage = damage;
    }
}
