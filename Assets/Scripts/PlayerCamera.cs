using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

public Transform player;
 
 void Update ()  {
     transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
 }

}
