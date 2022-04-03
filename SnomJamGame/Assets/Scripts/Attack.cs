using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float cooldownTime; // TODO: in projectile
    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private HealthBar healthBar;
    public Attack curTarget;  // just add in inspector since only 1 enemy
    private bool isInCooldown = false;

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isInCooldown)
        {
            ShootProjectile();
        }
        else
        {
            // TODO: sound effect or something to show cant
        }
    }

    public void DamageDealt(float damage)
    {
        curHealth = Mathf.Clamp(curHealth - damage, 0, maxHealth);
        healthBar.OnHealthChange(curHealth, maxHealth);
    }
    
    private void ShootProjectile()
    {
        Projectile newProj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        newProj.Init(this);
        StartCoroutine(CooldownCoroutine());
    }
    
    private IEnumerator CooldownCoroutine()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;
    }
}
