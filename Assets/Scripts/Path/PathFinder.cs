using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layerMask;

    private List<Vector2> _pathToTarget = new List<Vector2>();
    private List<Node> _checkedNodes = new List<Node>();
    private List<Node> _waitingNodes = new List<Node>();

    private const float Radius = 0.1f;

    private void Update()
    {
        Debug.Log(_checkedNodes.Count);
        Debug.Log(_waitingNodes.Count);
        _pathToTarget = GetPath(_player.transform.position);
    }

    private List<Vector2> GetPath(Vector2 target)
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(_player.transform.position.x, _player.transform.position.y);

        if (startPosition == targetPosition)
            return _pathToTarget;

        Node startNode = new Node(0, startPosition, targetPosition, null);
        _checkedNodes.Add(startNode);

        List<Node> neighbourNodes = GetNeighbourNodes(startNode);
        _waitingNodes.AddRange(neighbourNodes);

        while(_waitingNodes.Count > 0)
        {
            Node nodeToCheck = _waitingNodes.Where(node => node.AmountPathNode == _waitingNodes.Min(minNode => minNode.AmountPathNode)).FirstOrDefault();

            if (nodeToCheck.Position == targetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            bool walkeble = !Physics2D.OverlapCircle(nodeToCheck.Position, Radius, _layerMask);

            if(walkeble == false)
            {
                _waitingNodes.Remove(nodeToCheck);
                _checkedNodes.Add(nodeToCheck);
            }
            else if (walkeble)
            {
                _waitingNodes.Remove(nodeToCheck);

                bool isSameNode = _checkedNodes.Where(node => node.Position == nodeToCheck.Position).Any();

                if (isSameNode == false)
                {
                    _checkedNodes.Add(nodeToCheck);
                    _waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }
                else
                {
                    var sameNode = _checkedNodes.Where(node => node.Position == nodeToCheck.Position).ToList();
                }
            }
        }

        return _pathToTarget;
    }

    private List<Vector2> CalculatePathFromNode(Node node)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = node;

        while(currentNode.PreviousNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    private List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Vector2 nodePosition = node.Position;
        int startToNextNodeLength = node.StartToNodeLength + 1;

        Node left = new Node(startToNextNodeLength, new Vector2(nodePosition.x - 1, nodePosition.y), node.TargetPosition, node);
        Node rigth = new Node(startToNextNodeLength, new Vector2(nodePosition.x + 1, nodePosition.y), node.TargetPosition, node);
        Node up = new Node(startToNextNodeLength, new Vector2(nodePosition.x, nodePosition.y + 1), node.TargetPosition, node);
        Node down = new Node(startToNextNodeLength, new Vector2(nodePosition.x, nodePosition.y - 1), node.TargetPosition, node);

        neighbours.Add(left);
        neighbours.Add(rigth);
        neighbours.Add(up);
        neighbours.Add(down);

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (_pathToTarget != null)
        {
            foreach (var item in _checkedNodes)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item.Position, Radius);
            }


            foreach (var item in _pathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(item, Radius);
            }
        }
    }
}

public class Node
{
    public Node PreviousNode { get; private set; }
    public int AmountPathNode { get; private set; } //F
    public int StartToNodeLength { get; private set; } //G
    public int NodeToTargetLength { get; private set; } //H
    public Vector2 TargetPosition { get; private set; }
    public Vector2 Position { get; private set; }

    public Node(int startToNodeLength, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        StartToNodeLength = startToNodeLength;

        NodeToTargetLength = (int)Mathf.Abs(targetPosition.x = Position.x) + (int)Mathf.Abs(targetPosition.y = Position.y);
        AmountPathNode = StartToNodeLength + NodeToTargetLength;
    }
}
