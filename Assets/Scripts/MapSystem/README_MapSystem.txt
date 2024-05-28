HOW TO USE THE MAP SYSTEM

Creation Information:
	This tool was created by Ellie Martin in Spring 2024 for the game Finding Lake Chewaucan.
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.10f1
	This tool uses the TextMeshPro package (Unity Registry, Version 3.0.6)
	The TextMeshPro (aka TMPro) package also has dependencies. Their version information is as follows:
		Unity UI 1.0.0

General Description
	The map system is a user interface that tracks player movement within a scene and 
	allows fast travel in between locations (called Waypoints here).  It ties into the quest system and astrolabe.
	It consists of several prefab game objects with attached scripts, Minimap and Fullmap.

In-Game Mechanics
	The minimap initializes in the upper left-hand corner of the screen when the game starts. 
	To access the full map view (which is a bigger version of the minimap), the player presses M (by default). 
	They can then use the mouse to move, zoom in, and select teleport waypoints around the map.


The Minimap
	The minimap is a game object in the ModernMap scene. There is a prefab of it in the assets/prefab/environment/ 
	folder. The prefab has the Map UI Controller Script, a canvas, canvas  scaler, and graphic raycaster component attached.
	The only code associated with the minimap is MapUIController, which enabled/disables the minimap when the 0 key (by default) 
	is pressed and also controls switching to the full map view described above. There is also one line of code (the first line 
	of the update function) in both SwimmingMovement and LandMovement attached to the Player prefab, which keeps the rotation of 
	the minimap camera (a child of the gameobject Player named MinimapCamera) static.
	To save performance space, instead of using a 1:1 representation of the world, the construction of the Minimap 
	UI uses a pre-made render texture from a camera parented to the player called "Modern_Map_Detailed.png" 
	This image can be found in the folder Assets/Scripts/MapSystem/NewMapSystem
	The MinimapCamera customizes its culling mask such that it will only render gameobject on the layer named Minimap.
	The layer Minimap only contains the following objects:
		PlayerArrow, a child of the player
		PlayerViewCone, a child of the player
		MapImage, a gameobject in the ModernMap scene containing the png image.
	The MainCamera's culling mask is set to render all layers except Minimap. For more information on layers, refer
	to the documentation: https://docs.unity3d.com/Manual/use-layers.html

The Full Map View
	FullMapView is a separate unity scene, located in the folder Assets/Scenes
	How to add new waypoints:
		1. Check that these things are in the scene (they should already be present)
			- a gameobject named Teleport Info (it's a child of the canvas)
			- a gameobject named position label that is the first child of Teleport Info
		2. Drag the WaypointObj prefab into the scene
		3. Abide by these rules when determining where to place the object
			- place the waypoint at the exact x and z coordinates that you want the waypoint to teleport to
			- keep the y position at 5 so it can be seen by the camera
			- set the desired y teleport value manually in the TeleportWaypoint component of that prefab instance.
		4. Decide which map (Modern or Pleistocene) the new waypoint should teleport to, then do this:
			- in the hierarchy, drag the new waypoint object so that is is a child of the [Map Name] Map Waypoints object,
			  where [Map Name] is either Modern or Pleistocene. So if you want a new waypoint to teleport to the modern map,
			  drag the waypoint so that it is a child of the Modern Map Waypoints object. This makes it so it gets disabled
			  when switching the map view.
	To update the map image
		If a new map is drawn according to the instructions in "Drawing a new Map Image" below, then you can set that image
		as the new one in the object [Map Name] Map Image, which is a child of the object [Map Name] Map View, with
		[Map Name] being either Modern or Pleistocene. Simply change the sprite in the sprite renderer to the desired image.

Drawing a new Map Image
	The minimap image itself is a 1m:1px scale representation of the world, where 1 pixel in the image is equal
	to 1 meter (or 1 unit) in the game world. Please keep to this scaling when drawing a new map image . This 
	same image is also used in the full map view scene.
	When the new image is put into unity (preferably as a png), ensure that the png is set a a sprite and that the 
	pixels per unit is set to 1 and also that the pivot is set to the bottom left. All other settings remain default.