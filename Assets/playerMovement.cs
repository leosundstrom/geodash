using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float speed = 2;
    [SerializeField] float jump = 7;

    [Header("[Jetpack Settings]")]
    readonly float maxJetpackfuel = 1;
    float jetpackFuel;
    [SerializeField] float jetpackMultiplier = 1.2f;
    [SerializeField] Slider fuelBar;

    [Header("[Ground Check Settings]")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSize = 0.2f;

    Rigidbody2D rb2d;


    void Start()
    {
        jetpackFuel = maxJetpackfuel;
        rb2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb2d.velocity = new Vector2(0, jump);
        }

        //sends player to main menu if dead
        if(Dead())
        {
            SceneManager.LoadSceneAsync(0);
        }
        

        
        JetPack();
    }


    void JetPack()
    {   // If player is not grounded, is pressing jump, and jetpackfuel is biiger than 0 
        if (!IsGrounded() && Input.GetButton("Jump") && jetpackFuel > 0)
        // Adds force instantly
        {
            rb2d.AddForce(Vector2.up * 0.9f, ForceMode2D.Impulse);

            jetpackFuel -= Time.deltaTime * jetpackMultiplier;
        }
        // Checks if player is grounded and adds fuel to the jetpack
        if (IsGrounded())
        {
            if (jetpackFuel < maxJetpackfuel)
            {
                jetpackFuel += Time.deltaTime * jetpackMultiplier;
                // Checks if jetpack fuel is bigger than max 
                if (jetpackFuel > maxJetpackfuel) jetpackFuel = maxJetpackfuel;
            }
        }

        fuelBar.value = jetpackFuel;
    }

    // Makes the player move right 
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(speed * Time.deltaTime * 50, rb2d.velocity.y);
    }

 // Checks player y pos for death
    bool Dead()
    {
        if(transform.position.y < -1)
        {
            return true;
        }

        return false;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Death"))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }


    // Checks if player is grounded by using its position, a circle that has ground check size and a ground layer
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckSize, groundLayer);
    }


}

