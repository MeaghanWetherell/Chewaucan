HOW TO ADD AND USE AMBIENT/LOCALIZED SOUNDS

TO ADD A NEW AMBIENT SOUND

	IF THE SOUND PLAYS ANYWHERE AT RANDOM TIMES
		Create a new instance of Random Ambient Sound Object by right clicking in any folder in assets, and going
		create->Random Ambient Sound Object and name is anything you want (I'd name it similar to the audio clip 
		you plan to use).
		Then click on the newly created object to view it in the inspector. Drag the desired audio clip into the
		inspector variable, and set the frequency based on how often you want this sound to play.
		The frequency variable describes the chance of this audio clip playing anytime a sound is to be played. it
		is combined with the chances of every other random ambient sound.
		Under the player prefab, find the RandomAmbientSoundManager object. This object contains the RandomAmbientSound
		component which you will put your newly created object into. This component has 3 lists, and you will place the
		object in one of these lists based on what map the sound should play in. If the sound should play in both the
		modern and pleistocene map, place the object into the general ambient sounds list. Otherwise, place it in 

	IF THE SOUND SHOULD BE PLAYED WHEN WALKING NEAR/INTO SOMETHING
		Use the prefab LocalizedSound found in the Prefab/Environment folder. Simply drag it into the scene and place
		it where you want the sound to be triggered. In the objects Localized Sound component, add audio clips to the
		clip list. You should add at least 1, just any sounds you want to have possibly play at this location. If you
		want only 1 sound to ever play, only add 1, and add more if you want it to pick from multiple possible clips.
		By default, the selected audio clip will play a single time when triggered and can be immediately retriggered,
		but you may change this behavior based on your needs.
		If you want there to be some time before the sound can be triggered again, set the cooldown variable to the number
		of seconds until it can be triggered again. 
		If you want the sound to repeat playing while in the collider, check the looping boolean to true.

	IF THE SOUND SHOULD GROW GRADUALLY LOUDER AS YOU APPROACH AN AREA
		Use the prefab DistancedBasedSound also found in the Prefab/Environment folder. Drag it into the scene and
		place it where you want the sound to be loudest. The prefab only has an audio source component, which is
		set to use 3D sound settings, and you will modify those to your liking. Click the drop down arrow in the 
		audio source component by "3D Sound Settings" to see the settings.
		The most significant settings will be min distance and max distance, which you can see as blue sphere colliders
		as you change the values. By default the prefab is set to linear rolloff to have a noticable effect, but you may
		change this.
		For more details on the settings, refer to the unity documentation: https://docs.unity3d.com/Manual/class-AudioSource.html
