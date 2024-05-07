README ON HOW TO USE THE MOVEMENT SOUND EFFECTS SYSTEM

Creation Information:
	This tool was created by Ellie Martin in Spring 2024 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

	This tool was made with the help of this tutorial: https://johnleonardfrench.com/terrain-footsteps-in-unity-how-to-detect-different-textures/

Version Information:
	This tool was prepared using Unity version 2022.3.10f1

General Description
	This system ensures that the player will hear footsteps as they move (or swimming sounds when swimming) and that these will 
	update to reflect the type of terrain the player is walking across. The general workflow is that the scripts will check for keywords
	in the terrain layer name in the active terrain texture palette, and use that to correspond to specific sound sampling.

	Terrain layers are what Unity uses to stitch together different JPG files into a texture (e.g., base color and normal map). These are used
	in the 'paint texture' option for terrains. Because you can paint different terrain layer textures on top of one another with different degrees 
	of transparency, this system evaluates and calls out the texture with the strongest expression at each spot, 
	using the texture's alpha values (or degree of transparency)

	
	There are several scripts that are directly associated with this system which are called out by other scripts. Both 
	MovementSoundEffects and CheckGroundTexture are also attached directly to the player object. 
		MovementSoundEffects.cs [in SwimmingMovement.cs, LandMovement.cs]
		CheckGroundTexture.cs [in LandMovement.cs, MovementSoundEffects.cs]
		MovementSounds.cs

CheckGroundTexture.cs
	This script is attached to the Player object. It looks for and finds the terrain type the player is standing on. 
	It does this by returning the alpha map values of each terrain layer the player is standing on 
	(essentially, the relative degree of transparency for each possible terrain layer) and then returning the name of the 
	terrain layer with the largest alpha value.

	It looks for terrain layers in the currently active terrain palette. The script will automatically get the upper bound of
	the alpha map values array from the terrain, so any changes to the terrain palette may be automatically detected. 
	To avoid potential problems, ensure that every terrain uses the same terrain palette.

MovementSoundEffects.cs
	This script is attached to the Player object. It plays different sounds according to keywords in the layer name of the texture
	with the greatest alpha value. Which sound to play based on the layer name is defined by a MovementSounds object
	and a list of keywords that pertain to the particular sound type.

	To edit which sound group is intended to play on different terrain groups:
		Select the player object and navigate to the Movement Sound Effects component
		Look for the Movement Sounds Info variable
		Press the drop-down arrow to see a list of class objects (called number Elements).
			Each class object contains a MovementSounds object, 
			and a list of strings (called Keywords).
			The sound object contains all possible played sounds, and the keywords are in the terrain layer names associated.
			To add a new sound group, click the plus sign under Movement Sound Info
				Drag the desired Movement Sounds object into the field labeled Sounds
				The list of keywords may be already populated with default values; to change it:
					Click on Keywords to view the list. If an element already exists, change it to a keyword relevant
					to the terrain sound. Refer to "How to choose keywords" below for how to select a proper keyword.

	How to choose keywords
		Keywords are what the script uses to identify what sounds should play for the current texture layer name.
		You define the keywords that detemine which sounds play for each terrain layer by adding elements to the 
		list named Keywords and editing the strings in each element.
		They need to match portions of the filename of the Terrain Layer, but are not case sensitive.
		So sand3 and 3_sandy would both be names detected by the keyword "sand".

		In the event that you use two keywords in a terrain layer name (e.g., SandandGrass1), it will default to whichever is listed first
		in the movement sounds object. To ensure reliability, make sure names are unique between sound types.

		If you do not define a keyword in the Movement Sounds object, the first sound on the list (currently, Rocks) will play.


	This script also contains two lists that pertain to sounds played when swimming. Swim Sounds are the sound effects played
	when swimming at normal speed, and Swim Sprint Sounds are the sound effects played when sprinting while swimming. You may
	add as many audio clips as you like to each list, and the script will select a random one to play each time.


MovementSounds.cs
	This script defines a scriptable object containing 3 lists, an instance of which can be created in the editor.
	To create a new MovementSounds Object:
		Navigate to the folder where you want to place sound objects
		In the project window, right click
			Select
				Create, then Movement Sounds
				Name it anything you want (We recommend using similar naming conventions to your terrain palette).
				Click on the newly created object to view it in the inspector.

	Each MovementSounds object allows you to include sounds for footsteps, jumping, and landing sounds that should be customized 
	to a particular terrain. You can add a new sound step, jump, or land sound for the object by clicking the plus sign under
	any array and dragging the desired AudioClip into it. There is also a list of audio clips to be used when the player is 
	sprinting.
	
	Each object's variable can have multiple sounds that are randomly played, to make the soundscape more varied. 
	For example, you could have 3 different possible landing sounds when you land on rocks.  

	Currently, MovementSound objects and their associated sound files are stored under Assets/Sounds with 
	subfolders for sound types.

Troubleshooting and Errors
	If you happen to have the Player object selected when going into play mode, you may come across this error after
	going back to edit mode: “NullReferenceException: SerializedObject of SerializedProperty has been Disposed…”
		This is just an error with the inspector UI display and is not dangerous to the gameplay
		Select any other object when going into game mode to stop the message from appearing.

Suggestions and Hints
	If you are filming sounds for your own footsteps, make sure to keep those steps short! It's very easy to fall into the trap of walking
	slowly and then the audio files are too long. This is especially obvious when the player is running. An ideal footstep sound should be
	between ___ and ___ seconds long.  


