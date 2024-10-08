using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public bool playerIsClose;
    private bool isTyping;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && playerIsClose) {

            if(dialoguePanel.activeInHierarchy && !isTyping) {
                NextLine();
            } else {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        if(dialogueText.text == dialogue[index]) {
            
        }
    }

    public void zeroText() {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() {
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    public void NextLine() {
        if(index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        } 
        else {
            zeroText();
        }
    }

    private void OnTriggerEnter2d(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Close");
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2d(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
