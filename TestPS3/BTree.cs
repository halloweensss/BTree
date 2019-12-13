using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPS3
{
    public class BTree<T> where T : IComparable<T>
    {
        public int Degree { get; private set; }
        public int Height { get; private set; }

        public Node<T> Root { get; private set; }
        
        public BTree(int degree)
        {
            this.Degree = degree;
            this.Height = 1;
            this.Root = new Node<T>(degree);
        }

        public void Insert(T value)
        {
            if (!Root.HasReachedMaxEntries)
            {
                this.InsertNonFull(this.Root, value);
                return;
            }

            Node<T> oldRoot = this.Root;
            this.Root = new Node<T>(this.Degree);
            this.Root.Children.Add(oldRoot);
            this.InsertNonFull(this.Root, value);
            this.Height++;
        }

        private void InsertNonFull(Node<T> node, T value)
        {
            int positionToInsert = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) >= 0).Count();

            if (node.IsLeaf)
            {
                node.Entries.Insert(positionToInsert, new Entry<T>() {Value = value});
                return;
            }

            Node<T> child = node.Children[positionToInsert];
            if (child.HasReachedMaxEntries)
            {
                this.SplitChild(node, positionToInsert, child);
                if (value.CompareTo(node.Entries[positionToInsert].Value) > 0)
                {
                    positionToInsert++;
                }
            }
            
            this.InsertNonFull(node.Children[positionToInsert], value);

        }

        private void SplitChild(Node<T> parentNode, int nodeToBeSplitIndex, Node<T> nodeToBeSplit)
        {
            Node<T> newNode = new Node<T>(this.Degree);
            
            parentNode.Entries.Insert(nodeToBeSplitIndex, nodeToBeSplit.Entries[this.Degree - 1]);
            parentNode.Children.Insert(nodeToBeSplitIndex + 1, newNode);
            
            newNode.Entries.AddRange(nodeToBeSplit.Entries.GetRange(this.Degree, this.Degree - 1));
            
            nodeToBeSplit.Entries.RemoveRange(this.Degree - 1, this.Degree);

            if (!nodeToBeSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToBeSplit.Children.GetRange(this.Degree, this.Degree));
                nodeToBeSplit.Children.RemoveRange(this.Degree, this.Degree);
            }
        }
        public Entry<T> Search(T value)
        {
            return SearchInternal(this.Root, value);
        }

        private Entry<T> SearchInternal(Node<T> node, T value)
        {
            int i = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();
            
            if (i < node.Entries.Count && node.Entries[i].Value.CompareTo(value) == 0)
            {
                return node.Entries[i];
            }

            return node.IsLeaf ? null : this.SearchInternal(node.Children[i], value);
        }

        public void Delete(T value)
        {
            DeleteInternal(this.Root, value);

            if (this.Root.Entries.Count == 0 && !this.Root.IsLeaf)
            {
                this.Root = this.Root.Children.Single();
                this.Height--;
            }
        }

        private void DeleteInternal(Node<T> node, T value)
        {
            int i = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();

            if (i < node.Entries.Count && node.Entries[i].Value.CompareTo(value) == 0)
            {
                this.DeleteKeyFromNode(node, value, i);
                return;
            }

            if (!node.IsLeaf)
            {
                this.DeleteKeyFromSubtree(node, value, i);
            }
        }

        private void DeleteKeyFromSubtree(Node<T> parentNode, T value, int subtreeIndexInNode)
        {
            Node<T> childNode = parentNode.Children[subtreeIndexInNode];

            if (childNode.HasReachedMinEntries)
            {
                int leftIndex = subtreeIndexInNode - 1;
                Node<T> leftSibling = subtreeIndexInNode > 0 ? parentNode.Children[leftIndex] : null;

                int rightIndex = subtreeIndexInNode + 1;
                Node<T> rightSibling = subtreeIndexInNode < parentNode.Children.Count - 1
                    ? parentNode.Children[rightIndex]
                    : null;

                if (leftSibling != null && leftSibling.Entries.Count > this.Degree - 1)
                {

                    childNode.Entries.Insert(0, parentNode.Entries[leftIndex]);
                    parentNode.Entries[leftIndex] = leftSibling.Entries.Last();
                    leftSibling.Entries.RemoveAt(leftSibling.Entries.Count - 1);

                    if (!leftSibling.IsLeaf)
                    {
                        childNode.Children.Insert(0, leftSibling.Children.Last());
                        leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                    }
                }
                else if (rightSibling != null && rightSibling.Entries.Count > this.Degree - 1)
                {

                    childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                    parentNode.Entries[subtreeIndexInNode] = rightSibling.Entries.First();
                    rightSibling.Entries.RemoveAt(0);

                    if (!rightSibling.IsLeaf)
                    {
                        childNode.Children.Add(rightSibling.Children.First());
                        rightSibling.Children.RemoveAt(0);
                    }
                }
                else
                {
                    if (leftSibling != null)
                    {
                        childNode.Entries.Insert(0, parentNode.Entries[leftIndex]);
                        List<Entry<T>> oldEntries = childNode.Entries;
                        childNode.Entries = leftSibling.Entries;
                        childNode.Entries.AddRange(oldEntries);
                        if (!leftSibling.IsLeaf)
                        {
                            List<Node<T>> oldChildren = childNode.Children;
                            childNode.Children = leftSibling.Children;
                            childNode.Children.AddRange(oldChildren);
                        }

                        parentNode.Children.RemoveAt(leftIndex);
                        parentNode.Entries.RemoveAt(leftIndex);
                    }
                    else
                    {
                        childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                        childNode.Entries.AddRange(rightSibling.Entries);
                        if (!rightSibling.IsLeaf)
                        {
                            childNode.Children.AddRange(rightSibling.Children);
                        }

                        parentNode.Children.RemoveAt(rightIndex);
                        parentNode.Entries.RemoveAt(subtreeIndexInNode);
                    }
                }
            }

            this.DeleteInternal(childNode, value);
        }

        private void DeleteKeyFromNode(Node<T> node, T value, int keyIndexInNode)
        {
            if (node.IsLeaf)
            {
                node.Entries.RemoveAt(keyIndexInNode);
                return;
            }

            Node<T> predecessorChild = node.Children[keyIndexInNode];
            if (predecessorChild.Entries.Count >= this.Degree)
            {
                Entry<T> predecessor = this.DeletePredecessor(node, predecessorChild);
                node.Entries[keyIndexInNode] = predecessor;
            }
            else
            {
                Node<T> successorChild = node.Children[keyIndexInNode + 1];
                if (successorChild.Entries.Count >= this.Degree)
                {
                    Entry<T> successor = this.DeleteSuccessor(node, successorChild);
                    node.Entries[keyIndexInNode] = successor;
                }
                else
                {
                    predecessorChild.Entries.Add(node.Entries[keyIndexInNode]);
                    predecessorChild.Entries.AddRange(successorChild.Entries);
                    predecessorChild.Children.AddRange(successorChild.Children);
                    
                    node.Entries.RemoveAt(keyIndexInNode);
                    node.Children.RemoveAt(keyIndexInNode + 1);
                    
                    this.DeleteInternal(predecessorChild, value);
                }
            }
        }

        private Entry<T> DeletePredecessor(Node<T> parent, Node<T> node)
        {
            if (node.IsLeaf)
            {
                Entry<T> result = node.Entries[node.Entries.Count - 1];
                node.Entries.RemoveAt(node.Entries.Count - 1);
                
                if (node.Entries.Count < Degree - 1)
                {
                    Node<T> left = parent.Children[parent.Children.Count - 2];
                    node.Entries.AddRange(left.Entries);
                    node.Entries.Add(parent.Entries.Last());
                    parent.Entries.Remove(parent.Entries.Last());
                    parent.Children.RemoveAt(parent.Children.Count - 2);
                }

                return result;
            }

            return this.DeletePredecessor(node, node.Children.Last());
        }

        private Entry<T> DeleteSuccessor(Node<T> parent, Node<T> node)
        {
            if (node.IsLeaf)
            {
                Entry<T> result = node.Entries[0];
                node.Entries.RemoveAt(0);
                
                if (node.Entries.Count < Degree - 1)
                {
                    Node<T> right = parent.Children[1];
                    node.Entries.Add(parent.Entries.First());
                    node.Entries.AddRange(right.Entries);
                    parent.Entries.Remove(parent.Entries.First());
                    parent.Children.RemoveAt(1);
                }
                
                return result;
            }

            return this.DeleteSuccessor(node, node.Children.First());
        }

        public override string ToString()
        {
            return ToStringInternal(Root, "Root", 0);
        }

        private string ToStringInternal(Node<T> node, string text, int height)
        {
            string s = "";
            s += text + ") ";
            if (node.IsLeaf)
            {
                s += node.ToString();
                s += "(Leaf)";
                s += Environment.NewLine;
                return s;
            }
            else
            {
                s += node.ToString();
                s += Environment.NewLine;
                for (int i = 0; i < node.Children.Count; i++)
                {
                    string dot = ".";
                    if (height == 0)
                    {
                        text = "Node";
                        dot = " ";
                    }
                    s += ToStringInternal(node.Children[i],"    " + text + dot + i.ToString(),height + 1);
                }
            }

            return s;
        }
    }
}