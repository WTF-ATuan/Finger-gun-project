using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class PistolBinder : MonoBehaviour{
		private Weapon _bindingWeapon;
		private PistolRecoil _recoil;
		private AudioSource _audioPlayer;
		[SerializeField] private AudioClip fireClip;
		[SerializeField] private bool isRight = true;


		// 設置震動參數
		[SerializeField] private AudioClip vibrationClip;
		private const float Amplitude = 1.0f;
		private const float Frequency = 300.0f;

		private void Start(){
			_bindingWeapon = GetComponent<Weapon>();
			_recoil = GetComponent<PistolRecoil>();
			_audioPlayer = gameObject.AddComponent<AudioSource>();
		}

		private void Update(){
			var openingFire =
					OVRInput.GetDown(isRight
							? OVRInput.Button.SecondaryIndexTrigger
							: OVRInput.Button.PrimaryIndexTrigger);
			if(openingFire){
				_bindingWeapon.Fire();
				_recoil.Recoil();
				_audioPlayer.PlayOneShot(fireClip);
				SimpleHaptic();
			}

			var reloading = OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
			if(reloading){
				_bindingWeapon.Reload();
			}
		}

		private void ClipHaptic(){
			var hapticsClip = new OVRHapticsClip(vibrationClip, 10);
			var rightChannel = OVRHaptics.RightChannel;
			rightChannel.Preempt(hapticsClip);
		}

		private void SimpleHaptic(){
			if(isRight){
				OVRInput.SetControllerVibration(Frequency, Amplitude, OVRInput.Controller.RTouch);
				Invoke(nameof(StopHaptic), 0.15f);
			}
			else{
				OVRInput.SetControllerVibration(Frequency, Amplitude, OVRInput.Controller.LTouch);
				Invoke(nameof(StopHaptic), 0.15f);
			}
		}

		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
		}
	}
}