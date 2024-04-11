README FOR CUSTOM SPLINE TOOL FOR ALIGNING A SPLINE TO TERRAIN
The script can be found in Assets/Scripts/MapSystem/SplineFences/SplineAlignToTerrain.cs

Creation Information:
	This tool was created by Ellie Martin in Spring 2024 for the game Finding Lake Chewaucan,
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.10f1
	This tool uses the Splines package (Unity Registry, Version 2.4.0) https://docs.unity3d.com/Packages/com.unity.splines@2.4/manual/index.html 
	The Splines package also has dependencies. Their version information is as follows:
		Settings Manager 2.0.1
		Mathematics 1.2.6
		Unity UI 1.0.0


Basic Description:
	This tool is meant to align the knots of a spline to the surface of the terrain underneath it, 
	making linear placement of prefabs over uneven game terrain objects less of a hassle. 
	This tool does not work if there is no terrain object underneath the spline. 
	This tool meant to function as you add knots to a spline, it does not split the
	spline up all at once. If you plan on using this, add it as a component early.
	If the terrain is super flat or has a smooth, consistent slope, using this tool is not necessary.


How To Set Up The Custom Spline:
	We reccommend using this with the draw spline tool when adding splines to the scene, 
	as this tool has not been tested with default splines of other shapes (circle, square, etc.).
	1. Create a spline using the Splines package tool:
		Go to the scene hierarchy and select the dropdown add objects menu (this is accessed 
		using the plus sign at the upper left corner   of the Hierarchy list), 
		Select Splines and then the Draw Splines Tool. This will open the default splines tool 
		from the Splines package, indicated by a change in icons on the scene window and your 
		mouse will turn into a knot marker.	
		Click anywhere on the terrain in the scene to initialize the first knot. 
		We reccommendrecommend you add about 2-3 knots before adding the custom splines-to-terrain component.
	2. Add the custom spline align component.
		Select the spline object you just created in the hierarchy.
		When it opens in the inspector, click Add Component
		Search for the "SplineAlignToTerrain" script component and add it.
	3. Align the Spline to Terrain
		In the custom Spline Align to Terrain script there are several variables defined below that you may adjust.
		Every time you place a new knot, the tool will automatically split the distance from the previously added knot
		to the new knot into multiple knots that align with the surface of the terrain. Errors that may occur during this
		process are described below in troubleshooting.
		Once your spline is in the correct place, with the correct number of knots, press the "align to terrain" button
		This will settle the knots to the terrain. (This button may also be presed at any time to realign the knots to 
		the terrain in the event that individual knots or the spline as a whole is moved)
	4. Add the Spline Instantiate component from the Splines package
		This component allows you to add prefabs and game objects to the spline.
		Further information is available here: https://docs.unity3d.com/Packages/com.unity.splines@2.1/manual/instantiate-component.html 

Spline Align To Terrain Inspector Variables:
	Knot Interval -- integer that represents the distance between knots when aligning them to the 
					 terrain. The smaller this value, the more accurate the knot alignment.
					 You can modify this value at anytime when adding knots, and you should adjust
					 it depending on how highly variable the terrain height is.
	
	Spline to Align -- this integer represents which spline in the Spline container you are working with
					   Each spline game object (the component labeled Spline) may have multiple splines (see SplineAlignToTerrainReadme1.jpg).
					   This integer refers to which spline you are aligning to the terrain. 
					   However, we do not recommend having more than one spline per game object. 
					   The functionality for an object with multiple splines has not been fully tested and 
					   this tool can add a lot of knots very quickly. If you do use multiple splines in a single Spline object,
					   make sure this integer corresponds to an existing spline in Spline component (ex. 0 means Spline 0)
	
	Y Allowance -- this float represents the allowable difference in knot position to terrain ground height in in-game units.
				   This is meant to reduce the amount of redundant knots added if the terrain does not vary in height very much. 
				   When the terrain is highly variable at a small scale, this number should be smaller.
				   When terrain is more consistent in slope, this number can be larger and reduce the number of needed knots. 


Troubleshooting While Using The Tool:
	There are 2 main errors you might encounter while using the tool:
		nullReferenceException...SplineCacheUtility...
				This may happen sometimes when adding a knot. It is an error from the spline package.
				Use undo to remove the knot, then try adding again. This may take a few attempts.
		"Look rotation viewing vector is zero"
				This displays as a normal console message, and it may sometimes spam your console when 
				using this tool. We are not sure what causes it, but when it does happen, stop adding 
				knots and undo to remove at least once. 

Additional Links and Information
	For issues with creating splines using the Splines package, see:  
		https://docs.unity3d.com/Packages/com.unity.splines@2.4/manual/create-a-spline.html 
	For more information on using Splines Instantiate, see: 
		https://docs.unity3d.com/Packages/com.unity.splines@2.1/manual/instantiate-component.html 
