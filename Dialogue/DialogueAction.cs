using UnityEngine;
using System.Collections;
using System;
public class DialogueAction : MonoBehaviour
{
	public Action[] actions = new Action[] { };
	public DialogueAction(Action[] actions)
	{
		this.actions = actions;
	}	
}

