using UnityEngine;

public abstract class Rule : MonoBehaviour
{
    public abstract bool IsValide(Node[,] nodesGrid);
    public abstract void Apply(Node[,] nodesGrid);
}
