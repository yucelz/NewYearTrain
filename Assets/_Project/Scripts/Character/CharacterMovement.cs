using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    private bool dashing;
    private float startDashTime;
    private float horizontalInput;
    private bool _facingRight = true;
    public GameObject echo;
    [SerializeField] float echoSpawnDuration;

    private CustomInput characterInputActions;

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
        characterInputActions = new CustomInput();
        characterInputActions.Player.Enable();
        characterInputActions.Player.Jump.performed += Jump;
        characterInputActions.Player.Dash.performed += Dash;
    }

    // Update is called once per frame
    private void Update() {
        // adjust player orientation according to input

        /*if (!_facingRight && horizontalInput.x > 0 || _facingRight && horizontalInput.x < 0)
        {
            transform.localScale = -transform.localScale;
            _facingRight = !_facingRight;
        }
        */
    }

    private void FixedUpdate() {

        // get player input and move if not dashing
        if (!dashing) {
            horizontalInput = characterInputActions.Player.Movement.ReadValue<float>();
            //Debug.Log("horizontalInput:" + horizontalInput.x);
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

            // player pressed jump just before landing so jump
            if (OnGround() && lastJumpPressed + jumpBuffer > Time.time && rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }

            if (horizontalInput != 0)
                { TurnCheck(); }
        }
        else if (Time.time - startDashTime > dashDuration) {
            dashing = false;
            rb.gravityScale = gravityScale;
            CancelInvoke();
        }
    }

    private bool OnGround() {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, boxDistance, layerMask);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - (transform.up) * boxDistance, boxSize);
    }

    public void Jump(InputAction.CallbackContext context) {
        if (!dashing && OnGround()) {
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
            rb.velocity = new Vector2(Mathf.Sign(transform.rotation.y) * dashSpeed, 0);
            //InvokeRepeating("CreateEcho", 0, echoSpawnDuration);
        }
    }

    // creates ghost/echo of player on dash
    private void CreateEcho() {
        GameObject echoCopy = Instantiate(echo, transform.position, Quaternion.identity);
        Destroy(echoCopy, 1f);
    }

    private void OnDisable() {
        characterInputActions.Disable();
    }

    #region Turn Check

    private void TurnCheck() {
        if (horizontalInput > 0 && !_facingRight) {
            Turn();
        }
        else if (horizontalInput < 0 && _facingRight) {
            Turn();
        }
    }
    private void Turn() {
        if (_facingRight) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.x);
            transform.rotation = Quaternion.Euler(rotator);
            _facingRight = !_facingRight;
        }
        else {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.x);
            transform.rotation = Quaternion.Euler(rotator);
            _facingRight = !_facingRight;
        }
    }
    #endregion
}