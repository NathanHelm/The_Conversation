using UnityEngine;
using System.Collections;
using System;
namespace Data
{
	public class CutsceneData : StaticInstance<CutsceneData>
	{
		public object[] snapShotCutscene { get; set; }
		public (Action, float)[] cutsceneActionsAndTimeTurnaround { get; set; }
        public (Action, bool)[] cutsceneActionAndConditions { get; set; }
        public (string, Type)[] snapShotStates { get; set; }
		public (string, Type)[] replaceSnapShots { get; set; }

		
	}
}

