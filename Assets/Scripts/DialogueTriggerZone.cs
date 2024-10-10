using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private Interactable parentNPC;

    private void Start()
    {
        parentNPC = GetComponentInParent<Interactable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            parentNPC.playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
        if (other.CompareTag("Player"))
        {
            parentNPC.playerIsClose = false;
            parentNPC.closeDialogue();
        }
    }
}
