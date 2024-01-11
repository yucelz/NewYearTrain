using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private Transform posA, posB;
    [SerializeField] private float _speed = 3f;
    private Vector3 _targetPos;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _platformRb;
    private Vector3 _moveDirection;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _platformRb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _targetPos = posB.position;
        DirectinoCalculation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            _targetPos = posB.position;
            DirectinoCalculation();
        }
        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            _targetPos = posA.position;
            DirectinoCalculation();
        }
        
    }

    private void FixedUpdate()
    {
        _platformRb.velocity = _moveDirection * _speed;
    }

    private void DirectinoCalculation()
    {
        _moveDirection = (_targetPos - transform.position).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerMovement.isOnThePlatform = true;
            _playerMovement.platformRb = _platformRb;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerMovement.isOnThePlatform = true;
        }
    }
}
