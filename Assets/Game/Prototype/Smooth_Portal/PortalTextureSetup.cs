using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Prototype.Smooth_Portal{
	public class PortalTextureSetup : MonoBehaviour{
		public Camera cameraB;
		public Material cameraMatB;

		private void Start(){
			if(cameraB.targetTexture != null){
				cameraB.targetTexture.Release();
			}

			cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			cameraMatB.mainTexture = cameraB.targetTexture;
		}
	}
}