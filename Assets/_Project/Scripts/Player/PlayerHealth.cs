using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    [SerializeField] private bool _isPlayerRespawned;

    private Vector2 _checkPointPosition;
    private float _respawnDuration = 0.5f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;
        _checkPointPosition = transform.position;
    }
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0 && !_isPlayerRespawned)
        {
            Die();
        }
        else if (CurrentHealth <= 0 && _isPlayerRespawned)
        {
            StartCoroutine(RespawnPlayer(_respawnDuration));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            StartCoroutine(RespawnPlayer(_respawnDuration));
        }
    }

    public void UpdateCheckPoint(Vector2 checkPointPostion)
    {
        _checkPointPosition = checkPointPostion;
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator RespawnPlayer(float duration)
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = _checkPointPosition;
        _spriteRenderer.enabled = true;
    }

}
