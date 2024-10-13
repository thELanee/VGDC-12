using UnityEngine;

[CreateAssetMenu(fileName = "PlayerResource", menuName = "ScriptableObjects/PlayerResource", order = 1)]
public class PlayerResource : ScriptableObject
{
    public float max = 100;
    public float current;
}