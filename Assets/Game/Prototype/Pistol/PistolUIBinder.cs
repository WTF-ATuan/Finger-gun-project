using UnityEngine;
using uPIe;

namespace Game.Prototype.Pistol{
	[RequireComponent(typeof(uPIeMenu))]
	public class PistolUIBinder : MonoBehaviour{
		private uPIeMenu _menu;

		private void Awake(){
			_menu = GetComponent<uPIeMenu>();
		}

		private void Start(){
			_menu.ControlWithGamepad = true;
			_menu.UseCustomInputSystem = true;
		}

		private void Update(){
			var leftPadInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
			_menu.CustomInput = leftPadInput;
		}
	}
}