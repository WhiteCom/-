using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap : MonoBehaviour
{
    //TileSc 를 Heap으로 세팅해줄 것임.
    //현재 프로젝트를 오래 진행할 수 없기에
    //TileSc 에 대한 것으로만 타입을 정해서 Heap을 구성할 것임.
    //
    //추후 다른 인풋까지 확장하려면 Generic 타입으로 바꿔줄 수 있어야 한다.
    //C++의 경우 템플릿을 이용하든 하면 될 것임. (추정)

    private List<TileSc> heap = new List<TileSc>();

    public void Add(TileSc value)
    {
        //add at the end
        heap.Add(value);

        //bubble up
        int i = heap.Count; 
        while(i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[parent].Fcost > heap[i].Fcost)
            {
                Swap(parent, i);
                i = parent;
            }
            else
                break;
        }
    }

    //top의 Fcost
    public TileSc Top()
    {
        if(heap.Count == 0)
        {
            Debug.LogWarning("Heap is Empty");
            return null;
        }

        return heap[0];
    }

    //top 제거
    public TileSc Remove()
    {
        TileSc root = heap[0];
        if(heap.Count == 0)
        {
            Debug.LogWarning("Heap is Empty!");
            return null;
        }

        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        //bubble down
        int i = 0;
        int last = heap.Count - 1;
        while(i < last)
        {
            //get left child idx
            int child = i * 2 + 1;

            //use right child if it is smaller
            if (child < last &&
                heap[child].Fcost > heap[child = 1].Fcost)
                child = child + 1;

            //if parent is smaller or equal, stop
            if (child > last ||
                heap[i].Fcost <= heap[child].Fcost)
                break;

            Swap(i, child);
            i = child;
        }

        return root;
    }

    private void Swap(int i, int j)
    {
        TileSc tmp = heap[i];
        heap[i] = heap[j];
        heap[j] = tmp;
    }
}
