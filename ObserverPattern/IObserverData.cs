using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public interface IObserverData<T, K>
{
    public void OnNotify(T actionData, K myObject);
  
}
