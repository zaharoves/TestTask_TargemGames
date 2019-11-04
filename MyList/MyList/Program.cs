using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{

    class MyList<T> : IList<T>
    {
        T[] myList = new T[0];
        public T this[int index] { get => myList[index]; set => myList[index] = value; }

        public int Count => myList.Length;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            T[] myListNew = new T[myList.Length + 1];
            myList.CopyTo(myListNew, 0);
            myListNew[myListNew.Length-1] = item;
            myList = myListNew;
        }

        public void Clear()
        {
            T[] myList = new T[0];
        }

        public bool Contains(T item)
        {
            return myList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            myList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in myList)
            {
                yield return item;
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < myList.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(myList[i], item))
                {
                    return i;
                }
                else
                {
                    continue;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            T[] myListNew = new T[myList.Length + 1];
            for (int i = 0; i < index; i++)
            {
                myListNew[i] = myList[i];
            }
            myListNew[index] = item;
            for (int i = index+1; i < myListNew.Length; i++)
            {
                myListNew[i] = myList[i - 1];
            }
            myList = myListNew;
        }

        public bool Remove(T item)
        {
            int indexFind = 0;
            T[] myListNew = new T[myList.Length-1];
            for (int i=0; i<myList.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(myList[i], item) )
                {
                    indexFind = i;
                    break;
                }
                else if (!EqualityComparer<T>.Default.Equals(myList[i], item) && (i != myList.Length-1))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            for (int j=0; j < indexFind; j++)
            {
                myListNew[j] = myList[j];
            }

            for (int j = indexFind + 1; j < myList.Length; j++)
            {
                myListNew[j-1] = myList[j];
            }
            myList = myListNew;
            return true; ;
        }

        public void RemoveAt(int index)
        {
            T[] myListNew = new T[myList.Length - 1];
            for (int j = 0; j < index; j++)
            {
                myListNew[j] = myList[j];
            }

            for (int j = index + 1; j < myList.Length; j++)
            {
                myListNew[j - 1] = myList[j];
            }
            myList = myListNew;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
