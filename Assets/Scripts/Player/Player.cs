using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerSpriteChanger))]
public class Player : MonoBehaviour
{
    [SerializeField] private Buttons _buttons;
    [SerializeField] private Bomb _bombTemplate;

    private PlayerMover _playerMover;
    private PlayerSpriteChanger _playerSpriteChanger;

    private float _spendTime;
    private bool _isDamaged = false;

    private const float Delay = 3f;

    public event Action Damaged;

    public void TakeDamage()
    {
        Damaged?.Invoke();

        _isDamaged = true;
        _playerSpriteChanger.enabled = false;
        _playerMover.enabled = false;
    }

    private void OnEnable()
    {
        if (_buttons == null || _bombTemplate == null)
            throw new NullReferenceException(nameof(Player));

        _playerMover = GetComponent<PlayerMover>();
        _playerSpriteChanger = GetComponent<PlayerSpriteChanger>();

        _buttons.FireClicked += SetBomb;
    }

    private void OnDisable()
    {
        _buttons.FireClicked -= SetBomb;
    }

    private void SetBomb()
    {
        if (_isDamaged == false)
        {
            Vector2 plantPosition = GetPosition();

            var bomb = Instantiate(_bombTemplate, plantPosition, Quaternion.identity);
        }
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Update()
    {
        if(_isDamaged == false)
        {
            return;
        }
        else
        {
            _spendTime += Time.deltaTime;

            if (_spendTime > Delay)
            {
                _isDamaged = false;
                _playerMover.enabled = true;
                _playerSpriteChanger.enabled = true;
                _spendTime = 0;
            }
        }
    }
}
