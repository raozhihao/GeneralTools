using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleAppTest
{
    internal class TestEnumerator
    {
    }

    public class ManageClass<T> : ICollection<T>
    {
        private readonly EnumeratorModel<T> _model;
        public ManageClass()
        {
            _model = new EnumeratorModel<T>();
        }
        public ManageClass(IEnumerable<T> enumerable)
        {
            _model = new EnumeratorModel<T>(enumerable);
        }

        public int Count => _model.List.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            _model.List.Add(item);
        }

        public void Clear()
        {
            _model.List.Clear();
        }

        public bool Contains(T item)
        {
            return _model.List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_model.List.ToArray(), array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _model;
        }

        public bool Remove(T item)
        {
            return _model.List.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _model;
        }

        private class EnumeratorModel<E> : IEnumerator<T>
        {
            public List<T> List = new List<T>();
            private int _position = -1;

            public EnumeratorModel() { }
            public EnumeratorModel(IEnumerable<T> enumerable)
            {
                List.AddRange(enumerable);
                _position = -1;
            }


            public T Current => List[_position];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                List.Clear();
                List = null;
            }

            public bool MoveNext()
            {
                return ++_position < List.Count;
            }

            public void Reset()
            {
                _position = -1;
            }
        }
    }


    public class Person
    {

    }
}
