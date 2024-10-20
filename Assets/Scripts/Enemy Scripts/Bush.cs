using System.Collections.Generic;
using UnityEngine;

public class Bush : Enemy
{
    public Transform target;
    public float chaseRadius;
    public Transform homePosition;

    private List<PNode> path;
    private int currentPathIndex = 0;
    private Rigidbody2D rb;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        path = new List<PNode>(); // Initialize path
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D variable
    }

    public void CreatePath()
    {
        Vector3Int startPos = GameManager.Instance.TileProvider.groundTilemap.WorldToCell(transform.position); // Convert bush's position to grid coordinates
        Vector3Int targetPos = GameManager.Instance.TileProvider.groundTilemap.WorldToCell(target.position); // Convert player’s position to grid coordinates

        PathfindingResult result = GameManager.Instance.Pathfinder.Run(startPos.x, startPos.y, targetPos.x, targetPos.y, GameManager.Instance.TileProvider, out path);
        if (result != PathfindingResult.SUCCESSFUL)
        {

        }
        else
        {
            currentPathIndex = 0; // Reset the current path index after finding the path
        }
    }

    public void FollowPath()
    {
        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector3Int gridPosition = new Vector3Int(path[currentPathIndex].X, path[currentPathIndex].Y, 0); // Convert PNode to grid position (Z is 0 for 2D)
            Vector3 targetPosition = GameManager.Instance.TileProvider.groundTilemap.CellToWorld(gridPosition); // Convert grid position to world position

            // Calculate the new position using MovePosition
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition); // Use Rigidbody2D to move

            // If the bush is close to the current path node, move to the next node
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++; // Move to the next node in the path
            }
        }
    }

    void Update()
    {
        // Check if the target is within the chase radius
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
        {
            CreatePath();
            if (path != null && path.Count > 1)
            {
                currentPathIndex++;
            }
            FollowPath();
        }
        else
        {
            // Optional: Return to home position or stop moving
            // You could implement logic here to return to homePosition if needed
            //currentPathIndex = 0;
        }
    }
}
