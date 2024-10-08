using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{

    // Just movement for now

    // TODO: Let player sprint, lock player movement while in dialogue, make camera follow the player
    //       Also need a walk cycle animation once we have a design for the protag (I don't think that has to be in code) (sprint and idle animation optional)

    public float speed = 5f;
    public Rigidbody2D player;
    Vector2 velocity;

    void Start()
    {
    }

    void Update() 
    {
        velocity = Vector2.zero;
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        player.MovePosition(player.position + velocity * speed * Time.fixedDeltaTime);
    }
}  
