using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableBase : MonoBehaviour
{

    protected GameObject _character;
    protected bool _isInteractable;
    [SerializeField] protected GameObject _interactableIndicatorIcon;

    private void Awake()
    {
        _character = GameObject.FindGameObjectWithTag("Player").gameObject;
        _interactableIndicatorIcon.SetActive(false);

    }
    private void Update()
    {
        // !!!! should be fixed based on Input System
        if (_isInteractable && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject == _character)
        {
            _isInteractable = true;
            _interactableIndicatorIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        if (collison.gameObject == _character)
        {
            _isInteractable = false;
            _interactableIndicatorIcon.SetActive(false);
        }
    }
    public abstract void Interact();
}
