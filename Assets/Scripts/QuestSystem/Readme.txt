Art Team: If you want to write a quest, you need to do three things: 
1. write a quest description in a text file containing a short description of the quest and a long description of the quest (long is optional)
2. Right click in project view > create > QuestObj and fill out the fields in your new quest object. All of the fields have tooltips for more in depth descriptions of what they are.
3. Tell someone from the code team you made a quest and what the objective is. They'll finish hooking it up. 
----------------------------------------------------------------------------------------------------------------
Code Team: Quest system is pretty much a fully closed system. If you need to refactor it, look at my comments in the class files.
If you just need to interface with it to get a quest hooked up you have two options:
1. Write a class that extends QuestHandler, assign the appropriate quest object in the inspector, and call this.startQuest to initialize the quest and this.progressQuest to progress it.
2. Instantiate a QuestNode using QuestManager.questManager.createQuestNode(), then call addCount on it to progress it. createQuestNode will return an existing quest with the same ID if there is one or create one for you if there isn't.
or, do some combination of these. QuestHandler is basically just a simple wrapper over the second. 
The only methods outside the QuestHandler from the quest system you should ever call externally are the functions above awake
You can access instance variables on QuestNode if you'd like, but treat them as readonly.