using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
APPLY THIS INTERFACE TO ALL DATA THAT REQUIRES SAVING

*/

public interface ISaveLoad
{
  public (FileNames, JsonObject[])[] Save(); //filename, object you wish to be stored

  
  public void Load();

}
