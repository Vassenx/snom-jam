using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float cooldownBubbleTime;
    [SerializeField] private float cooldownTime; // TODO: in projectile
    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private HealthBar healthBar;
    public Attack curTarget;  // just add in inspector since only 1 enemy
    private bool isProjectilesInCooldown = false;
    private bool isBubblesInCooldown = false;
    [SerializeField] private AudioSource bubbleAudio;

    [SerializeField] private ParticleSystem bubbles;
    
    // quick polish
    [SerializeField] private StartSequence seq;
    
    private void Start()
    {
        curHealth = maxHealth;
        if (bubbles)
        {
            bubbles.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isProjectilesInCooldown && gameObject.CompareTag("Player"))
        {
            ShootProjectile();
            StartCoroutine(ProjectileCooldownCoroutine());
        }
        else if(bubbles != null && Input.GetMouseButton(1) && !isBubblesInCooldown)
        {
            bubbles.Play();
            bubbleAudio.Play();
            StartCoroutine(BubblesCooldownCoroutine());
        }
        // TODO: sound effect or something to show cant do attack
    }

    private void ShootProjectile()
    {
        Projectile newProj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        newProj.Init(this);
    }
    
    private IEnumerator ProjectileCooldownCoroutine()
    {
        isProjectilesInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isProjectilesInCooldown = false;
    }
    
    private IEnumerator BubblesCooldownCoroutine()
    {
        isBubblesInCooldown = true;
        yield return new WaitForSeconds(cooldownBubbleTime);
        isBubblesInCooldown = false;
    }
    
    public void DamageDealt(float damage)
    {
        if (curHealth <= 0)
            return;
        
        curHealth = Mathf.Clamp(curHealth - damage, 0, maxHealth);
        healthBar.OnHealthChange(curHealth, maxHealth);

        if (curHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Boss"))
        {
            seq.OnEndGame(true);
        }
        else if(gameObject.CompareTag("Player"))
        {
            seq.OnEndGame(false);
        }
    }
}
