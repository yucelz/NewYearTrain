using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    private bool dashing;
    private float startDashTime;
    private float horizontalInput;
    private bool facingRight = true;

    PlayerInputActions playerInputActions;

    private Rigidbody2D rb;
    private float gravityScale;

    public GameObject echo;
    [SerializeField] float echoSpawnDuration;

    // Start is called before the first frame update
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Dash.performed += Dash;
    }

    // Update is called once per frame
    private void Update() {
        // adjust player orientation according to input
        if (!facingRight && horizontalInput > 0 || facingRight && horizontalInput < 0) {
            transform.localScale = -transform.localScale;
            facingRight = !facingRight;
        }
    }

    private void FixedUpdate() {

        // get player input and move if not dashing
        if (!dashing) {
            horizontalInput = playerInputActions.Player.Movement.ReadValue<float>();
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
            CancelInvoke();
        }
        else if (Time.time - startDashTime > dashDuration) {
            dashing = false;
            rb.gravityScale = gravityScale;
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (!dashing) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        // only allow dash input if not already dashing
        if (!dashing) {
            dashing = true;
            startDashTime = Time.time;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
            InvokeRepeating("CreateEcho", 0, echoSpawnDuration);
        }
    }

    // creates ghost/echo of player on dash
    private void CreateEcho() {
        GameObject echoCopy = Instantiate(echo, transform.position, Quaternion.identity);
        Destroy(echoCopy, 1f);
    }
}
