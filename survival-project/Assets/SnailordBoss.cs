using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class SnailordBoss : MonoBehaviour
{
    //JUST SOME COMMENTS FOR FUTURE
    //DUST TRAIL WHEN DASHING and SONIC EFFECT WHEN CHARGING

    private Rigidbody2D rb;
    public GameObject targetPlayer;
    [SerializeField] private float chargeAttackPower;
    [SerializeField] private int chargeAttackDamage;
    [SerializeField] private GameObject snailProjectile;
    [SerializeField] private GameObject snusterBomb;
    [SerializeField] private float projectileSpeed;
    private bool canDamage; //Flag for if enemy can hurt player. False by default.
    [SerializeField] private int projectileDamage = 5;
    public SnailordState snailordState;
    [SerializeField] private TMP_Text stateDebugText;
    [SerializeField] private Animator animator;
    
    public BossHealth bossHealth;
    public GameObject bossHealthbarObject;

    [SerializeField] private GameObject rotateTrapAnchor;
    [SerializeField] private GameObject projectileRotateAnchor;
    [SerializeField] private GameObject objectPool;
    [SerializeField] private GameObject snailordSpriteObject;
    [SerializeField] private float degreesPerSecondRotate;

    [SerializeField] private float degreesPerSecondTRAPRotate;

    private bool isReving;
    private bool isCharging;
    private bool isTrapping = false; //Bool for if trapping player

    int attackNum = 0;
    int prevNum;

    private float jumpSpeed = 20f;

    private bool canAttack; //Flag for if snailord can launch an attack.

    //Stuff to be done
    //Phase transitions

    //Intro, snailord roars and the music comes in
    //Maybe have health bar cinematically fill up? Look at Kirby for inspiration on that.

    //Phase 1 > Phase 2
    //Snailord becomes invincible for a second, roars again, and attacks

    //Phase 2 > 3
    //Snailord looks like he is about to die before roaring way louder.
    //he turns slightly red and his health bar fills back up.

    public enum SnailordState
    {
        Sleeping,
        Phase1,
        Phase2,
        Phase3,
        Phase4,
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

    private void PhaseCheck()
    {
        float halfHealth = bossHealth.maxHealth / 2; //Get half health. Its half of the max health
        if (bossHealth.currentHealth <= halfHealth && bossHealth.isBerserk == false) //If boss is at half health
        {
            snailordState = SnailordState.Phase2; //Set state to Phase 2.
        }
        if (bossHealth.currentHealth <= halfHealth && bossHealth.isBerserk == true) //If boss is at half health
        {
            snailordState = SnailordState.Phase4; //Set state to Phase 2.
        }

    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return; //If collider hit info doesn't exist return
        
        if (hitInfo.CompareTag("Player"))
        {
            if (canDamage == false) return; //If can't damage return
            PlayerHealth playerHealth = hitInfo.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.invincibile == false)
            {
                playerHealth.TakeDamage(chargeAttackDamage);
            }
        }
    }

    private void Update()
    {
        
        PhaseCheck();
        if (isTrapping == true)
        {
            this.transform.Rotate(new Vector3(0, 0, degreesPerSecondTRAPRotate) * Time.deltaTime);
        }

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

                projectileRotateAnchor.transform.Rotate(new Vector3(0, 0, degreesPerSecondRotate) * Time.deltaTime);
                bossHealth.canBossBeHurt = true;
                stateDebugText.text = "Phase2";

                Phase2Logic();
                break;

            case SnailordState.Phase3:
                projectileRotateAnchor.transform.Rotate(new Vector3(0, 0, degreesPerSecondRotate) * Time.deltaTime);
                stateDebugText.text = "Phase3";

                Phase3Logic();
                break;

            case SnailordState.Phase4:
                stateDebugText.text = "Phase4";
                break;

            case SnailordState.Final:
                break;

            default:
                break;
        }
    }

    private void Phase1Logic()
    {
        //TREVOR SUGGESTION - CANNOT REPEAT ATTACK
        //Store the previous attack int rolled. If next one is same as last one, roll again.

        if (canAttack == true) //If snailord can attack...
        {
            canAttack = false;
            prevNum = attackNum; //Prev num is previous attack num
            attackNum = Random.Range(0, 3); //Get the num

            while (attackNum == prevNum) //if attack num is equal to prev num
            {
                attackNum = Random.Range(0, 3); //Roll till its not
            }
            
            if (attackNum == 0)
            {
                StartCoroutine(ChargeAttack());
            }
            else if (attackNum == 1)
            {
                StartCoroutine(JumpAttack());
            }
            else //If num is 2
            {
                StartCoroutine(SpinProjectileAttack());
            }
        }
    }

    private void Phase2Logic()
    {
        if (canAttack == true)
        {
            canAttack = false;
            attackNum = Random.Range(0, 3);
            if (attackNum == 0)
            {
                //Triple Snail Charge
                //Snailord revs up before charging 3 times, with a short delay between each.
                StartCoroutine(TripleChargeAttack());
            }
            else if (attackNum == 1)
            {
                //Triple Snail Stomp

                //Snailord stomps 3 times in a row. The final stomp has increased radius, height, and projectile.
                StartCoroutine(TripleJumpAttack());
            }
            else
            {
                //Snailord Super Spinout

                //This time, snailord spins out with faster speed, releasing more projectiles. Also throw bombs at player.
                StartCoroutine(SnailSuperSpinout());
            }
        }    
    }

    private void Phase3Logic()
    {
        if (canAttack == true)
        {
            canAttack = false;
            var num = Random.Range(0, 3);
            if (num == 0)
            {
                //Snailord revs up and charges, then jumps, then charges, then jumps, then charges,
                //followed by a final super jump where he unleashes a storm of projectiles around him.
                //Each charge also releases some projectiles around him.
                StartCoroutine(Phase3ChargeAttack1());
            }
            else if (num == 1)
            {
                //Snailord spins out rapidly in a large circle around the player at fast speed, trapping them inside his zone.
                //While doing this, projectiles are launched from around the circle towards the player, and you have to dodge inside the area. 
                StartCoroutine(SnailTurboTrap());
            }
            else
            {
                //Snailord spins in place and rapidly launches projectiles everywhere.
                //After a few seconds, he charges to another spot nearby, and does it again. He does this 3 times.
                StartCoroutine(SnailordPhase3Attack3TEST());
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        canDamage = false;
        Debug.Log("Cooldown Start");
        yield return new WaitForSeconds(2f); //Pause for 1 seconds
        Debug.Log("Cooldown End");
        canAttack = true; //Set can attack to true
    }    

    //Attack 1 - Snailord revs and charges like killer snail
    private IEnumerator ChargeAttack()
    {
        Debug.Log("CHARGE!");
        rb.velocity = Vector3.zero; //Stop his movement
        yield return new WaitForSeconds(1f); //Wait 1 seconds
                                             
        canDamage = true;
        var targetChargePosition = targetPlayer.transform.position;
        Vector2 direction = (targetChargePosition - transform.position).normalized; //Get direction of player
        rb.AddForce(direction * chargeAttackPower, ForceMode2D.Impulse); //Charge at the player
        yield return new WaitForSeconds(1f); //Charge for 1 second

        rb.velocity = Vector3.zero; //Disable movement 
        

        yield return new WaitForSeconds(.5f); //Pause for half a second
        canDamage = false;
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

        Pattern1();
        Pattern2();

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

    private IEnumerator TripleChargeAttack()
    {
        Debug.Log("TRIPLE CHARGE!");
        rb.velocity = Vector3.zero; //Stop his movement
        yield return new WaitForSeconds(1f); //Wait 1 seconds
        canDamage = true;

        for (int i = 0; i < 3; i++) //Charge 3 Times
        {
            var targetChargePosition = targetPlayer.transform.position;
            Vector2 direction = (targetChargePosition - transform.position).normalized; //Get direction of player
            rb.AddForce(direction * chargeAttackPower, ForceMode2D.Impulse); //Charge at the player
            yield return new WaitForSeconds(1f); //Charge for 1 second
            rb.velocity = Vector3.zero; //Disable movement 
            yield return new WaitForSeconds(.5f); //Pause for half a second
        }
         
        canDamage = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator TripleJumpAttack()
    {
        Debug.Log("TRIPLE JUMP!");

        for (int i = 0; i < 3; i++) //Charge 3 Times
        {
            animator.Play("SnailordLand");
            canDamage = false; //Cant damage during jump
            var targetPosition = targetPlayer.transform.position;
            Vector2 direction = (targetPosition - transform.position).normalized; //Get direction of player
            rb.AddForce(direction * jumpSpeed, ForceMode2D.Impulse); //Jump at player
            yield return new WaitForSeconds(2f); //Wait for jump to end
            canDamage = true; //Can damage on land.

            Pattern1();
            Pattern2();

            animator.Play("Snailord");
            yield return new WaitForSeconds(0.5f); //Wait HALF a second
            canDamage = false;
        }
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator Phase3ChargeAttack1()
    {
        Debug.Log("Super Charges");
        isReving = false;
        isCharging = false;

        for (int i = 0; i < 3; i++) //Charge 3 Times
        {
            Debug.Log("Start Revving");
            //Rev Up
            //Launch Projectiles
            rb.velocity = Vector3.zero; //Stop his movement       
            isReving = true;
            StartCoroutine(revCooldown());

            while (isReving == true)
            {
                Debug.Log("REVING!");
                var direction = projectileRotateAnchor.transform.up;
                var direction2 = -direction; //Opposite of direction 1
                LaunchSnailProjectile(direction, projectileSpeed, false);
                LaunchSnailProjectile(direction2, projectileSpeed, false);
                yield return new WaitForSeconds(0.1f); //Wait 0.1 seconds between each projectile
            }
            
            canDamage = true;
            //Once rev is done
            //Charge
            //Launch Projectiles on his sides while dashing.
            Debug.Log("Start Charge");
            var targetChargePosition = targetPlayer.transform.position;
            Vector2 direction3 = (targetChargePosition - transform.position).normalized; //Get direction of player
            rb.AddForce(direction3 * chargeAttackPower, ForceMode2D.Impulse); //Charge at the player
            isCharging = true;

            StartCoroutine(chargeCooldown());

            while (isCharging == true)
            {
                Debug.Log("CHARGING!");
                LaunchSnailProjectile(Vector2.up, projectileSpeed, false);
                LaunchSnailProjectile(Vector2.down, projectileSpeed, false);
                yield return new WaitForSeconds(0.2f); //Wait 0.2 seconds between each projectile
            }
            Debug.Log("Charge Done!");
            rb.velocity = Vector3.zero; //Disable movement 
            yield return new WaitForSeconds(.5f); //Pause for half a second
        }
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator revCooldown()
    {
        yield return new WaitForSeconds(2f);
        isReving = false;
    }

    private IEnumerator chargeCooldown()
    {
        yield return new WaitForSeconds(1f);
        isCharging = false;
    }

    private IEnumerator trapCooldown()
    {
        yield return new WaitForSeconds(8f);
        isTrapping = false;
    }


    private void Pattern1()
    {
        LaunchSnailProjectile(Vector2.up, projectileSpeed, false); //Up
        LaunchSnailProjectile(Vector2.down, projectileSpeed, false); //Down
        LaunchSnailProjectile(Vector2.left, projectileSpeed, false); //Left
        LaunchSnailProjectile(Vector2.right, projectileSpeed, false); //Right
        LaunchSnailProjectile(new Vector2(1f, 1f).normalized, projectileSpeed, false); //Up Right
        LaunchSnailProjectile(new Vector2(-1f, 1f).normalized, projectileSpeed, false); //Up Left
        LaunchSnailProjectile(new Vector2(1f, -1f).normalized, projectileSpeed, false); //Down Right
        LaunchSnailProjectile(new Vector2(-1f, -1f).normalized, projectileSpeed, false); //Down Left
    }
    private void Pattern2()
    {
        LaunchSnailProjectile(new Vector2(0.5f, 1f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(-0.5f, 1f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(1f, 0.5f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(-1f, 0.5f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(1f, -0.5f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(-1f, -0.5f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(-0.5f, -1f).normalized, projectileSpeed, false);
        LaunchSnailProjectile(new Vector2(0.5f, -1f).normalized, projectileSpeed, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canDamage == false) return; //Don't do anything if damage can not be done.

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealthScript = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealthScript.invincibile == true) return; //If player is invincible, return and do not take damage
            playerHealthScript.TakeDamage(chargeAttackDamage);
        }
    }

    private void LaunchSnailProjectile(Vector2 projectileDirection, float projectileSpeed, bool isTrapping)
    {
        if (isTrapping == false)
        {
            var go = Instantiate(snailProjectile, transform.position, Quaternion.identity); //Spawn projectile on snailord
            go.GetComponent<NetworkObject>().Spawn();
            var rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
            var projScript = go.GetComponent<EnemyProjectile>();
            projScript.enemyProjectileLifetime = 3f;
            projScript.enemyProjectileDamage = projectileDamage;
            projScript.StartDestructionCoroutine(3f);
        }
        else
        {
            var go = Instantiate(snailProjectile, snailordSpriteObject.transform.position, Quaternion.identity); ; //Spawn projectile on snailord
            go.GetComponent<NetworkObject>().Spawn();
            var rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
            var projScript = go.GetComponent<EnemyProjectile>();
            projScript.enemyProjectileLifetime = 3f;
            projScript.enemyProjectileDamage = projectileDamage;
            projScript.StartDestructionCoroutine(3f);
        }          
    }

    private IEnumerator SnailSuperSpinout()
    {
        Debug.Log("SUPER SPINOUT!");

        for (int i = 0; i < 50; i++) //Launch 50 projectiles
        {
            var direction = projectileRotateAnchor.transform.up;
            var direction2 = -direction; //Opposite of direction 1
            LaunchSnailProjectile(direction, projectileSpeed, false);
            LaunchSnailProjectile(direction2, projectileSpeed, false);
            yield return new WaitForSeconds(0.1f); //Wait 0.2 seconds between each projectile
        }
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator SnailTurboTrap()
    {
        Debug.Log("TURBO TRAP");
        var snailordCurrentPos = this.transform.position; //Get where snail is before moving

        //TO DO - ADD ANIMATION OF SNAILORD MOVING TO TARGET POSITION INSTEAD OF HIM TELEPORTING THERE 
        
        var snailordTargetPos = new Vector3(targetPlayer.transform.position.x + 10, targetPlayer.transform.position.y, 0);
        this.transform.position = targetPlayer.transform.position; //Set actual boss object to be on player

        snailordSpriteObject.transform.position = snailordTargetPos; //Set sprite object to be where it needs to be (right of player).
        
        yield return new WaitForSeconds(1f); //Wait 1 second then start rotating

        isTrapping = true;

        //TO DO - MAKE SPRITE HURT PLAYER, NOT CORE

        StartCoroutine(trapCooldown());

        while (isTrapping == true)
        {
            var dir = (targetPlayer.transform.position - snailordSpriteObject.transform.position).normalized;
            LaunchSnailProjectile(dir, (projectileSpeed*0.8f), true);
            yield return new WaitForSeconds(0.2f); //Wait 0.2 seconds between each projectile
        }

        //canDamage = false;

        var spriteCurrentPosition = snailordSpriteObject.transform.position;
        this.transform.position = spriteCurrentPosition;
        snailordSpriteObject.transform.position = this.transform.position;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator SnailordPhase3Attack3TEST()
    {
        //This attack is the spin storm
        //Snailord spins in place and launches projectiles like the other attack
        for (int i = 0; i < 5; i++) //Launch 5 projectiles
        {
            var targetLocation = targetPlayer.transform.position;
            var go = Instantiate(snusterBomb, this.transform.position, Quaternion.identity);
            var bombScript = go.GetComponent<SnusterBomb>();
            bombScript.endLocation = targetLocation;        
            yield return new WaitForSeconds(2f); //Wait 2 seconds between each projectile
        }
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator SnailordPhase3Attack3REAL()
    {
        //This attack is the spin storm
        //Snailord spins in place and launches projectiles like the other attack
        for (int i = 0; i < 100; i++) //Launch 100 projectiles
        {
            var direction = projectileRotateAnchor.transform.up;
            var direction2 = -direction; //Opposite of direction 1
            LaunchSnailProjectile(direction, projectileSpeed, false);
            LaunchSnailProjectile(direction2, projectileSpeed, false);
            yield return new WaitForSeconds(0.1f); //Wait 0.2 seconds between each projectile
        }
        StartCoroutine(AttackCooldown());

        //However, the projectiles are more dangerous, and he also launches Snuster Bombs at the player
        //Snuster Bomb - new projectile
    }

    private IEnumerator spinAttackP3A3()
    {
        for (int i = 0; i < 100; i++) //Launch 100 projectiles
        {
            var direction = projectileRotateAnchor.transform.up;
            var direction2 = -direction; //Opposite of direction 1
            LaunchSnailProjectile(direction, projectileSpeed, false);
            LaunchSnailProjectile(direction2, projectileSpeed, false);
            yield return new WaitForSeconds(0.1f); //Wait 0.2 seconds between each projectile
        }
    }

    //While Spinning (Attack 3)
    //Launch Projectiles like before ANd Snuster Bombs.

    //private void LaunchRotatingProjectile(Vector2 projectileDirection, float projectileSpeed)
    //{
    //    var go = Instantiate(snailProjectile, transform.position, Quaternion.identity); //Spawn projectile on snailord
    //    go.GetComponent<NetworkObject>().Spawn();
    //    //go.transform.parent = projectileRotateAnchor.transform;
    //    var rb = go.GetComponent<Rigidbody2D>();
    //    rb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
    //    var projScript = go.GetComponent<EnemyProjectile>();
    //    projScript.enemyProjectileLifetime = 3f;
    //    projScript.enemyProjectileDamage = damage;
    //    projScript.StartDestructionCoroutine(3f);
    //}
}
