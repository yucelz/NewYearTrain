using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] Sprite passive, active;

    private void Awake()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerHealth.UpdateCheckPoint(transform.position);
            _spriteRenderer.sprite=active;
        }
    }
}
