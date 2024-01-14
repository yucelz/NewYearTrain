using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
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

    [SerializeField] InputReader inputReader;

    private Rigidbody2D rb;
    private float gravityScale;
    // for player floating when in air
    [SerializeField] float glideDrag;
    public bool glideActive;
    private float glideInput;
    [SerializeField] float groundPoundSpeed;
    private bool groundPounding;

    [SerializeField] Vector2 boxSize;
    [SerializeField] float boxDistance;
    [SerializeField] LayerMask layerMask;
    private float jumpBuffer = 0.2f;
    private float lastJumpPressed;

    public bool isOnThePlatform;

    public Rigidbody2D platformRb;

    private bool timerActive;
    [SerializeField] float maxTime;
    private float currentTime;
    [SerializeField] Slider timer;

    public bool dashActive;
    public bool poundActive;

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;

        inputReader.MoveEvent += HandleMove;
        inputReader.GlideEvent += HandleGlide;
        inputReader.JumpEvent += Jump;

        currentTime = maxTime;
    }

    // Update is called once per frame
    private void Update() {

    }

    private void FixedUpdate() {
        // get player input and move if not dashing
        if (!dashing && !groundPounding) {

            if (horizontalInput != 0)
                { TurnCheck(); }

            Move();

            // player pressed jump just before landing so jump
            if (OnGround() && lastJumpPressed + jumpBuffer > Time.time && rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }

            if (!OnGround() && glideInput == 1) {
                rb.drag = glideDrag;
            }
            else {
                rb.drag = 0;
            }
        }
        else if (Time.time - startDashTime > dashDuration && !groundPounding) {
            dashing = false;
            rb.gravityScale = gravityScale;
            CancelInvoke();
        }
        else if (groundPounding && OnGround()) {
            StartCoroutine(DelayMovement());
            rb.gravityScale = gravityScale;
        }

        if (timerActive) {
            currentTime = Mathf.Clamp(currentTime -= Time.deltaTime, 0, maxTime);
            timer.value = currentTime / maxTime;
            if (currentTime <= 0) {
                timerActive = false;
                if (dashActive) {
                    DisableDash();
                }
                else if (glideActive) {
                    DisableGlide();
                }
                else if (poundActive) {
                    DisableGroundPound();
                }
            }
        }
        else {
            currentTime = maxTime;
            timer.value = 0;
        }
    }

    private void HandleMove(float input) {
        horizontalInput = input;
    }

    private void HandleGlide(float input) {
        if (glideActive) {
            glideInput = input;
        }
    }

    private void Move() {
        if (!isOnThePlatform) {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2((horizontalInput * speed) + platformRb.velocity.x, rb.velocity.y);
        }
    }

    private bool OnGround() {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, boxDistance, layerMask);
    }

    // visualize boxcast
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - (transform.up) * boxDistance, boxSize);
    }

    public void Jump() {
        if (!dashing && OnGround() && !groundPounding) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    public void Dash() {
        // only allow dash input if not already dashing
        if (!dashing && !groundPounding) {
            dashing = true;
            startDashTime = Time.time;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(Mathf.Sign(transform.rotation.y) * dashSpeed, 0);
            InvokeRepeating("CreateEcho", 0, echoSpawnDuration);
        }
    }

    public void GroundPound() {
        if (!dashing) {
            groundPounding = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.down * groundPoundSpeed;
        }
    }

    // wait so that player cant move right after landing from ground pound
    private IEnumerator DelayMovement() {
        yield return new WaitForSeconds(0.25f);
        groundPounding = false;
    }

    public void ActivateDash() {
        inputReader.DashEvent += Dash;
        dashActive = true;
        timerActive = true;
    }

    public void ActivateGroundPound() {
        inputReader.PoundEvent += GroundPound;
        poundActive = true;
        timerActive = true;
    }

    public void ActivateGlide() {
        glideActive = true;
        timerActive = true;
    }

    public void DisableDash() {
        inputReader.DashEvent -= Dash;
        dashActive = false;
    }

    public void DisableGroundPound() {
        inputReader.PoundEvent -= GroundPound;
        poundActive = false;
    }

    public void DisableGlide() {
        glideActive = false;
    }

    // creates ghost/echo of player on dash
    private void CreateEcho() {
        GameObject echoCopy = Instantiate(echo, transform.position, Quaternion.identity);
        Destroy(echoCopy, 1f);
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