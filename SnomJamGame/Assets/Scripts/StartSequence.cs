using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSequence : MonoBehaviour
{
    [SerializeField] private Transform boss;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private float bossNameTimeOnScreen = 5f;
    [SerializeField] private Animator animator;
    private bool doneOnce = false;
    private bool bossInView = false;
    private bool startCam = true;

    private Camera cam;
    
    private void Start()
    {
        startCam = true;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!doneOnce)
        {
            StartCoroutine(CooldownCoroutine());
        }

        if (!startCam)
        {
            Vector3 bossInViewportPoint = cam.WorldToViewportPoint(boss.position);
            bool curBossInView = bossInViewportPoint.x >= 0 && bossInViewportPoint.x <= 1 &&
                                 bossInViewportPoint.y >= 0 && bossInViewportPoint.y <= 1 &&
                                 bossInViewportPoint.z >= 0;
        
            if (curBossInView != bossInView)
            {
                bossInView = curBossInView;
                SwitchState();
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        bossName.enabled = true;
        animator.Play("StartBossCamState");
        yield return new WaitForSeconds(bossNameTimeOnScreen);
        bossName.enabled = false;
        startCam = false;
        doneOnce = true;
    }
    
    private void SwitchState()
    {
        if (bossInView)
        {
            animator.Play("PlayerAndBossCamState");
        }
        else
        {
            animator.Play("PlayerCamState");
        }
    }
}
