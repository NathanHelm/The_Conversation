using UnityEngine;
using System.Collections;
using System;
namespace Data
{
	public class CutsceneData : StaticInstanceData<CutsceneData>, IExecution
	{
		public object[] snapShotCutscene { get; set; }
		public (Action, float)[] cutsceneActionsAndTimeTurnaround { get; set; }
        public (Action, bool)[] cutsceneActionAndConditions { get; set; }
     

		
	}
}

