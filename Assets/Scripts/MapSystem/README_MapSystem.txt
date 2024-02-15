HOW TO USE THE MAP SYSTEM

The Minimap
	The minimap is an object in the ModernMap scene. There is a prefab of it in the prefab/environment folder.
	The UI itself requires no code at all. It uses a render texture from a camera parented to the player. The
	camera only renders the minimap layer, which is only the player icon and minimap image, so performace 
	should not be a major issue. The main camera does not render the minimap layer.

	The minimap image itself is a 1m:1px scale representation of the world, where 1 pixel in the image is equal
	to 1 meter (or 1 unit) in the game world. Please keep to this scaling when drawing a new map image. This 
	same image is also used in the full map view scene.

The Full Map View
	When in the game, the player presses M by default to open the FullMapView scene. They can use the mouse to
	move, zoom in, and select teleport waypoints around the map. The map image is 1:1 scale with the Modern Map,
	1 pixel = 1 unit.
	How to add new waypoints:
		1. Check that these things are in the scene (they are in FullMapView)
			- a gameobject named Teleport Info
			- a gameobject named position label that is the first child of Teleport Info
		2. Drag the WaypointObj prefab into the scene
		3. Abide by these rules when determining where to place the object
			- place the waypoint at the exact x and z coordinates that you want the waypoint to teleport to
			- keep the y position at 5 so it can be seen by the camera
			- set the desired y teleport value manually in the TeleportWaypoint component of that prefab instance.