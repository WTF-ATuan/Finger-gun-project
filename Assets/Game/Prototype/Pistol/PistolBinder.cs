using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class PistolBinder : MonoBehaviour{
		private Weapon _bindingWeapon;
		private PistolRecoil _recoil;
		private AudioSource _audioPlayer;
		[SerializeField] private AudioClip fireClip;


		// 設置震動參數
		[SerializeField] private AudioClip vibrationClip;
		private const float Amplitude = 1.0f;
		private const float Duration = 0.3f;
		private const float Frequency = 300.0f;

		private void Start(){
			_bindingWeapon = GetComponent<Weapon>();
			_recoil = GetComponent<PistolRecoil>();
			_audioPlayer = gameObject.AddComponent<AudioSource>();
		}

		private void Update(){
			var openingFire = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
			if(openingFire){
				_bindingWeapon.Fire();
				_recoil.Recoil();
				_audioPlayer.PlayOneShot(fireClip);
				ClipHaptic();
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
			OVRInput.SetControllerVibration(Frequency, Amplitude, OVRInput.Controller.RTouch);
		}

		private void LocationHaptic(){
			OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Index, Frequency, Amplitude,
				OVRInput.Controller.RTouch);
		}

		private void AmplitudeEnvelopeHaptic(){
			var envelopeVibration = new OVRInput.HapticsAmplitudeEnvelopeVibration{
				Duration = Duration
			};
			OVRInput.SetControllerHapticsAmplitudeEnvelope(envelopeVibration);
		}

		private void HapticPCM(){
			var hapticsPcmVibration = new OVRPlugin.HapticsPcmVibration();
			OVRPlugin.SetControllerHapticsPcm(OVRPlugin.Controller.RTouch, hapticsPcmVibration);
		}
	}
}