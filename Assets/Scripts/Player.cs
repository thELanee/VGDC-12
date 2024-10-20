using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    sprint,
    interact,
    idle
}
public class Player_movement : MonoBehaviour
{

    // Just movement for now

    // TODO: Let player sprint (Done I think...), lock player movement while in dialogue (TO DO), make camera follow the player (Done)
    //       Also need a walk cycle animation once we have a design for the protag (I don't think that has to be in code) (sprint and idle animation optional)

    public float speed = 5f; // Normal speed.
    public float sprint = 10f; // add on to speed when pressing the left shift button.
    public float playerSpeed; // playerSpeed, calcuated using sprint and/or speed.
    public Rigidbody2D player;
    public Animator playerAnimator; //controls animation speed, might not be needed, as sprite may be too quick to notice.
    public PlayerState currentState;
    public VectorValue startingPosition;

    Vector2 velocity;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        playerAnimator.SetFloat("moveX", 0);
        playerAnimator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    void Update()
    {
        // Calculate player speed based on sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = speed + sprint;
        }
        else
        {
            playerSpeed = speed;
        }

        // Get input axes
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        // Normalize the velocity vector to prevent faster diagonal movement
        if (velocity.sqrMagnitude > 1)
        {
            velocity.Normalize();
        }

        // Set animation parameters based on movement
        if (velocity != Vector2.zero)
        {
            playerAnimator.SetFloat("moveX", velocity.x);
            playerAnimator.SetFloat("moveY", velocity.y);
        }
    }


    //playerAnimator.SetFloat("Speed", velocity.sqrMagnitude);

    //}

    void FixedUpdate()
    {
        player.MovePosition(player.position + velocity * playerSpeed * Time.fixedDeltaTime);
    }

    public void SetMovement(float movement)
    {
        speed = movement;
    }
}
