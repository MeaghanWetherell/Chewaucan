using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTI {
	
	public static class CtiUtils {

	
	//	Function to adjust the translucent lighting fade according to the given shadow distance – or any othe distance that is passed in
	//	@params:
	//	(float) TranslucentLightingRange	: Range in which translucent lighting will be applied – most likely the shadow distance as set in Quality Settings
	//	(float) FadeLengthFactor			: Lenth relative to TranslucentLightingRange over which the fade will take place (0.0 - 1.0 range)

		public static void SetTranslucentLightingFade(float translucentLightingRange, float fadeLengthFactor) {
			translucentLightingRange *= 0.9f; // Add some padding as real time shadows fade out as well
			var fadeLength = translucentLightingRange * fadeLengthFactor;
		//	Pleae note: We use sqr distances here!
			Shader.SetGlobalVector ("_CTI_TransFade", 
				new Vector2( 
					translucentLightingRange * translucentLightingRange,
					fadeLength * fadeLength * ( (translucentLightingRange / fadeLength) * 2.0f )
				)
			);
		}

	}
}
