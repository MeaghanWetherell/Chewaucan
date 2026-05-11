README ON HOW TO USE THE NARRATION SYSTEM

Creation Information:
	Our custom cutscene modifications were created by Charlie Scott in Fall 2025/Spring 2026 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.62f3
	This tool uses the TextMeshPro package (Unity Registry, Version 3.0.7) https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/license/LICENSE.html
	Our cutscenes used the Cinemachine package (Unity Registry, Version 2.10.3)

	
General Description:
	We triggered cutscenes to play when certain objects were interacted with (Quest System/TriggerUpdateOnInteract.cs) 
	and when the player entered certain areas (Misc/StartCutsceneOnTriggerEnter.cs). These cutscenes had UI elements that showed up at
	the end of the cutscene, and were often linked to quest updates. 


Triggering Cutscenes Using Interactables (TriggerUpdateOnInteract.cs)
	These cutscenes are indicated with "_INT" in our scenes. The player would mouse over objects which would turn red. Upon clicking,
	the cutscene would be triggered. In some cases, a single object could have multiple triggerable events (e.g., science log items
	occurred only after their corresponding nature log item had happened). 

	To set up this style of cutscene, create a blank object with a playable director on it.
	Add the Trigger Update on Interact script component.
	For the object that is used to trigger the interaction:
		Drag and position the object in the scene.
		Ensure it has a box collider on it that has "is trigger" checked.
		Make a copy of this object.
		Set the copy to inactive, and change the layer from Default to "Outlined"
		The copy does not need a copy of the box collider.
	Drag these objects into the correct trigger update on interact slot.
	If there is a quest associated:
		Enter the correct quest update title (should match what is in the update text file)
		Enter the correct quest id (e.g. naturelog, sciencelog)
		Drag the playable director object into the correct slot.
		If a different cutscene should trigger before this, drag that gameobject into the slot.
	If you do not plan for an entire cutscene to occur, you may instead pull a narration object directly into the Narr slot.
	Add a signal receiver track to your cutscene object.
		The signal should be Cutscene Complete
		The reaction should be TriggerUpdateOnInteract.SendQuestChanges().
	In your timeline, create a signal reciever slot.
	Drag the cutscene object itself into the signal reciever slot
	Right click on that slot at the end of the cutscene and select "add signal from asset" and select CutsceneComplete.

Triggering Cutscenes using Box Colliders/Entering an area
	These cutscenes are indicated with "_TE" in our scenes. They are all triggered by box colliders. You may set up multiple objects with box colliders
	that trigger the same cutscene, as the cutscene will only play once.

	To set up this style of cutscene, create a blank object with a playable director on it.
	Add the StartCutsceneOnTriggerEnter.cs component to this
	Drag the cutscene object into the Cutscene slot
	Include the quest id (naturelog or sciencelog)
	Make sure the update name matches the quest update file 
	Add a signal receiver track to your cutscene object.
		The signal should be Cutscene Complete
		The reaction should be StartCutsceneOnTriggerEnter.OnCutsceneEnd().
	In your timeline, create a signal reciever slot.
	Drag the cutscene object itself into the signal reciever slot
	Right click on that slot at the end of the cutscene and select "add signal from asset" and select CutsceneComplete.
	This ensures that at the end of the cutscene, the quest update UI plays.


Having trouble with cutscenes? Us too! Here are some things to double check:
	-Make sure that all your virtual cameras are inactive when the cutscene isn't in play
	- all virtual cameras should have lower priority than your regular camera (e.g. 9 rather than 10)
	- make sure that in the cinemachine object your customblends object has the correct style of blend between cameras
	- Make sure the 'play on awake' button is not ticked for any cutscene
	- if you are using our trigger systems and trying to test cutscenes, you might have to make a new game or edit 
	  the save file to re-test once the cutscene has played past the cutscene complete signal. Otherwise even with the
	  play on awake button checked, it might not play.
	- we found to make the camera system work, the cinamchine brain needed to have a track in the cutscene and the normal player following camera
	  needed to have an activation slot where it was turned back to active at the very end of the cutscene. 
	- we have a custom narration system as well, which you can see in the narration readme.

