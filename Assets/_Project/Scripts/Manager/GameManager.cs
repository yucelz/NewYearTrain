using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject vendingMachine;
    [SerializeField] InputReader inputReader;
    private GameObject player;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        inputReader.OpenVendingEvent += DisplayVendingMachine;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideVendingMachine() {
        vendingMachine.SetActive(false);
        inputReader.EnablePlayer();
    }

    private void DisplayVendingMachine() {
        if (!(playerMovement.dashActive || playerMovement.glideActive || playerMovement.poundActive)) {
            vendingMachine.SetActive(true);
            inputReader.DisablePlayer();
        }
    }
}
