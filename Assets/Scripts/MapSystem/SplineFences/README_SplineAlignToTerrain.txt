README FOR CUSTOM SPLINE TOOL FOR ALIGNING A SPLINE TO TERRAIN

Basic Description:
	This tool is meant to align the knots of a spline to the terrain underneath it, making placement
	over mountainous terrain less of a hassle, with terrain meaning the actual unity terrain component. 
	This tool does not work if there is no terrain object underneath the spline. 
	This tool meant to function as you add knots to a spline, it does not split the
	spline up all at once. If you plan on using this, add it as a component early.
	It the terrain is super flat, using this tool is not necessary.

How to set it up:
	I reccommend using this with the draw spline tool when adding splines to the scene.
	Go to the scene hierarchy, click the + to add an object, and go to Spline -> draw splines tool.
	Click anywhere on the terrain in the scene to initialize the first knot. 
	I reccommend you add about 2-3 knots before adding this component.
	After you have the first few knots added, select the spline object in the hierarchy and in the
	inspector, click add component and search for "SplineAlignToTerrain" and add it.

How to use the inspector variables:
	Knot Interval -- this integer represents how far apart to add knots when aligning them to the 
					 terrain. The smaller this value, the more accurate the knots will be aligned.
					 You can modify this value at anytime when adding knots, and you should adjust
					 it depending on how mountainous the terrain is.
	
	Spline to Align -- this integer represents which spline in the Spline container you are working with
					   as a single spline game object may have multiple splines in its container (the component
					   labeled Spline). I reccommend having only 1 spline per game object, as the functionality 
					   for an object with multiple splines has not been fully tested, and a lot of knots are
					   added very quickly. Make sure this integer corresponds to an existing spline in Spline
					   component (ex. 0 means Spline 0)
	
	Y Allowance -- this float represents the allowable difference in knot position to terrain ground height.
				   This is meant to reduce the amount of redundant knots added if the terrain does not vary
				   in height very much.


Troubleshooting while using the tool:
	There are 2 main errors you might encounter while using the tool:
		nullReferenceException...SplineCacheUtility...
				This may happen sometimes when adding a knot. It is not the fault of the script (most likely)
				but rather the spline package itself. If this does happen, simply Ctrl+Z and try adding the
				knot again. It may take a few tries.
		""Look rotation viewing vector is zero"
				This isn't really an error, but it may spam your console log when using this tool. I am not sure
				what causes it, but when it does happen, stop adding knots and press Ctrl+Z at least once. 