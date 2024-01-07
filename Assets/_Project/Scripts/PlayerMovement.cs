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
    public GameObject echo;
    [SerializeField] float echoSpawnDuration;

    PlayerInputActions playerInputActions;

    private Rigidbody2D rb;
    private float gravityScale;

    [SerializeField] Vector2 boxSize;
    [SerializeField] float boxDistance;
    [SerializeField] LayerMask layerMask;
    private float jumpBuffer = 0.2f;
    private float lastJumpPressed;

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

            // player pressed jump just before landing so jump
            if (OnGround() && lastJumpPressed + jumpBuffer > Time.time && rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }
        // dash is over
        else if (Time.time - startDashTime > dashDuration) {
            dashing = false;
            rb.gravityScale = gravityScale;
            CancelInvoke();
        }
    }

    private bool OnGround() {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, boxDistance, layerMask)) {
            Debug.Log("ground");
            return true;
        }
        else {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - (transform.up) * boxDistance, boxSize);
    }

    public void Jump(InputAction.CallbackContext context) {
        if (!dashing && OnGround()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        lastJumpPressed = Time.time;
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
