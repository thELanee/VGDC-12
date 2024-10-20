using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AStarNode : MonoBehaviour
{
    public AStarNode cameFrom;
    public List<AStarNode> connections;
    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
            }
        }
    }
}