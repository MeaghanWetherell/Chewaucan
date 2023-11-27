using UnityEngine;
using System.Collections;

namespace CTI {

	[RequireComponent (typeof (WindZone))]
	public class CtiCustomWind : MonoBehaviour {

		private WindZone _mWindZone;

		private Vector3 _windDirection;
		private float _windStrength;
		private float _windTurbulence;

	    public float windMultiplier = 1.0f;

	    private bool _init = false;
	    private int _terrainLODWindPid;

	    void Init () {
			_mWindZone = GetComponent<WindZone>();
			_terrainLODWindPid = Shader.PropertyToID("_TerrainLODWind");
		}

		void OnValidate () {
			Update ();
		}
		
		void Update () {
			if (!_init) {
				Init ();
			}
			_windDirection = this.transform.forward;

			if(_mWindZone == null) {
				_mWindZone = GetComponent<WindZone>();
			}
			_windStrength = _mWindZone.windMain * windMultiplier;
			_windStrength += _mWindZone.windPulseMagnitude * (1.0f + Mathf.Sin(Time.time * _mWindZone.windPulseFrequency) + 1.0f + Mathf.Sin(Time.time * _mWindZone.windPulseFrequency * 3.0f) ) * 0.5f;
			_windTurbulence = _mWindZone.windTurbulence * _mWindZone.windMain * windMultiplier;

			_windDirection.x *= _windStrength;
			_windDirection.y *= _windStrength;
			_windDirection.z *= _windStrength;

			Shader.SetGlobalVector(_terrainLODWindPid, new Vector4(_windDirection.x, _windDirection.y, _windDirection.z, _windTurbulence) );
		}
	}
}