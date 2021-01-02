using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float explosionDuration = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;

    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip shotSound;
    [SerializeField] [Range(0, 1)] float shotVolume = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownToShot();
    }

    private void CountDownToShot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Shoot();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        } 
    }

    private void Shoot()
    {
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
        shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, shotVolume);

    }

    private void OnTriggerEnter2D(Collider2D elem)
    {
        DamageDealer damageDealer = elem.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HandleHit(damageDealer);
        
    }

    private void HandleHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }
}
