using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single{
	public class DoorGate : MonoBehaviour{
		[SerializeField] private GameObject leftGate;
		[SerializeField] private GameObject rightGate;
		[SerializeField] private float duration = 1;

		private void OnEnable(){
			OpenGate();
		}

		private void OnDisable(){
			CloseGate();
		}

		public void OpenGate(){
			leftGate.transform.DOMoveX(leftGate.transform.position.x +5, duration);
			rightGate.transform.DOMoveX(rightGate.transform.position.x - 5, duration);
		}

		public void CloseGate(){
			leftGate.transform.DOMoveX(leftGate.transform.position.x - 5, duration);
			rightGate.transform.DOMoveX(rightGate.transform.position.x + 5, duration);
		}
	}
}