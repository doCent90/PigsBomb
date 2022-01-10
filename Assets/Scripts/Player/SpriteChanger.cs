using System;
using UnityEngine;

public abstract class SpriteChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("Sprites")]
    [SerializeField] protected Sprite _rightDefault;
    [SerializeField] protected Sprite _left;
    [SerializeField] protected Sprite _up;
    [SerializeField] protected Sprite _down;

    protected abstract void TryChangeSpriteOnMove();

    protected abstract void OnBombExploded();

    private void OnEnable()
    {
        if (_rightDefault == null || _left == null || _up == null || _down == null || _spriteRenderer == null)
            throw new NullReferenceException(nameof(SpriteChanger));

        SetSprite(_rightDefault);
    }

    protected void SetSprite(Sprite target)
    {
        _spriteRenderer.sprite = target;
    }
}
