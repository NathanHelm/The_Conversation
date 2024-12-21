using System;
using System.Collections.Generic;

public interface IDialogueManagerInterface
{

    public int getDialogueQuestionId();  //get the dialogueQuestionId
    public int getDialogueCharacterId(); //
    public Dictionary<int, Action> getDialogueAction(int characterId, int playerQuestionId);
    public DialogueObject[] GetDialogueConversation(int characterId, int playerQuestionId);
    

}

