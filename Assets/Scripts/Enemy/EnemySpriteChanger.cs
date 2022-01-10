using System;
using UnityEngine;

public class EnemySpriteChanger : SpriteChanger
{
    [Header("Sprites Attacking")]
    [SerializeField] protected Sprite _rightAttack;
    [SerializeField] protected Sprite _leftAttack;
    [SerializeField] protected Sprite _upAttack;
    [SerializeField] protected Sprite _downAttack;
    [Header("Sprites Dirty")]
    [SerializeField] protected Sprite _rightDirty;
    [SerializeField] protected Sprite _leftDirty;
    [SerializeField] protected Sprite _upDirty;
    [SerializeField] protected Sprite _downDirty;

    private Enemy _enemy;
    private EnemyMover _enemyMover;
    private Vector2 _direction;

    private int _activeSide;

    protected override void TryChangeSpriteOnMove()
    {
        Vector3 position = transform.position;

        if(_direction != null)
        {
            if (GetValue(position.x) > GetValue(_direction.x))
            {
                SetSprite(_left);
                _activeSide = (int)Side.Left;
            }
            else if (GetValue(position.x) < GetValue(_direction.x))
            {
                SetSprite(_rightDefault);
                _activeSide = (int)Side.Rigth;
            }
            else if (GetValue(position.y) > GetValue(_direction.y))
            {
                SetSprite(_down);
                _activeSide = (int)Side.Down;
            }
            else if (GetValue(position.y) < GetValue(_direction.y))
            {
                SetSprite(_up);
                _activeSide = (int)Side.Up;
            }
        }
    }

    protected override void OnBombExploded()
    {
        SetTargetSprites(_leftDirty, _rightDirty, _upDirty, _downDirty);
    }

    private void OnAttacked()
    {
        SetTargetSprites(_leftAttack, _rightAttack, _upAttack, _downAttack);
    }

    private void SetTargetSprites(Sprite left, Sprite rigth, Sprite up, Sprite down)
    {
        _enemyMover.StopMove();

        if (_activeSide == (int)Side.Rigth)
            SetSprite(rigth);
        else if (_activeSide == (int)Side.Left)
            SetSprite(left);
        else if (_activeSide == (int)Side.Up)
            SetSprite(up);
        else if (_activeSide == (int)Side.Down)
            SetSprite(down);
    }

    private int GetValue(float value)
    {
        return (int)Mathf.Round(value);
    }

    private void OnEnable()
    {
        if (_rightAttack == null || _leftAttack == null || _upAttack == null || _downAttack == null
           || _rightDirty == null || _leftDirty == null || _upDirty == null || _downDirty == null)
            throw new NullReferenceException(nameof(EnemySpriteChanger));

        _enemy = GetComponentInParent<Enemy>();
        _enemyMover = GetComponentInParent<EnemyMover>();

        _enemy.Attacked += OnAttacked;
        _enemy.Exploded += OnBombExploded;
        _enemyMover.NextPositionSetted += SetNextPosition;
    }

    private void OnDisable()
    {
        _enemy.Attacked -= OnAttacked;
        _enemy.Exploded -= OnBombExploded;
        _enemyMover.NextPositionSetted -= SetNextPosition;
    }

    private void SetNextPosition(Vector2 direction)
    {
        _direction = direction;
        TryChangeSpriteOnMove();
    }
}

public enum Side
{
    Left,
    Rigth,
    Up,
    Down
}
