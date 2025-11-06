using UnityEngine;

public class DoubleList<T>
{
    public Node<T> Head = null;
    public Node<T> Tail = null;
    public int Count = 0;
    public virtual void AddNode(T dato)
    {
        Node<T> newNode = new Node<T>(dato);

        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            Tail.SetNext(newNode);
            newNode.SetPrev(Tail);
            Tail = newNode;
        }

        Count++;
    }

    public Node<T> Find(T target, Node<T> start, int depth = 1000)
    {
        if (start == null || depth <= 0) return null;
        if (start.Value.Equals(target)) return start;
        return Find(target, start.Next, depth - 1);
    }

    public void ReadForward(Node<T> value, int depth = 1000)
    {
        if (value == null || depth <= 0) return;
        Debug.Log(value.Value.ToString());
        ReadForward(value.Next, depth - 1);
    }

    public void ReadBackward(Node<T> value, int depth = 1000)
    {
        if (value == null || depth <= 0) return;
        Debug.Log(value.Value.ToString());
        ReadBackward(value.Prev, depth - 1);
    }

    public void InsertAfter(T target, Node<T> value)
    {
        Node<T> temp = Find(target, Head);
        if (temp == null)
        {
            Debug.LogError("InsertAfter: No se encontró el nodo objetivo.");
            return;
        }

        value.SetNext(temp.Next);
        value.SetPrev(temp);

        if (temp.Next != null)
            temp.Next.SetPrev(value);
        else
            Tail = value;

        temp.SetNext(value);
        Count++;
    }

    public void Delete(T target)
    {
        Node<T> temp = Find(target, Head);
        if (temp == null) return;

        if (temp.Prev != null)
            temp.Prev.SetNext(temp.Next);
        else
            Head = temp.Next;

        if (temp.Next != null)
            temp.Next.SetPrev(temp.Prev);
        else
            Tail = temp.Prev;

        temp.SetNext(null);
        temp.SetPrev(null);
        Count--;
    }
}
