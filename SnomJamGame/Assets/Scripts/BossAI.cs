using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Transform caveTarget;
    [SerializeField] private float swimSpeed = 10f;
    private Transform target;
    private Rigidbody rb;
    
    /* Grasp */
    private bool inGraspPhase;

    float timeElapsed;
    float lerpDuration = 3;
    float startValue=0;
    float endValue=10;
    
    private void Start()
    {
        inGraspPhase = false;
        rb = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        if (inGraspPhase)
        {
            FaceTarget();
            GoToTarget();
        }
    }

    public void Grasp(Transform newTarget)
    {
        target = newTarget;
        inGraspPhase = true; 
    } 

    public void GoToTarget()
    {
        rb.AddForce(transform.forward * swimSpeed, ForceMode.VelocityChange);
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        if (direction.sqrMagnitude > 0.25f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && inGraspPhase && target != null)
        {
            Attack targetAttack = target.GetComponent<Attack>();
            if (targetAttack)
            {
                targetAttack.DamageDealt(20f);

                target = caveTarget;
                GoToTarget();
            }
            inGraspPhase = false;
        }
    }
}
