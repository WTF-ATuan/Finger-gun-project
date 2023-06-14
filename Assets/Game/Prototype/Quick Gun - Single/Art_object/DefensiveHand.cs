using System;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Art_object{
	public class DefensiveHand : MonoBehaviour{
		private void Update(){
			var leftHandGrip = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
			if(leftHandGrip) ActiveDefensiveHand();
			else DisableDefensiveHand();
		}

		private void ActiveDefensiveHand(){ }

		private void DisableDefensiveHand(){ }
	}
}