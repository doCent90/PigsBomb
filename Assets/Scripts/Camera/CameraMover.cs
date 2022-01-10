using System;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Player _player;

    private readonly float _speed = 5f;
    private readonly float _edgeX = 4f;
    private readonly float _edgeY = 1.7f;
    private bool _canSetX = true;
    private bool _canSetY = true;

    private void OnEnable()
    {
        if (_player == null)
            throw new NullReferenceException();
    }

    private void Follow()
    {
        _canSetX = true;
        _canSetY = true;

        if (_player.transform.position.x < -_edgeX || _player.transform.position.x > _edgeX)
            _canSetX = false;

        if (_player.transform.position.y < -_edgeY || _player.transform.position.y > _edgeY)
            _canSetY = false;

        if (_canSetX)
        {
            Vector3 target = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        }

        if (_canSetY)
        {
            Vector3 target = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        }
    }

    private void Update()
    {
        Follow();
    }
}
