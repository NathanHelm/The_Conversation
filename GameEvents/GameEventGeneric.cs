using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public abstract class GameEventGeneric
{

}
 //is this too much abstraction
 class GameEventGeneric<T1> : GameEventGeneric { public static Dictionary<string, Func<T1>> dict1 = new Dictionary<string, Func<T1>>(); }
 class GameEventGeneric<T1, T2> : GameEventGeneric { public static Dictionary<string, Func<T1, T2>> dict2 = new Dictionary<string, Func<T1, T2>>(); }
 class GameEventGeneric<T1, T2, T3> : GameEventGeneric { public static Dictionary<string, Func<T1, T2, T3>> dict3 = new Dictionary<string, Func<T1, T2, T3>>(); }
 class GameEventGeneric<T1, T2, T3, T4> : GameEventGeneric { public static Dictionary<string, Func<T1, T2, T3, T4>> dict4 = new Dictionary<string, Func<T1, T2, T3, T4>>(); }
 class GameEventGeneric<T1, T2, T3, T4, T5> : GameEventGeneric { public static Dictionary<string, Func<T1, T2, T3, T4, T5>> dict5 = new Dictionary<string, Func<T1, T2, T3, T4, T5>>(); }

