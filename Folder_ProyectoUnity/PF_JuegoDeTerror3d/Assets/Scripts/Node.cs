using UnityEngine;

public class Node<T>
{
    private T value;
    private Node<T> next;
    private Node<T> prev;
    public Node(T _value)
    {
        value = _value;
        next = null;
        prev = null;
    }
    public void SetNext(Node<T> _next)
    {
        next = _next;
    }
    public void SetPrev(Node<T> _prev)
    {
        prev = _prev;
    }
    public T Value => value;
    public Node<T> Next => next;
    public Node<T> Prev => prev;
}
