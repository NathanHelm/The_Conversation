using System.Collections.Generic;
using UnityEngine;

public class Subject<T>
{
    private List<IObserver<T>> observers = new();
    public void AddObserver(IObserver<T> observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserver<T> observer)
    {
        observers.Remove(observer);
    }
    public void NotifyObservers(T data)
    {
        foreach (var single in observers)
        {
            single.OnNotify(data);
        }
    }
}