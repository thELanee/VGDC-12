using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{


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
