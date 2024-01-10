using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogueName;
    [SerializeField] private TextMeshProUGUI DialogueText;
    private Queue<string> _paragraphs = new Queue<string>();
    private bool _conversationEnded;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if (_paragraphs.Count == 0)
        {
            if (!_conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else
            {
                EndConversation();
            }
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        DialogueName.text = dialogueText.speakerName;
        foreach (var item in dialogueText.paragraphs)
        {
            Debug.Log("item:" + item.ToString());
            _paragraphs.Enqueue(item);
        }
    }

    private void EndConversation()
    {
        _conversationEnded = false;
        
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
