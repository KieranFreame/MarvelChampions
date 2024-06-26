using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

public class ObservableSet<T>
{
    public ObservableCollection<T> Collection { get; private set; }

    public ObservableSet()
    {
        Collection = new ObservableCollection<T>();
    }

    public void AddItem(T item)
    {
        if (Collection.Contains(item)) return;

        Collection.Add(item);
    }

    public void RemoveItem(T item)=> Collection.Remove(item);
    public bool Contains(T item) => Collection.Contains(item);

    public void Subscribe(NotifyCollectionChangedEventHandler func) => Collection.CollectionChanged += func;
    public void Unsubscribe(NotifyCollectionChangedEventHandler func) => Collection.CollectionChanged -= func;

}
