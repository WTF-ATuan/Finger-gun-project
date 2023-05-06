using Oculus.Interaction.HandGrab;
using UnityEngine;
using uPIe;

namespace Game.Prototype.Pistol{
	[RequireComponent(typeof(uPIeMenu))]
	public class PistolUIBinder : MonoBehaviour{
		private uPIeMenu _menu;
		[SerializeField] private GameObject grabbable;
		[SerializeField] private HandGrabInteractor interactor;

		private void Awake(){
			_menu = GetComponent<uPIeMenu>();
		}

		private void Start(){
			_menu.ControlWithGamepad = true;
			_menu.UseCustomInputSystem = true;
		}

		private void Update(){
			Picking();
			Selecting();
		}

		private void Selecting(){
			var selected = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
			if(!selected) return;
			var selectedId = _menu.SelectedPieceId;
			if(selectedId == 1){
				var grabObject = Instantiate(grabbable, transform.position, Quaternion.identity);
				var grabInteractable = grabObject.GetComponentInChildren<HandGrabInteractable>();
				interactor.ForceSelect(grabInteractable);
			}

			_menu.Deselect();
		}

		private void Picking(){
			var leftPadInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
			_menu.CustomInput = leftPadInput;
		}
	}
}