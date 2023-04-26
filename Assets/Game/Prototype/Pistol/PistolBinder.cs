using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class PistolBinder : MonoBehaviour{
		private Weapon _bindingWeapon;

		private void Start(){
			_bindingWeapon = GetComponent<Weapon>();
		}

		private void Update(){
			var openingFire = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.RHand);
			if(openingFire){
				_bindingWeapon.Fire();
			}
		}
	}
}