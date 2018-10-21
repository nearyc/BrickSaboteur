namespace Nearyc.Collections
{
    public sealed class MyLinkedListNode<T>
    {
        public MyLinkedListNode<T> next;
        public T value;

        public MyLinkedListNode(T value)
        {
            this.value = value;
            next = null;
        }
        public MyLinkedListNode() : this(default(T))
        {

        }
    }
}
