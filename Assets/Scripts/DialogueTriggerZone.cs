using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private NPC parentNPC;

    private void Start()
    {
        parentNPC = GetComponentInParent<NPC>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentNPC.playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentNPC.playerIsClose = false;
            parentNPC.closeDialogue();
        }
    }
}
