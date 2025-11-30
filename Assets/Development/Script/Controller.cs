using UnityEngine;

public abstract class Controller<T> : MonoBehaviour
{
    public T StateMachine { get; protected set; }
}