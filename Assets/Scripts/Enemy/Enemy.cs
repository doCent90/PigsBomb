using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action Exploded;
    public event Action Attacked;

    public void OnExloded()
    {
        Exploded?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            Attacked?.Invoke();
    }
}
