using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnusterBomb : MonoBehaviour
{
    private Vector3 startLocation; //Location bomb spawns at
    public Vector3 endLocation; //Location the bomb will go to
    public enum ExplosionType
    { 
        pattern1,
        pattern2,
    }

    public ExplosionType explosionType;

    private float desiredDuration = 2f; //How long we want it to take the bomb to get to location
    private float elapsedTime;

    //Will move from start to target over time. Use Lerp.

    [SerializeField] private GameObject visualObject; //Object of the bomb sprite
    [SerializeField] private GameObject projectileToSpawn; //This is the projectile the bomb will unleash upon explosion
    [SerializeField] private int projectileDamage; //Damage of the projectile

    private void Awake()
    {
        startLocation = this.transform.position;
        StartCoroutine(PrepareDestruction());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        transform.position = Vector3.Lerp(startLocation, endLocation, percentageComplete);
    }

    private IEnumerator PrepareDestruction()
    {
        yield return new WaitForSeconds(desiredDuration);
        Explode();
    }    


    private void Explode() //Function for the destruction of the projectile.
    {
        switch (explosionType)
        {
            case ExplosionType.pattern1:
                Pattern1();
                break;
            case ExplosionType.pattern2:
                Pattern2();
                break;
            default:
                break;
        }
    }

    private void LaunchProjectile(Vector2 projectileDirection, float projectileSpeed)
    {
        var go = Instantiate(projectileToSpawn, transform.position, Quaternion.identity); //Spawn projectile on snailord
        go.GetComponent<NetworkObject>().Spawn();
        var rb = go.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
        var projScript = go.GetComponent<EnemyProjectile>();
        projScript.enemyProjectileLifetime = 3f;
        projScript.enemyProjectileDamage = projectileDamage;
        projScript.StartDestructionCoroutine(3f);
    }

    private void Pattern1() //4 Cardinal Directions
    {
        //Turn off sprite
        visualObject.SetActive(false);

        //Instantiate projectiles in direction
        LaunchProjectile(new Vector2(0, 1), 8); //Up
        LaunchProjectile(new Vector2(0, -1), 8); //Down
        LaunchProjectile(new Vector2(-1, 0), 8); //Left
        LaunchProjectile(new Vector2(1, 0), 8); //Right

        Destroy(this.gameObject); //Destroy the object
    }

    private void Pattern2() //8 Directional Spreadshot
    {
        //Turn off sprite
        visualObject.SetActive(false);

        LaunchProjectile(new Vector2(0, 1), 8); //Up
        LaunchProjectile(new Vector2(0, -1), 8); //Down
        LaunchProjectile(new Vector2(-1, 0), 8); //Left
        LaunchProjectile(new Vector2(1, 0), 8); //Right
        LaunchProjectile(new Vector2(1f, 1f), 8); //Up Right
        LaunchProjectile(new Vector2(-1f, 1f), 8); //Up Left
        LaunchProjectile(new Vector2(1f, -1f), 8); //Down Right
        LaunchProjectile(new Vector2(-1f, -1f), 8); //Down Left

        Destroy(this.gameObject); //Destroy the object
    }
    

    //This is a special projectile that will be spawned in by enemies
    //Picks a target location
    //Flies there over a certain amount of time (Maybe animation of it going in the air?)
    //Spawns a warning at that location on the ground (Big exclamation circle?)

    //Upon reaching its location, it will detonate and launch a spread of chosen projectile in a direction
}
