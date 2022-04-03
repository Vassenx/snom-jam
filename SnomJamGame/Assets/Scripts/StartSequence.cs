using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Scenes/StartScene");
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

    public void OnEndGame(bool won)
    {
        if (won)
        {
            bossName.enabled = true;
            bossName.fontStyle = FontStyles.Strikethrough;
            bossName.color = Color.red;
        }
        else
        {
            bossName.enabled = true; // hack
            bossName.text = "DEATH";
            bossName.color = Color.red;
        }

        PauseGame(true);
        StartCoroutine(EndCoroutine());
    }
    
    private IEnumerator EndCoroutine()
    {
        yield return new WaitForSecondsRealtime(bossNameTimeOnScreen);
        bossName.enabled = false;
        PauseGame(false);
        SceneManager.LoadScene("Scenes/StartScene");
    }
    
    private void PauseGame (bool pause)
    {
        if(pause)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }
}
