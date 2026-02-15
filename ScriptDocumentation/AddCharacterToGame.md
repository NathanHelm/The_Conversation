## Add character to scene ##
1) Begin by adding a 2d character to scene: **Artwork/2D_Prefab/Environment_Prefab/DONT_DESTROY_Character.prefab**
2) Create a new scriptable object and add to character mono: 
- GOTO: **Assets/4Scriptable_Object/CharacterSO**
- GOTO: **Create > Scriptableobjects > CharacterSO**
- Naming formate should be \<characterID>_c_\<characterName>
- Character id should be unique 
3) Add dialogue conversation array Optional: add a unique id if it requires a unique action
- Add character player question response to question response manager/SetUpQuestionResponseManager:
- a key pair value for **conversation persistence** id is **required**.


>		npcToQuestionDialogueNpc[<characterid>] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {
		//key pair value code goes here.
		//question id							
			new (new int[] { 0, 1, 4 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(<characterid>,<dialogue>)), //based on character & conversation id, return conversation
	
			/*
			 here, question 0, 1, 4 return a conversation response of character 1, with question id 2. 
			 
             persistence id = 0, unless change otherwise

             this code block is required.

             */
		});
## IDs:
- **31**: a unique dialogue id used for all memory dialogue
- **1XXX**: Body
- **2XXX**: Character
- **3XXX**: Clue

### Advanced ###
- **21XX**: won't run ledger interview state (press tab state)
- **22XX**: narrative character (might change this)