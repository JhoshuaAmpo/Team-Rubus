using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private HUDBar hpBar;

    [Header("Decay")]
    [SerializeField]
    [Tooltip("Health Loss Per Second")]
    private float rateOfDecay;

    private float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        hpBar.SetBar(currentHP/ maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        Decay();   
    }

    private void Decay()
    {
        DecreaseHealth(rateOfDecay * Time.deltaTime);
    }

    public void DecreaseHealth(float dmg) {
        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        hpBar.SetBar(currentHP/ maxHP);
        if (currentHP <= 0) {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        Debug.Log("Player Has Died!");
    }
}
