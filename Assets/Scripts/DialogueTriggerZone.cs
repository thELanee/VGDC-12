using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private NPC parentNPC;

    private void Start()
    {
        parentNPC = GetComponentInParent<NPC>();

        if (parentNPC == null)
        {
            Debug.LogError("Parent NPC script not found!");
        }
        else
        {
            Debug.Log("Parent NPC script found successfully.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger zone: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone");
            parentNPC.playerIsClose = true;
        }
        else
        {
            Debug.Log("Other object entered trigger zone: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exit event fired!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone");
            parentNPC.playerIsClose = false;
            parentNPC.zeroText();
        }
    }
}
