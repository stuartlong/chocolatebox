using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TreeMap<T>
{

    private TreeMapNode<T> root;
    private List<TreeMapNode<T>> allNodes;
    /// <summary>
    /// A Tree that maps a range of values to an object.
    /// </summary>
	public TreeMap()
	{
        root = null;
        allNodes = new List<TreeMapNode<T>>();
	}

    public void Add(float probability, T obj)
    {
        allNodes.Add(new TreeMapNode<T>(probability, obj));
    }

    public void Reindex()
    {
        // Sort all the nodes to decreasing order
        allNodes.Sort();
        allNodes.Reverse();

        //Create a tree with the highest priorities towards the top.
        Queue<TreeMapNode<T>> queue = new Queue<TreeMapNode<T>>();

        foreach (TreeMapNode<T> node in allNodes)
        {

        }

        //Set the indexes
    }

}


public class TreeMapNode<T> : IComparable
{
    private int leftHeight;
    private int rightHeight;
    
    private float minimum;
    private float maximum;
    private float probability;
    private T data;


    private TreeMapNode<T> leftChild;
    private TreeMapNode<T> rightChild;

    public TreeMapNode(float p, T obj)
    {
        this.probability = p;
        this.data = obj;
        this.leftChild = null;
        this.rightChild = null;
    }

    public T Get(float index)
    {
        if (index >= minimum && index < maximum)
        {
            return data;
        }

        if (index < minimum)
        {
            if (leftChild != null)
            {
                return leftChild.Get(index);
            }
            else
            {
                return default(T);
            }
        }

        if (index >= maximum)
        {
            if (rightChild != null)
            {
                return rightChild.Get(index);
            }
            else
            {
                return default(T);
            }
        }

        return default(T);
    }

    override public int CompareTo(TreeMapNode<T> node)
    {
        return this.probability.CompareTo(node.probability);
    }

}
