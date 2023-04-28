using System.Collections;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class PistolBinder : MonoBehaviour{
		private Weapon _bindingWeapon;


		// 設置震動參數
		private const float Amplitude = 1.0f;
		private const float Duration = 0.3f;
		private const float Frequency = 300.0f;

		private void Start(){
			_bindingWeapon = GetComponent<Weapon>();
		}

		private void Update(){
			var openingFire = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
			if(openingFire){
				_bindingWeapon.Fire();
				ClipHaptic();
			}

			var reloading = OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
			if(reloading){
				_bindingWeapon.Reload();
			}
		}

		private void ClipHaptic(){
			var hapticsClip = new OVRHapticsClip();

			// 設置震動數據
			var cnt = (int)(Duration * OVRHaptics.Config.SampleRateHz);
			for(var i = 0; i < cnt; i++){
				var time = Mathf.PI * 2.0f * Frequency * (i / (float)OVRHaptics.Config.SampleRateHz);
				var sample = Amplitude * Mathf.Sin(time);
				hapticsClip.WriteSample((byte)Mathf.RoundToInt(sample * 127 + 128));
			}

			var rightChannel = OVRHaptics.RightChannel;
			rightChannel.Preempt(hapticsClip);
		}

		private void SimpleHaptic(){
			OVRInput.SetControllerVibration(Frequency, Amplitude, OVRInput.Controller.RTouch);
		}
		
	}
}