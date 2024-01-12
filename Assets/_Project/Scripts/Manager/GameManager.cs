using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject vendingMachine;
    [SerializeField] InputReader inputReader;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        inputReader.OpenVendingEvent += DisplayVendingMachine;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideVendingMachine() {
        vendingMachine.SetActive(false);
    }

    private void DisplayVendingMachine() {
        vendingMachine.SetActive(true);
    }
}
