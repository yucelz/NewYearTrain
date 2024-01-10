using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : InteractableBase, ITalkable
{
    [SerializeField] private DialogueText _dialogueText;
    [SerializeField] private DialogueController _dialogueController;

    public override void Interact()
    {
        Talk(_dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        _dialogueController.DisplayNextParagraph(dialogueText);
    }
}
