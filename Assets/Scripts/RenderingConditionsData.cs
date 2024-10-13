using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CustomVector2 // Moved outside
{
    public float x;
    public float y;

    public UnityEngine.Vector2 ToUnityVector2()
    {
        return new UnityEngine.Vector2(x, y);
    }
}

[System.Serializable]
public class RenderingConditionData
{
    public List<RenderingConditionEntry> entries;

    [System.Serializable]
    public class RenderingConditionEntry
    {
        public string sceneName;                // Make this public for serialization
        public string characterName;             // Make this public for serialization
        public RenderingInstructions instructions; // Make this public for serialization
    }
}

[System.Serializable]
public class RenderingInstructions
{
    public List<Condition> conditions = new List<Condition>(); // Made public for serialization
    public string nodeId;               // Made public for serialization
    public CustomVector2 renderingCoords; // Now refers to the standalone struct
    public Direction facingDirection;    // Add this line to specify the direction to face

    public override string ToString()
    {
        // Create a string representation of the conditions
        string conditionsString = string.Join(", ", conditions.Select(c => c.ToString()));

        // Format the output string
        return $"NodeId: {nodeId}, RenderingCoords: ({renderingCoords.ToUnityVector2().x}, {renderingCoords.ToUnityVector2().y}), Conditions: [{conditionsString}], FacingDirection: {facingDirection}";
    }
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}