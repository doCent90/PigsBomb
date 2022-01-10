using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BombAnimator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private Bomb _bomb;
    private Animator _animatior;

    private const string Explosion = "Explosion";

    private void OnEnable()
    {
        if (_particleSystem == null)
            throw new NullReferenceException(nameof(BombAnimator));

        _bomb = GetComponentInParent<Bomb>();
        _animatior = GetComponent<Animator>();

        _bomb.Activated += PlayExlosion;
    }

    private void OnDisable()
    {
        _bomb.Activated -= PlayExlosion;
    }

    private void PlayExlosion()
    {
        _animatior.SetTrigger(Explosion);
    }

    private void OnAnimationEvent()
    {
        _particleSystem.Play();
        _bomb.Explosion();
    }
}
