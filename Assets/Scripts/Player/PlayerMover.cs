using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;

    private Rigidbody2D _rigidbody2D;

    private readonly float _speed = 2f;

    public event Action<FloatingJoystick> Moved;

    private void OnEnable()
    {
        if (_joystick == null)
            throw new NullReferenceException(nameof(PlayerMover));

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        _rigidbody2D.velocity = direction * _speed;

        Moved?.Invoke(_joystick);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
