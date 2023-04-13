// ---------------------------------------
// Spectrum Visualizer code by Bad Raccoon
// (C)opyRight 2017/2018 By :
// Bad Raccoon / Creepy Cat / Barking Dog 
// ---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Wolfey.Extensions;

public class SpectrumEmission : MonoBehaviour {
	public int audioChannel = 4;
	public float audioSensibility = 0.15f;
	public float intensity = 3.0f;
	public float lerpTime = 2.0f;

	public Material lt;
	[SerializeField] Color color;
	

	void Update () {

		// If i find the beat
		if (SpectrumKernel.spects [audioChannel] * SpectrumKernel.threshold >= audioSensibility)
		{
			var currentIntensity = SpectrumKernel.spects[audioChannel] * (intensity * SpectrumKernel.threshold);
			lt.SetEmissionColor(color * currentIntensity) ;
		}else{
			// Retrieve the old intensity
			var currentIntensity = Mathf.Lerp(lt.GetEmissionColor().Intensity(), 1f, lerpTime * Time.deltaTime);
			lt.SetEmissionColor(color * currentIntensity);
		}

	}
}
