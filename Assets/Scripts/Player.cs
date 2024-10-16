using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{

    // Just movement for now

    // TODO: Let player sprint (Done I think...), lock player movement while in dialogue, make camera follow the player
    //       Also need a walk cycle animation once we have a design for the protag (I don't think that has to be in code) (sprint and idle animation optional)

    public float speed = 5.0f;
    public float sprint = 2.0f;
    public float playerSpeed;
    public Rigidbody2D player;
    public Animator playerAnimator;
    public float wait = 5.0f; // if you would like a bigger delay, edit this float number.
    float pressTime = 0.0f; 
    float sprintTime = 3.0f;
    float sprintTimer;

    Vector2 velocity;

    //void Start()
    ///{

    void Update() 
    {
       
        if (Input.GetKey(KeyCode.LeftShift))
        {
             if(Time.time >= pressTime)
        { 
            pressTime = Time.time + wait;
            playerSpeed = speed + sprint;
            //sprintTimer += 1/(1/Time.deltaTime);

        }
        } else {
         playerSpeed = speed;
        }
       // if (Input.GetKeyDown (KeyCode.LeftShift) && sprintTimer <= 2f) {
		//	playerSpeed = speed + sprint;
		//}
		
		//if (Input.GetKeyUp (KeyCode.LeftShift) || sprintTimer > 2f) {
		//	playerSpeed = speed;
		//}
	
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

       //if (velocity != Vector2.zero) {
        
       // playerAnimator.SetFloat("Horizontal", velocity.x);
       // playerAnimator.SetFloat("Vertical", velocity.y);

    }


    //playerAnimator.SetFloat("Speed", velocity.sqrMagnitude);
    
    //}

    void FixedUpdate()
    {
        player.MovePosition(player.position + velocity * playerSpeed * Time.fixedDeltaTime);
    }
}

