using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator anim;


    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            anim.SetBool("Run", true);
            Debug.Log("I can run");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Run", false);
            Debug.Log("No run");
        }


        //Listen for key presses and move right
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            anim.SetBool("Run", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("Jump", false);
        }
    }
   }

