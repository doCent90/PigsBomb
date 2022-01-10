using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerSpriteChanger : SpriteChanger
{
    [SerializeField] private Sprite _damagedSprite;

    private Player _player;
    private PlayerMover _playerMover;
    private FloatingJoystick _joystick;

    protected override void TryChangeSpriteOnMove()
    {
        if ((_joystick.Horizontal + _joystick.Horizontal) > (_joystick.Vertical + _joystick.Vertical))
        {
            if (_joystick.Horizontal < 0)
            {
                SetSprite(_down);
                return;
            }
            else if (_joystick.Horizontal > 0)
            {
                SetSprite(_rightDefault);
                return;
            }
        }
        else
        {
            if (_joystick.Vertical < 0)
            {
                SetSprite(_left);
                return;
            }
            else if (_joystick.Vertical > 0)
            {
                SetSprite(_up);
                return;
            }
        }
    }

    protected override void OnBombExploded()
    {
        SetSprite(_damagedSprite);
    }

    private void OnEnable()
    {
        _player = GetComponent<Player>();
        _playerMover = GetComponent<PlayerMover>();

        _playerMover.Moved += OnMoved;
        _player.Damaged += OnBombExploded;
    }

    private void OnDisable()
    {
        _playerMover.Moved -= OnMoved;
        _player.Damaged -= OnBombExploded;
    }

    private void OnMoved(FloatingJoystick joystick)
    {
        _joystick = joystick;
        TryChangeSpriteOnMove();
    }
}
