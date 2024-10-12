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
            DialogueManager.Instance.eraseNodes();
        }
    }
}
