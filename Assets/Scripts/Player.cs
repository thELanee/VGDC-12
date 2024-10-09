using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{

    // Just movement for now

    // TODO: Let player sprint (Done I think...), lock player movement while in dialogue (TO DO), make camera follow the player (Done)
    //       Also need a walk cycle animation once we have a design for the protag (I don't think that has to be in code) (sprint and idle animation optional)

    public float speed = 5f;
    public float sprint = 10f;
    public float playerSpeed;
    public Rigidbody2D player;
    public Animator playerAnimator;

    Vector2 velocity;

    void Start()
    {
    }

    void Update() 
    {
       
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = speed + sprint;
        }
        else    {
            playerSpeed = speed;
    }
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

       // if (velocity != Vector2.zero) {
        
        //playerAnimator.SetFloat("Horizontal", velocity.x);
        //playerAnimator.SetFloat("Vertical", velocity.y);

    }

    //playerAnimator.SetFloat("Speed", velocity.sqrMagnitude);
    
    //}

    void FixedUpdate()
    {
        player.MovePosition(player.position + velocity * playerSpeed * Time.fixedDeltaTime);
    }
}  
