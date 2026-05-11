README ON HOW TO USE THE 3D PLATFORMER RADIOCARBON DATING GAME

Creation Information:
	This tool was created by Charlie Scott in Fall 2025 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.62f3
	This tool uses the TextMeshPro package (Unity Registry, Version 3.0.7) https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/license/LICENSE.html

General Description:
	This game involves jumping on 3D models of rocks to sample them for radiocarbon ages. Rocks that are of the wrong type
	or are not on the correct plateau will give incorrect dates. There are also rattlesnakes that will attack you and
	tumbleweeds that will slow you down. There are scripts to make objects climbable in the Movement folder.

	There are several scripts directly associated with this system. They rely on some other scripts and systems.
		CourseManager.cs (LoadGuiManager.cs, Player.cs, HUD.cs)
		CourseTimer.cs (PauseCallback.cs)
		CourseWall.cs (CourseManager.cs)
		DateRock.cs (CourseManager.cs, SoundManager.cs)
		LakeLevelData.cs
		PlateauQuestManager.cs (QuestManager.cs, CourseManager.cs)
		Rock.cs
		ScaleRandomizer.cs
		Snake.cs
		SnakeKill.cs (CourseManager.cs, Player.cs)
		SnakeMove.cs(PauseCallback.cs)
		SnakeRotate.cs (SoundManager.cs, Player.cs)
		SnakeSlow.cs (LandMovement.cs, Player.cs, SoundManager.cs, HUDManager.cs)
		StartCourseOnTriggerEnter.cs (CourseManager.cs, QuestManager.cs, Player.cs)
		Tumbleweed.cs (PauseCallback.cs, HUDManager.cs, Player.cs)


Prefabs
	All necessary prefabs are under prefabs/dating. These include rock variants, coursewalls, and snakes.

To Make a New Level
	1. add the coursemanager prefab near the desired course location. use this as a folder for the other components.
		adjust the exposed number parameters as desired, give the manager a unique Level ID
	2. add a start prefab under the coursemanager at the desired start location. adjust the text on the sign as desired.
		Optional: add a DatingHelpInteractable prefab in front to allow the user to access the help menu. 
		add an empty object representing the position the player should be reset to on death and assign 
			it to the start position field of the coursemanager
	3. add coursewalls under the coursemanager, setting rotations, positions, and scales as desired to fence in the course area
	4. create empty folder objects for the spawn locations of the date rocks (carbonate and tuffa) and snakes. 
		optionally, also create one for rock obstacles. assign these to the appropriate transform fields on the coursemanager
	5. fill the folders with empty gameobjects. the location of these gameobjects indicates a valid spawn location
		for the corresponding object to the manager, which will then randomly spawn the objects each game at some of these locations 
		optionally, use cubes instead of empty objects; they will be disabled automatically on start 
			(note that the spawn location is the center of the cube)
	6. add any other desired obstacles
	7. bake nav mesh


