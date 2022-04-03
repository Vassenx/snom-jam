using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxRangeFromTarget;
    [SerializeField] private float maxRangeTilDespawn;
    [SerializeField] private float speed;
    
    [SerializeField] private Attack attacker;
    [SerializeField] private Attack target;

    private Rigidbody rb;

    public void Init(Attack attack)
    {
        this.attacker = attack;
        this.target = attack.curTarget;
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var projPos = transform.position;
        var projToAttacker = (attacker.transform.position - projPos);
        
        /* movement */
        if ((projToAttacker.sqrMagnitude <= maxRangeFromTarget * maxRangeFromTarget) && target != null)
        {
            transform.position = projPos + (speed * Time.deltaTime * projToAttacker.normalized);
        }
        else if(projToAttacker.sqrMagnitude <= maxRangeTilDespawn * maxRangeTilDespawn) // too far from target, run away off screen
        {
            transform.position = projPos + (speed * Time.deltaTime * rb.velocity.normalized);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target == null)
            return;

        if (other.transform == target)
        {
            target.DamageDealt(damage);
            
            // TODO: polish
            if (other.CompareTag("Player"))
            {
            
            }
            else if (other.CompareTag("Boss"))
            {

            }
        }
        
        Destroy(gameObject);
    }
}
