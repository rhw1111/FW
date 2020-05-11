using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Collections;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 双向链表处理策略
    /// </summary>
    public interface INLinkedListStrategy
    {
        /// <summary>
        /// 新加入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">值</param>
        /// <param name="link">双向链表</param>
        void Add<T>(T value, NLinkedList<T> link);
        /// <summary>
        /// 命中已存在的节点
        /// (已经明确存在节点)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">已存在的节点</param>
        /// <param name="link">双向链表</param>
        void Hit<T>(LinkedListNode<T> node, NLinkedList<T> link);
        /// <summary>
        /// 命中值
        /// (未知是否该值已经存在)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">传入值</param>
        /// <param name="link">单向链表</param>
        /// <param name="compare">传入值与节点值的比较器,第一个参数是传入值，第二个参数是节点值</param>
        void Hit<T>(T value, NLinkedList<T> link, Func<T, T, bool> compare);
    }
}
