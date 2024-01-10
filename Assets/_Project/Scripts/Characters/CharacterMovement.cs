using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    private bool dashing;
    private float startDashTime;
    private Vector2 horizontalInput;
    private bool _facingRight = true;
    public GameObject echo;
    [SerializeField] float echoSpawnDuration;

    private CustomInput characterInputActions;

    private Rigidbody2D rb;
    private float gravityScale;

    [SerializeField] Vector2 boxSize;
    [SerializeField] float boxDistance;
    [SerializeField] LayerMask layerMask;

    public bool isOnThePlatform;

    public Rigidbody2D platformRb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterInputActions = new CustomInput();
        characterInputActions.Player.Enable();
        characterInputActions.Player.Jump.performed += Jump;
        characterInputActions.Player.Dash.performed += Dash;
    }

    // Update is called once per frame
    private void Update()
    {
        // adjust player orientation according to input

        /*if (!_facingRight && horizontalInput.x > 0 || _facingRight && horizontalInput.x < 0)
        {
            transform.localScale = -transform.localScale;
            _facingRight = !_facingRight;
        }
        */
    }

    private void FixedUpdate()
    {

        // get player input and move if not dashing
        if (!dashing)
        {
            Move(isOnThePlatform);
            if (horizontalInput.x != 0)
            { TurnCheck(); }

            CancelInvoke();
        }
        else if (Time.time - startDashTime > dashDuration)
        {
            dashing = false;
            rb.gravityScale = gravityScale;
        }
    }

    private void Move(bool isOnThePlatform)
    {
        horizontalInput = characterInputActions.Player.Movement.ReadValue<Vector2>();
        if (!isOnThePlatform)
        {
            rb.velocity = new Vector2(horizontalInput.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2((horizontalInput.x * speed) + platformRb.velocity.x, rb.velocity.y);
        }

    }

    private bool OnGround()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, boxDistance, layerMask))
        {
            Debug.Log("ground");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - (transform.up) * boxDistance, boxSize);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        if (!dashing && OnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        // only allow dash input if not already dashing
        if (!dashing)
        {
            dashing = true;
            startDashTime = Time.time;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
            // InvokeRepeating("CreateEcho", 0, echoSpawnDuration);
        }
    }

    // creates ghost/echo of player on dash
    private void CreateEcho()
    {
        GameObject echoCopy = Instantiate(echo, transform.position, Quaternion.identity);
        Destroy(echoCopy, 1f);
    }
    private void OnDisable()
    {
        characterInputActions.Disable();
    }

    #region Turn Check

    private void TurnCheck()
    {
        if (horizontalInput.x > 0 && !_facingRight)
        {
            Turn();
        }
        else if (horizontalInput.x < 0 && _facingRight)
        {
            Turn();
        }
    }
    private void Turn()
    {
        if (_facingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.x);
            transform.rotation = Quaternion.Euler(rotator);
            _facingRight = !_facingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.x);
            transform.rotation = Quaternion.Euler(rotator);
            _facingRight = !_facingRight;
        }
    }
    #endregion
}
