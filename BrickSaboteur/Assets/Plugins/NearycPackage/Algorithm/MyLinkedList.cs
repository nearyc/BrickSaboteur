using System;
using System.Collections.Generic;

namespace Nearyc.Collections
{
    public sealed class MyLinkedList<T> : IList<T>
    {
        private int _count;
        public int Count => Count;
        private MyLinkedListNode<T> head;
        public MyLinkedList()
        {
            this.head = null;
            _count = 0;
        }
        public T this[int index]
        {
            get
            {
                return GetElement(index);
            }
            set
            {
                ChangeIndex(index, value);
            }
        }
        /// <summary>
        /// 添加新的元素
        /// </summary>
        /// <param name="t">待添加的元素</param>
        public void AddToLast(T t)
        {
            var newNode = new MyLinkedListNode<T>(t);
            var temp = head;
            if (this.head == null)
            {
                head = newNode;
            }
            else
            {
                while (temp.next != null)
                {
                    temp = temp.next;
                }
                temp.next = newNode;
            }
            _count++;
        }
        /// <summary>
        /// 移除第一个匹配的索引
        /// </summary>
        /// <param name="t">待匹配的元素</param>
        public void Remove(T t)
        {
            var temp = head;
            var last = new MyLinkedListNode<T>();
            if (temp == null)
                throw new Exception();
            if (temp.value.Equals(t))
            {
                _count--;
                return;
            }
            while (!temp.value.Equals(t) && temp.next != null)
            {
                last = temp;
                temp = temp.next;
            }
            last.next = temp.next;
            temp = null;
            _count--;

        }
        /// <summary>
        /// 取得索引处的元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引处的元素</returns>
        public T GetElement(int index)
        {
            if (index > _count || index < 0)
                throw new Exception();

            var temp = head;
            if (index >= 1)
                for (int i = 0; i < index; i++)
                {
                    temp = temp.next;
                }
            return temp.value;
        }
        /// <summary>
        /// 根据元素值找到第一个匹配的索引
        /// </summary>
        /// <param name="t">待匹配的元素</param>
        /// <returns>第一个匹配的索引</returns>
        public int FindIndexFirstMatch(T t)
        {
            var tempIndex = -1;
            var temp = head;
            if (temp == null)
                return tempIndex;
            if (temp.value.Equals(t))
                tempIndex++;
            while (!temp.value.Equals(t) && temp.next != null)
            {
                temp = temp.next;
                tempIndex++;
            }
            return tempIndex;
        }
        /// <summary>
        /// 改变指定索引处的元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="value">新的元素</param>
        private void ChangeIndex(int index, T value)
        {
            if (index > _count || index < 0)
                throw new Exception();

            var temp = head;
            //var last = new LinkedListNode<T>();
            for (int i = 0; i < index + 1; i++)
            {
                temp = temp.next;
            }
            temp.value = value;
            _count--;
        }
        /// <summary>
        /// 打印每一个元素
        /// </summary>
        public void ShowAll()
        {
            var temp = head;
            Console.Write($"--{temp.value}--");
            while (temp.next != null)
            {
                temp = temp.next;
                Console.Write($"--{temp.value}--");
            }
            Console.WriteLine("\\\\");
        }
        /// <summary>
        /// 移除指定索引处的元素
        /// </summary>
        /// <param name="index">索引</param>
        public void RemoveAt(int index)
        {
            if (index > _count || index < 0)
                throw new Exception();

            var temp = head;
            var last = new MyLinkedListNode<T>();
            if (index == 0)
            {
                head = head.next;
                _count--;
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    last = temp;
                    temp = temp.next;
                }
                last.next = temp.next;
                temp = null;
                _count--;
            }
        }
        /// <summary>
        /// 将新元素插入在索引处
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="t">新的元素</param>
        public void Insert(int index, T t)
        {
            if (index > _count || index < 0)
                throw new Exception();

            var temp = head;
            var last = new MyLinkedListNode<T>();
            if (index >= 1)
                for (int i = 0; i < index; i++)
                {
                    last = temp;
                    temp = temp.next;
                }
            var newNode = new MyLinkedListNode<T>(t);
            last.next = newNode;
            newNode.next = temp;
            _count++;
            if (index == 0)
                head = newNode;
        }
    }
}
