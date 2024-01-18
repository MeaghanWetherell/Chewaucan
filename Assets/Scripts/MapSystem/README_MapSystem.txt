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
	move and zoom in.