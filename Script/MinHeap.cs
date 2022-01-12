using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap : MonoBehaviour
{
    private List<int> A = new List<int>();

    public void push(int value)
    {
        A.Add(value);
        int i = A.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (A[parent] > A[i])
            {
                Swap(parent, i);
                i = parent;
            }
            else
            {
                break;
            }
        }
    }

    public bool isEmpty()
    {
        return A.Count <= 0 ? true : false;
    }

    public int top()
    {
        return A[0];
    }

    public int pop()
    {
        if (A.Count == 0)
            throw new System.Exception();

        int root = A[0];

        A[0] = A[A.Count - 1];
        A.RemoveAt(A.Count - 1);

        int i = 0;
        int last = A.Count - 1;
        while (i < last)
        {
            int child = i * 2 + 1;
            if (child < last && A[child] > A[child + 1])
                child = child + 1;
            if (child > last || A[i] <= A[child])
                break;

            Swap(i, child);
            i = child;
        }

        return root;
    }

    public void Swap(int i, int j)
    {
        int tmp = A[i];
        A[i] = A[j];
        A[j] = tmp;
    }

}
