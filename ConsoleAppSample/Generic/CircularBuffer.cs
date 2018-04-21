using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IronPython.Modules;

namespace ConsoleAppSample.Generic
{
     interface IBuffer<T>: IEnumerable<T>
    {
        bool IsEmpty { get; }

        void Write(T value);

        T Read();
    }

    public class Buffer<T>:IBuffer<T>
    {
        protected Queue<T> queue = new Queue<T>();

        public virtual bool IsEmpty
        {
            get { return queue.Count == 0; }
        }
        public virtual void Write(T value)
        {
            queue.Enqueue(value);
        }

        public virtual T Read()
        {
            return queue.Dequeue();
        }

        public IEnumerator<T> GetEnumerator()
        {
            //return queue.GetEnumerator();
            foreach (var item in queue)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queue.GetEnumerator();

        }
    }

    public class CircularBuffer<T> : Buffer<T>
    {
        private int _capacity;

        public CircularBuffer(int capacity=10)
        {
            _capacity = capacity;
        }

        public override void Write(T value)
        {
            base.Write(value);
            if (queue.Count > _capacity)
                queue.Dequeue();
        }

        public override T Read()
        {
            return base.Read();
        }

        public bool IsFull
        {
            get { return queue.Count == _capacity; }
        }
    }
    public class CircularBufferOld<T> : IBuffer<T>
    {
        private T[] _buffer;
        private int _start, _end;

        public CircularBufferOld() : this(capacity:10)
        {
        }

        public CircularBufferOld(int capacity)
        {
            _buffer = new T[capacity + 1];
            _start = 0;
            _end = 0;

        }
        
        public void Write(T value)
        {
            _buffer[_end] = value;
            _end = (_end + 1) % _buffer.Length;
            if (_end == _start)
                _start = (_start + 1) % _buffer.Length;

        }

        public T Read()
        {
            var result = _buffer[_start];
            _start = (_start + 1) % _buffer.Length;
            return result;

        }

        public int Capacity
        {
            get { return _buffer.Length; }
        }

        public bool IsEmpty
        {
            get { return _end == _start; }
        }

        public bool IsFull
        {
            get { return (_end + 1) % _buffer.Length == _start; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
