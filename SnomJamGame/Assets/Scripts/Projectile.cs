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
        transform.forward = attack.transform.forward;
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var projPos = transform.position;
        var projToTarget = (target.transform.position - projPos);
        float dotToTarget = Vector3.Dot(target.transform.forward, transform.forward); // angle/FOV
        
        /* movement */
        if ((projToTarget.sqrMagnitude <= maxRangeFromTarget * maxRangeFromTarget) && dotToTarget < 0.2) // move to boss
        {
            transform.position = projPos + (speed * Time.deltaTime * projToTarget.normalized);
        }
        else if(projToTarget.sqrMagnitude <= maxRangeTilDespawn * maxRangeTilDespawn) // too far from target, run away off screen
        {
            var dir = rb.velocity.normalized;
            if (rb.velocity.sqrMagnitude < 0.1)
                dir = attacker.transform.forward.normalized;
            
            transform.position = projPos + (speed * Time.deltaTime * dir);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(target == null)
            return;

        if (col.transform == target.transform)
        {
            target.DamageDealt(damage);
            
            // TODO: polish
            if (col.gameObject.CompareTag("Player"))
            {
            
            }
            else if (col.gameObject.CompareTag("Boss"))
            {

            }
        }
        
        Destroy(gameObject);
    }
}
