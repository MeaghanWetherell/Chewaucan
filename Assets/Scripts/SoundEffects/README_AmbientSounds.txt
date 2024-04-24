README FOR HOW TO ADD AND USE AMBIENT/LOCALIZED SOUNDS

Creation Information:
	This tool was created by Ellie Martin in Spring 2024 for the game Finding Lake Chewaucan.
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Basic Description:
	The ambient sound system is a modular set of game objects and scripts that allows you to place sound effects that follow three different play protocols.
	There are sounds that play randomly throughout the map, sounds that play when you are in certain regions, and sounds that play and get louder when approached. 
	There are four separate scripts involved in this system: 
		ActiveSoundManager.cs [Attached to the ActiveSoundManager GameObject under the Player prefab]
		LocalizedSound.cs [Attached to the LocalizedSound Prefab]
		RandomAmbientSound.cs [Attached to the Random Ambient Sound Manager GameObject]
		RandomAmbientSoundObject.cs [Creates a class object for RandomAmbientSound]


TO ADD A NEW AMBIENT SOUND

	IF THE SOUND PLAYS ANYWHERE AT RANDOM TIMES
		Navigate to the folder where you want to place sound objects
		In the project window, right click
			Select
				Create->Random Ambient Sound Object
				Name it anything you want (We recommend naming it like the audio clip you plan to use).
				Click on the newly created object to view it in the inspector. 
				Drag the desired audio clip into the inspector variable called AudioClip
				Set the frequency (how often you want this sound to play). Larger numbers are played more often
		Under the player prefab, find the RandomAmbientSoundManager child game object.
			In the attached Random Ambient Sound Script component, there are three lists that define what scenes 
			the sound should play in.
				General, which is what you select if the sound plays in all scenes
				Modern, which is for sounds only played in the modern map (e.g., airplane sounds)
				Pleistocene, which is for sounds only played in the Pleistocene (e.g., mammoth calls)
			In the correct list, add an element (new sound) using the plus sign. Drag the newly created Random
			Ambient Sound Object into the new slot in the list
		The frequency variable describes the chance of an audio clip playing anytime a sound is to be played. It
		is weighted along with the chances of every other random ambient sound. 0 is never, 1 is very often.


	IF THE SOUND SHOULD BE PLAYED WHEN WALKING NEAR/INTO SOMETHING
		Find prefab LocalizedSound (Assets/Prefab/Environment). 
			Drag it into the scene and place it on the map where the sounds should be triggered.
			Adjust the size of the box collider to encompass the triggered area. Sound will only play upon entering the collider.
			In the object's Localized Sound component, add audio clips to the clip list. 
				You must add at least one. Any sounds you want to have played at this location should be included in the list
				Adjust the cooldown variable to increase or decrease trigger time (how long after the sound is played and you exit the
				collider the sound can be triggered again).
		By default, the selected audio clip can be immediately retriggered (Cooldown of 0)
			The cooldown number is in seconds
		If you want the sound to repeat playing while in the collider, check the looping boolean in the script (not audio source) component

	IF THE SOUND SHOULD GROW GRADUALLY LOUDER AS YOU APPROACH AN AREA
		Find the prefab DistancedBasedSound (Assets/Prefab/Environment). 
			Drag it into the scene and place it where you want the sound to be loudest
			The prefab only has an audio source component, which is set to use 3D sound settings
			Click the drop down arrow in the audio source component by "3D Sound Settings"
			To adjust how far away you hear things, look at Min distance and Max distance (visible as blue sphere colliders)
				Min distance is how close you must be for maximum volume
				Max distance is how far away you must be for minimum volume
			To adjust how obvious the volume change is, look at the Volume Rolloff
				The default is linear, which is very obvious
		For more details on the settings, refer to the Unity documentation: https://docs.unity3d.com/Manual/class-AudioSource.html
