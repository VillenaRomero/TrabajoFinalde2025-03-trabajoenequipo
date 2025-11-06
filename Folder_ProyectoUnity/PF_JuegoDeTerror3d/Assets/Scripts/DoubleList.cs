using UnityEngine;

public class DoubleList<T>
{
    public Node<T> Head = null;
    public Node<T> Tail = null;
    public int Count;

    public virtual void AddNode(T dato)
    {
        Node<T> newNode = new Node<T>(dato);
        #region Caso de primer elemento
        if (Head == null && Tail == null)
        {
            Head = newNode;
            Tail = newNode;
            Count++;
            return;
        }
        #endregion
        #region Case de dos a mas elementos
        if (Head != null)
        {
            newNode.SetNext(Head);
            Head.SetPrev(newNode);

            Head = newNode;
            Count++;
        }
        #endregion
    }
    public Node<T> Find(T target, Node<T> start, int depth = 1000)
    {
        if (start == null || depth <= 0) return null;

        if (start.Value.Equals(target))
        {
            return start;
        }


        return Find(target, start.Next, depth - 1);
    }
    public void ReadForward(Node<T> value, int depth = 1000)
    {
        if (value == null || depth <= 0) return;

        Debug.Log(value.Value.ToString());

        if (value.Next != null)
            ReadForward(value.Next, depth - 1);
    }
    public void ReadBackward(Node<T> value, int depth = 1000)
    {
        if (value == null || depth <= 0) return;

        Debug.Log(value.Value.ToString());

        if (value.Prev != null)
            ReadBackward(value.Prev, depth - 1);
    }

    public void InsertAfter(T target, Node<T> value)
    {
        Node<T> temp = Find(target, Head);
        if (temp == null)
        {
            Debug.LogError("NULLO");
            return;
        }
        if (temp.Next != null)
        {
            value.SetNext(temp.Next);
            value.SetPrev(temp);

            temp.Next.SetPrev(value);
            temp.SetNext(value);

        }
        else
        {
            value.SetPrev(temp);
            value.SetNext(null);

            temp.SetNext(value);

            Tail = value;
        }

    }
    public void Delete(T target)
    {
        if (Head.Value.Equals(target))
        {
            Head = Head.Next;
            Head.Prev.SetNext(null);
            Head.SetPrev(null);

            Count--;
            return;
        }
        if (Tail.Value.Equals(target))
        {
            Tail = Tail.Prev;
            Tail.Next.SetPrev(null);
            Tail.SetNext(null);

            Count--;
            return;

        }
        Node<T> temp = Find(target, Head);
        if (temp == null) return;

        temp.Prev.SetNext(temp.Next);
        temp.Next.SetPrev(temp.Prev);

        temp.SetNext(null);
        temp.SetPrev(null);

        Count--;
    }
}
