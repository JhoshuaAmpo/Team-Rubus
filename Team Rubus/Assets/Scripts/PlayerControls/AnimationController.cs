using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator anim;


    Rigidbody rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();

        if (spriteRenderer == null || anim == null)
        {
            Debug.LogError("SpriteRenderer or Animator is not assigned!");
        }
    }

    void Update()
    {
        //if (rb == null) return; // Exit if Rigidbody is not found

        float speedX = rb.velocity.x;
        float speedY = rb.velocity.y;

        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            anim.SetBool("Run", true);
        } else if ((Input.GetKeyUp(KeyCode.A)))
        {
            anim.SetBool("Run", false);
        }


        //Listen for key presses and move right
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            anim.SetBool("Run", true);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Run", false);
        }




        /*
        // Check if the Rigidbody is moving on the x axis
        if (Mathf.Abs(speedX) > Mathf.Epsilon)
        {
            if (speedX > 0)
            {
                Debug.Log("Rigidbody is moving in the +x direction at speed: " + speedX);
                spriteRenderer.flipX = false;
                //anim.SetBool("Run", true);
                anim.Play("run");
            }
            else if (speedX < 0)
            {
                Debug.Log("Rigidbody is moving in the -x direction at speed: " + speedX);
                spriteRenderer.flipX = true;
                //anim.SetBool("Run", true);
                anim.Play("run");
            }
        }
        else
        {
            //anim.SetBool("Run", false); // Stop running if no movement on x-axis
            anim.Play("idle");
        }

        // Check if the Rigidbody is moving on the y axis
        if (Mathf.Abs(speedY) > Mathf.Epsilon)
        {
            if (speedY > 0)
            {
                Debug.Log("Rigidbody is moving in the +y direction at speed: " + speedY);
            }
            else
            {
                Debug.Log("Rigidbody is moving in the -y direction at speed: " + speedY);
            }
        }
        else
        {
            Debug.Log("Rigidbody is not moving on the y axis.");
        }*/
    }
}