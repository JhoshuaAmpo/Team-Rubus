using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance { get; private set; }
    public bool isGamePaused = false;
    public GameObject PauseMenuHUD;

    [SerializeField]
    [Tooltip("Adds more time to the game being paused between exiting the pause menu and resuming play of the game")]
    private float resumeDelayTime = 0f;
    float prevTimeScale = 0f;

    private GameObject player;
    private CinemachineVirtualCamera playerVC;
    
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        player = GameObject.FindWithTag("Player");
        if (player) {
            playerVC = player.GetComponentInChildren<CinemachineVirtualCamera>();
        }
    }

    public void Pause()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        isGamePaused = true;
        AudioListener.pause = true;
        playerVC.enabled = false;
    }

    public void Resume()
    {
        StartCoroutine(ResumeGameDelay());
        Time.timeScale = prevTimeScale;
        isGamePaused = false;
        AudioListener.pause = false;
        playerVC.enabled = true;
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }
        if(isGamePaused) { 
            Resume();
        }
        else 
        { 
            Pause(); 
        }
    }

    private IEnumerator ResumeGameDelay()
    {
        yield return new WaitForSeconds(resumeDelayTime);
    }
}