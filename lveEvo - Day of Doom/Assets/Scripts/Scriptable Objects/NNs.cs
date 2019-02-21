using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Layer {
	
	public float InputValue;
	
	public float[] Weights;
	
	public float Bias;
	
}

public class NNs : MonoBehaviour {
	
	public Layer[] InputLayer;
	
	public Layer[] HiddenLayer;
	
	public Layer[] OutputLayer;
	
	public float CalculateAnim () {
		
		float Max = -1000000;
		int Anim = 0;
		
		for (int a = 0; a < OutputLayer.Length; ++a) {
			OutputLayer[a].InputValue = 0;
			for (int b = 0; b < HiddenLayer.Length; ++b) {
				HiddenLayer[b].InputValue = 0;
				for (int c = 0; c < InputLayer.Length; ++c) {
					HiddenLayer[b].InputValue += (InputLayer[c].InputValue*InputLayer[c].Weights[b]);
				}
				HiddenLayer[b].InputValue *= HiddenLayer[b].Bias;
				HiddenLayer[b].InputValue = Mathf.Clamp01(((HiddenLayer[b].InputValue/HiddenLayer.Length)+1)/2);
				
				OutputLayer[a].InputValue += (HiddenLayer[b].InputValue*HiddenLayer[b].Weights[a]);
			}
			OutputLayer[a].InputValue *= OutputLayer[a].Bias;
			OutputLayer[a].InputValue = Mathf.Clamp01(((OutputLayer[a].InputValue/OutputLayer.Length)+1)/2);
			
			if (OutputLayer[a].InputValue > Max) {
				Max = OutputLayer[a].InputValue;
				Anim = a;
			}
		}
		
		return (Anim);
	}
}
