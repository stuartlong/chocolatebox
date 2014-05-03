﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RangeTree<T>
{

    private RangeTreeNode<T> root;
    private List<RangeTreeNode<T>> allNodes;
    public float minimumValue;
    public float maximumValue;

    /// <summary>
    /// A Tree that maps a range of values to an object.
    /// </summary>
	public RangeTree()
	{
        root = null;
        allNodes = new List<RangeTreeNode<T>>();
	}

    public void Add(float probability, T obj)
    {
        allNodes.Add(new RangeTreeNode<T>(probability, obj));
    }

    /// <summary>
    /// After inputing a number of objects <typeparamref name="T"/> 
    /// Set the number of the probabilities of each object so we can 
    /// associate a number in a range to an object
    /// 
    /// This should only be called once the tree is set. 
    /// </summary>

    public void Index()
    {
        // Sort all the nodes to decreasing order
        allNodes.Sort();
        allNodes.Reverse();

        //Set Root
        this.root = allNodes.First();
        root.parent = null;
        allNodes.RemoveAt(0);

        //Populate tree and reset all nodes values;
        while (allNodes.Count > 0)
        {
            RangeTreeNode<T> node = allNodes.First();
            allNodes.RemoveAt(0);
            root.Add(node);
        }

        //Set the indexes
        this.maximumValue = root.SetIndexes(0);
    }

    /// <summary>
    /// Generate a valid random index for the given RangeTree
    /// </summary>
    /// <returns></returns>
    public float RandomIndex()
    {
        return UnityEngine.Random.Range(this.minimumValue, this.maximumValue);
    }

    public T Get(float index)
    {
        return root.Get(index);
    }
}


public class RangeTreeNode<T> : IComparable
{
    private int leftHeight;
    private int rightHeight;
    
    private float minimum;
    private float maximum;
    private float probability;
    private T data;


    private RangeTreeNode<T> leftChild;
    private RangeTreeNode<T> rightChild;
    public RangeTreeNode<T> parent;


    public RangeTreeNode(float p, T obj)
    {
        this.probability = p;
        this.data = obj;
        this.leftChild = null;
        this.rightChild = null;
        this.parent = null;
    }

    public void Add(RangeTreeNode<T> node){
        if (this.leftChild == null)
        {
            this.leftChild = node;
            node.parent = this;
            this.leftHeight = 1;
            if (this.parent != null)
            {
                this.parent.IncrementHeight(this);
            }
        }
        else if(this.rightChild == null)
        {
            this.rightChild = node;
            node.parent = this;
            this.rightHeight = 1;
            if (this.parent != null)
            {
                this.parent.IncrementHeight(this);
            }
        }
        else if (this.leftHeight == this.rightHeight)
        {
            this.leftChild.Add(node);
        }
        else
        {
            this.rightChild.Add(node);
        }
    }

    public void IncrementHeight(RangeTreeNode<T> node)
    {
        if (node == this.leftChild)
        {
            this.leftHeight++;
        }
        else
        {
            this.rightHeight++;
        }

        if (parent != null)
        {
            this.parent.IncrementHeight(this);
        }
    }

    /// <summary>
    /// Set the probabilities of the entire tree following the nodes in-
    /// order
    /// </summary>
    /// <param name="currentMax"></param>
    /// <returns></returns>
    public float SetIndexes(float currentMax)
    {
        float tempMax = currentMax;
        
        if (this.leftChild != null)
        {
            tempMax = this.leftChild.SetIndexes(tempMax);
        }
        
        this.minimum = tempMax;
        this.maximum = tempMax + probability;
        tempMax = this.maximum;

        if (this.rightChild != null)
        {
            tempMax = this.rightChild.SetIndexes(tempMax);
        }

        return tempMax;
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

    public int CompareTo(System.Object obj)
    {
        RangeTreeNode<T> node = obj as RangeTreeNode<T>;
        return this.probability.CompareTo(node.probability);
    }
}