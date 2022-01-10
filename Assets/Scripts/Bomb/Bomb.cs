using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bomb : MonoBehaviour
{
    private CircleCollider2D _circleCollider2D;

    private bool _hasActivated = false;
    private float _spentTime = 0;

    private const float RadiusExplosion = 20f;
    private const float TimeToAction = 3f;
    private const float Delay = 2f;

    public event Action Activated;

    public void Explosion()
    {
        _circleCollider2D.enabled = true;
        Destroy(gameObject, Delay);
    }

    private void OnEnable()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            player.TakeDamage();

        if (collision.TryGetComponent(out Enemy enemy))
            enemy.OnExloded();
    }

    private void Update()
    {
        if(_hasActivated)
        {
            return;
        }
        else
        {
            _spentTime += Time.deltaTime;

            if (_spentTime >= TimeToAction)
            {
                _spentTime = 0;
                _hasActivated = true;

                Activated?.Invoke();
            }
        }
    }
}
