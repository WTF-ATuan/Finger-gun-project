using System;
using Oculus.Interaction.Input;
using UnityEngine;

namespace Game.Prototype{
	public class InputEventHandler : MonoBehaviour{
		[SerializeField] private Controller rightController;

		private void Start(){
			if(rightController == null) throw new NullReferenceException("Controller is null");
		}

	}
}