using System;
using System.Collections.Generic;
using System.Text;


namespace MSLibrary.Collections
{
    /// <summary>
    /// /双向链表
    /// </summary>
    public class NLinkedList<T> : LinkedList<T>
    {
        private int _maxLength = 1;
        public NLinkedList() : base()
        {
            CurrentLength = 0;
        }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("双向链表长度不能小于1");
                }
                _maxLength = value;
            }
        }
        /// <summary>
        /// 当前长度
        /// </summary>
        public int CurrentLength
        {
            get; set;
        }


        public new LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = null;
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    result = base.AddAfter(node, value);
                }
                else
                {
                    CurrentLength++;
                    if (node == First)
                    {
                        result = base.AddFirst(value);
                    }
                    else if (node == Last)
                    {
                        result = base.AddAfter(node.Previous, value);
                    }
                    else
                    {
                        result = base.AddAfter(node, value);
                    }

                    RemoveLast();
                }
            }
            InnerOnAdded(node, result.Value);
            return result;
        }

        public new void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    base.AddAfter(node, newNode);
                }
                else
                {
                    CurrentLength++;
                    if (node == First)
                    {
                        base.AddFirst(newNode);
                    }
                    else if (node == Last)
                    {
                        base.AddAfter(node.Previous, newNode);
                    }
                    else
                    {
                        base.AddAfter(node, newNode);
                    }

                    RemoveLast();
                }
            }

            InnerOnAdded(newNode, newNode.Value);
        }

        public new LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = null;
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    result = base.AddBefore(node, value);
                }
                else
                {
                    CurrentLength++;
                    if (node == First)
                    {
                        result = base.AddFirst(value);
                    }
                    else if (node == Last)
                    {
                        result = base.AddAfter(node.Previous, value);
                    }
                    else
                    {
                        result = base.AddBefore(node, value);
                    }

                    RemoveLast();
                }
            }
            InnerOnAdded(result, result.Value);
            return result;
        }

        public new void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    base.AddBefore(node, newNode);
                }
                else
                {
                    CurrentLength++;
                    if (node == First)
                    {
                        base.AddFirst(newNode);
                    }
                    else if (node == Last)
                    {
                        base.AddAfter(node.Previous, newNode);
                    }
                    else
                    {
                        base.AddBefore(node, newNode);
                    }

                    RemoveLast();
                }
            }

            InnerOnAdded(newNode, newNode.Value);
        }

        public new LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> result = null;
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    result = base.AddFirst(value);
                }
                else
                {
                    CurrentLength++;
                    result = base.AddFirst(value);
                    RemoveLast();
                }
            }
            InnerOnAdded(result, result.Value);
            return result;
        }

        public new void AddFirst(LinkedListNode<T> node)
        {
            lock (this)
            {
                if (CurrentLength <= MaxLength)
                {
                    //CurrentLength++;
                    try
                    {
                        base.Remove(node);
                    }
                    catch
                    {

                    }
                    base.AddFirst(node);
                }
                else
                {
                    try
                    {
                        base.Remove(node);
                    }
                    catch
                    {

                    }
                    base.AddFirst(node);
                    RemoveLast();
                }
            }
            //InnerOnAdded(node, node.Value);
        }

        public new LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> result = null;
            lock (this)
            {
                if (CurrentLength < MaxLength)
                {
                    CurrentLength++;
                    result = base.AddLast(value);
                }
                else
                {
                    CurrentLength++;
                    RemoveLast();
                    result = base.AddLast(value);
                }
            }
            InnerOnAdded(result, result.Value);
            return result;
        }


        public new void AddLast(LinkedListNode<T> node)
        {
            lock (this)
            {
                if (CurrentLength <= MaxLength)
                {
                    //CurrentLength++;
                    try
                    {
                        base.Remove(node);
                    }
                    catch
                    {

                    }
                    base.AddLast(node);
                }
                else
                {
                    RemoveLast();
                    try
                    {
                        base.Remove(node);
                    }
                    catch
                    {

                    }
                    base.AddLast(node);
                }
            }
            //InnerOnAdded(node, node.Value);
        }


        public new bool Remove(T value)
        {
            bool result = false;
            lock (this)
            {
                result = base.Remove(value);
                if (result)
                {
                    CurrentLength--;
                }
            }

            if (result)
            {
                InnerOnRemoved(value);
            }
            return result;
        }

        public new void Remove(LinkedListNode<T> node)
        {
            lock (this)
            {
                try
                {
                    base.Remove(node);
                }
                catch
                {

                }
                CurrentLength--;
            }
            InnerOnRemoved(node.Value);
        }

        public new void RemoveFirst()
        {
            T value = default(T);
            bool isRemove = false;
            lock (this)
            {
                if (First != null)
                {
                    value = First.Value;
                    base.RemoveFirst();
                    CurrentLength--;
                    isRemove = true;
                }
            }
            if (isRemove)
            {
                InnerOnRemoved(value);
            }
        }

        public new void RemoveLast()
        {
            T value = default(T);
            bool isRemove = false;
            lock (this)
            {
                if (Last != null)
                {
                    value = Last.Value;
                    base.RemoveLast();
                    CurrentLength--;
                    isRemove = true;
                }
            }
            if (isRemove)
            {
                InnerOnRemoved(value);
            }
        }


        /// <summary>
        /// 当节点移除后触发
        /// </summary>
        public Action<T> OnRemoved
        {
            get; set;
        }
        /// <summary>
        /// 当节点加入后触发
        /// </summary>
        public Action<LinkedListNode<T>, T> OnAdded
        {
            get; set;
        }


        private void InnerOnAdded(LinkedListNode<T> node, T value)
        {
            OnAdded?.Invoke(node, value);
        }

        private void InnerOnRemoved(T value)
        {
            OnRemoved?.Invoke(value);
        }
    }
}
