README ON HOW TO USE THE NARRATION SYSTEM

Creation Information:
	This tool was created by Charlie Scott in Fall 2025/Spring 2026 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.62f3
	This tool uses the TextMeshPro package (Unity Registry, Version 3.0.7) https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/license/LICENSE.html

	
General Description:
	The narration system is a system designed to manage what narrations can play and when, set up subtitles appropriately, 
	and store information about played narrations in a narration journal. There were several different scenarios in which narrations
	needed to play including cutscenes, when certain areas were entered, when interacting with certain objects, and upon specific events.

Narration Manager:
	The NarrationManager.cs script controls and tracks which narrations have been played and stores them in the Narration Journal. 
	It is attached to an object in the PersistentObjects Scene, and it tracks objects using the FullList narration object.
	Only objects hardcoded to play during the bonepile script need to be in this narration list object. 


Narration Triggers:
	You may find the different narration trigger scripts under "Narration / Triggers".
		AstrolabeNarration.cs - Not currently used, was previously a prompt to open the Astrolabe. Requires LoadGuiManager.cs
		CutsceneNarrTrigger.cs - Used extensively in cutscenes
		PlayOnCollisionEnter.cs - Used to trigger narration using a collider; must have default playability selected!
		PlayOnLoad.cs - Plays automatically when a specific scene loads
		PlayRandomlyOnCollisionEnter.cs - Plays and replays when player interacts with a collider, but has a delay effect
		PleistoceneLoad1.cs - Depreciated in favor of specific cutscenes, but played when the Pleistocene opened. Requires NarrationManager.cs

Narration Skipping:
	The SkipNarrOnInteract.cs script controls whether or not a narration can get skipped, and tracks which key is bound to that. 
	It requires the SoundManager.cs script to function.

To Set Up Narration Objects:
	To set up a Narration, create a Narration scriptable object in the Resources folder. 
	The name of this object should begin with the folder name and be followed by a number e.g. RC1. 
	Add the narration clip associated with the object and set whether it is playable by default (if not, it will not be able to be played by 		
		most narration scripts until set playable in code). 
	Optionally, create a subtitle file for the Narration. 
	This Narration object can then be slotted into several scripts under the "Triggers" folder for simple triggers, 
	such as playing when the player enters a trigger volume, or passed off to a programmer to trigger with bespoke code. 

To Set Up Subtitles:
	Create a text asset file for each unique audio file of narration.
	Each line of the file should begin with a timestamp when that line should STOP displaying 
	It should be followed by a space, then the character "|", then another space, then the text that should be displayed, e.g:
		0:03 | This line would stop playing at 3 seconds.
		1:20 | This line would start playing at 3 seconds and stop playing at 1 minute 20 seconds
	See more examples in subtitles folder for format. 

To Trigger Narration During a Cutscene:
	Create an Empty Gameobject. Ours are labeled with "Narr" and the narration object identifier e.g. "Narr.RC1"
	Add the CutsceneNarrTrigger.cs script to the object
	Add the correct narration object in the "Narr to Play" slot
	If a second narration should play right afterwards, make a second game object following these instructions, 
		then pull that object into the "Next Narr" slot.
	
	Add a signal receiver component to the first Narr object only. 
	The signal should be PlayNarration, and the Reaction is CutsceneNarrTrigger.StartNarr.

	In the cutscene timeline, create a signal track. Drag the Narr object into that slot.
	Add a signal emitter from the asset - select PlayNarration.

Manually Coded Events
	There were a few events that were manually coded. This includes the narration during the tutorial, which uses the BonepileScript.cs
	This is attached to the gameobject BonePileScriptHelper in the Modern Map. It requires the PlayOnLoad.cs script, and controls
	which narrations play when the object the player is examining is incorrect.
	