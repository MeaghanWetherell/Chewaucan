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
	update to reflect the type of texture the player is walking across.
	
	There are several scripts that are directly associated with this system which are called out by other scripts.
		MovementSoundEffects.cs [in SwimmingMovement.cs, LandMovement.cs]
		CheckGroundTexture.cs [in LandMovement.cs, MovementSoundEffects.cs]
		MovementSounds.cs

CheckGroundTexture.cs
	This script is attached to the Player object. It looks for and finds the texture type the player is standing on. 
	It does this by returning the alpha map values of each terrain texture the player is standing on 
	(essentially, the relative degree of transparency for each possible texture) and then returning the name of the 
	texture layer with the largest alpha value.

	It looks for textures in the currently active terrain palette. The script will automatically get the upper bound of
	the alpha map values array from the terrain, so any changes to the terrain palette may be automotically detected.
	Depending on how terrain works, the terrain may only get alpha values for textures that exist underneath it.
	Overall, to avoid potential problems, ensure that every terrain uses the same terrain palette.

MovementSoundEffects.cs
	This script is attached to the Player object. It plays certain sounds according to the layer name of the texture
	with the greatest alpha value. Which sound to play based on the layer name is defined by a MovementSounds object
	and a list of keywords that pertain to the particular sound type.

	To edit which sound group is intended to play on different terrain groups:
		Select the player object and navigate to the Movement Sound Effects component
		Look for the Movement Sounds Info variable
			This is a list of class objects, and each class object contains a MovementSounds object, 
			and a list of strings (called Keywords).
			To add a new sound group, click the plus sign to add a new class object
				Drag the desired Movement Sounds object into the field labeled Sounds
				The list of keywords may be already populated with default values, to change it.
					Click on Keywords to view the list. If an element already exists, change it to a keyword relevant
					to the terrain sound. Refer to "How to choose keywords" below for how to select a proper keyword.

	How to choose keywords
		keywords are what the script uses to identify what sounds should play for the current texture layer name, so 
		the keywords should be substrings of the texture names that would trigger the related sound. Do not worry
		about case matching.
		(For example: many rock sound textures are named "Crack1", etc, so the keywords list contains the work crack)

	This script also creates a public modifiable variable for what sounds will play when swimming. 
	Currently, this only accepts one movement sound and not a selection of randomly drawn sounds.

	If you happen to have the Player object selected when going into play mode, you may come across this error after
	going back to edit mode: “NullReferenceException: SerializedObject of SerializedProperty has been Disposed…”
		This is just an error with the inspector ui display and is not dangerous to the gameplay
		Select any other object to stop the message from appearing.
		This error only appears when the Player is selected when entering play mode, not under any other circumstances


MovementSounds.cs
	This script defines a scriptable object containing 3 lists, an instance of which can be created in the editor.
	To create a new MovementSounds Object:
		Navigate to the folder where you want to place sound objects
		In the project window, right click
			Select
				Create->Movement Sounds
				Name it anything you want (We recommend naming it like the type of terrain it sounds like).
				Click on the newly created object to view it in the inspector.

	Each MovementSounds object allows you to include sounds for footsteps, jumping, and landing sounds that should be customized 
	to a particular terrain. You can add a new sound step, jump, or land sound for the object by clicking the plus sign under
	any array and dragging the desired AudioClip into it.
	
	Each object's variable can have multiple sounds that are randomly played, to make the soundscape more varied. 
	For example, you could have 3 different possible landing sounds when you land on rocks.  

	Currently, MovementSound objects and their associated sound files are stored under Assets/Sounds with 
	subfolders for sound types.

Suggestions and Hints
	If you are filming sounds for your own footsteps, make sure to keep those steps short! It's very easy to fall into the trap of walking
	slowly and then the audio files are too long

