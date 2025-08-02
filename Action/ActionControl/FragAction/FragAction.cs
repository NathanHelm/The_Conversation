using System;
using UnityEngine;
using System.Diagnostics;

namespace ActionControl
{
    public class FragAction<T>
    {
        /*
        like a grenade, a frag action will only run the action ONCE. once played it is terminated.
        */
        public void FragOut(Action<T> actionController,Action<T> playAction)
        {
            UnityEngine.Debug.Log("Frag out!!");
            playAction += m => { actionController -= playAction; };
            actionController += playAction; 
        }
    }
}