using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BashSoft.Contracts;

namespace BashSoft.DataStructures
{
    public class SimpleSortedList<T> : ISimpleOrderedBag<T> where T : IComparable<T>
    {
        private const int DefaultSize = 16;

        private T[] innerCollection;
        private int size;
        private IComparer<T> comparision;

        public SimpleSortedList(IComparer<T> comparer, int capacity)
        {
            InitializeInnerCollection(capacity);
            this.comparision = comparer;
        }

        public SimpleSortedList(int capacity)
            : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), capacity)
        {
        }

        public SimpleSortedList(IComparer<T> comparer)
            : this(comparer, DefaultSize)
        {
        }

        public SimpleSortedList()
            : this(DefaultSize)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Size; i++)
            {
                yield return this.innerCollection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T element)
        {
            if (this.innerCollection.Length == this.Size)
            {
                Resize();
            }

            this.innerCollection[size] = element;

            this.Size++;
            Array.Sort(this.innerCollection, 0, this.Size, comparision);
        }

        public void AddAll(ICollection<T> collection)
        {
            if (this.Size + collection.Count >= this.innerCollection.Length)
            {
                this.MultiResize(collection);
            }

            foreach (var element in collection)
            {
                this.innerCollection[this.Size] = element;
                this.Size++;
            }

            Array.Sort(this.innerCollection, 0, this.Size, this.comparision);
        }

        private void MultiResize(ICollection<T> collection)
        {
            int newSize = this.innerCollection.Length * 2;

            while (this.Size + collection.Count >= newSize)
            {
                newSize *= 2;
            }

            T[] newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.Size);

            this.innerCollection = newCollection;
        }

        public int Size
        {
            get { return this.size; }
            private set { this.size++; }
        }

        public string JoinWith(string joiner)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var element in this)
            {
                builder.Append(element);
                builder.Append(joiner);
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity cannot be negative!");
            }

            this.innerCollection = new T[capacity];
        }

        private void Resize()
        {
            T[] newCollection = new T[this.Size * 2];
            Array.Copy(innerCollection, newCollection, this.Size);

            this.innerCollection = newCollection;
        }
    }
}