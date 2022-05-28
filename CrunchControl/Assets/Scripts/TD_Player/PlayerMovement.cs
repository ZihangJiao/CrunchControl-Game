using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float playerDirection; 

    Vector2 playerMovement;
    public float playerVertical;
    public float playerHorizontal;


    public Rigidbody2D playerRB;

    public Animator playerAnim;

   // public int HP = 5;

    
    void Update()
    {
        //Player Input
        playerHorizontal = Input.GetAxisRaw("Horizontal");
        playerVertical   = Input.GetAxisRaw("Vertical");

        //Communicate's with player animator to control current animation
        playerAnim.SetFloat("Vertical", playerVertical);
        playerAnim.SetFloat("Horizontal", playerHorizontal);

        //translates player input into a vector 2 to find out if player is moving
        playerMovement.x = playerHorizontal;
        playerMovement.y= playerVertical;
        playerAnim.SetFloat("Speed", playerMovement.sqrMagnitude); //sqr magnitude makes sure the value is positive

        //stores the direction the player is facing for idle animation
        playerAnim.SetFloat("FacingDirection", playerDirection);
    }

    private void FixedUpdate()
    {
        Movement();   
    }

    private void Movement()
    {
        //checks player horizontal value and moves player appropiately. Also updates player direction
        if(playerHorizontal > 0)
        {
            playerDirection = 2;
            playerRB.MovePosition(playerRB.position + new Vector2(playerHorizontal * playerSpeed * Time.deltaTime, 0f));
        }

        else if (playerHorizontal < 0)
        {
            playerDirection = 1;
            playerRB.MovePosition(playerRB.position + new Vector2(playerHorizontal * playerSpeed * Time.deltaTime, 0f));
        }

        // checks player vertical value and moves player appropiately. Also updates player direction
        else if (playerVertical > 0)
        {
            playerDirection = 0;
            playerRB.MovePosition(playerRB.position + new Vector2( 0f, playerVertical * playerSpeed * Time.deltaTime));
        }

        else if (playerVertical < 0)
        {
            playerDirection = 3;
            playerRB.MovePosition(playerRB.position + new Vector2(0f, playerVertical * playerSpeed * Time.deltaTime));
        }
    }




}
