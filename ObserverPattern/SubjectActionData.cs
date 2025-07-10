using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class SubjectActionData<T,K>
    {
        private List<IObserverData<T,K>> observers = new();
        public void AddObserver(IObserverData<T,K> observer)
        {
            observers.Add(observer);
        }
        public void RemoveObserver(IObserverData<T,K> observer)
        {
            observers.Remove(observer);
        }
        public void NotifyObservers(T action, K data)
        {
            foreach (var single in observers)
            {
                single.OnNotify(action, data);
            }
        }
    }
