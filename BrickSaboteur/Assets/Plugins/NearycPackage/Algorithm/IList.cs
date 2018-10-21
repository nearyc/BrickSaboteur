namespace Nearyc.Collections
{
    public interface IList<T>
    {
        int Count { get; }
        void AddToLast(T t);
        void Remove(T t);
        void RemoveAt(int index);
        void Insert(int index, T t);
        T GetElement(int index);
        int FindIndexFirstMatch(T t);
        T this[int index] { get; set; }
    }
}
