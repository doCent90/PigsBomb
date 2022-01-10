using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private bool _isDog = false;

    private NavMeshAgent _navMeshAgent;
    private bool _isReadyToMove = false;
    private float _spendTime;

    private const float TimeToChange = 4f;
    private const float RangeX = 7f;
    private const float RangeY = 3.5f;

    public event Action<Vector2> NextPositionSetted;

    public void StopMove()
    {
        _navMeshAgent.isStopped = true;
    }

    private void ContinueMove()
    {
        _navMeshAgent.isStopped = false;
    }

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _isReadyToMove = true;
        SetTargetPosition();
    }

    private void SetTargetPosition()
    {
        Vector2 currentTarget;

        if (_isDog == false)
        {
            currentTarget = GetRandomPoint();
        }
        else
        {
            Vector2[] targets = new Vector2[2];
            targets[0] = GetRandomPoint();
            targets[1] = _player.transform.position;

            currentTarget = targets[Random.Range(0, targets.Length)];
        }

        _navMeshAgent.SetDestination(currentTarget);

        NextPositionSetted?.Invoke(_navMeshAgent.velocity);
    }

    private Vector2 GetRandomPoint()
    {
        return new Vector2(Random.Range(RangeX, -RangeX), Random.Range(RangeY, -RangeY));
    }

    private void Update()
    {
        if (_isReadyToMove == false)
        {
            return;
        }
        else
        {
            _spendTime += Time.deltaTime;

            if (_spendTime > TimeToChange)
            {
                SetTargetPosition();
                ContinueMove();
                _spendTime = 0;                
            }
        }
    }
}
