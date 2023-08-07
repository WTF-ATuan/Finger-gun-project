using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class ShotGunBinder : MonoBehaviour{
		[SerializeField] private GameObject projectile;
		[SerializeField] private GameObject fireVFX;
		[SerializeField] private int ammoMax = 3;
		private PistolRecoil _recoil;
		[SerializeField] private AudioClip fireClip;
		[SerializeField] private AudioClip vibrateClip;
		private AudioSource _audioPlayer;
		private int _currentAmmo;

		private void Start(){
			_recoil = GetComponent<PistolRecoil>();
			_audioPlayer = gameObject.AddComponent<AudioSource>();
			_currentAmmo = ammoMax;
		}

		private void Update(){
			var openingFire =
					OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
			if(openingFire){
				Fire();
				_recoil.Recoil();
				_audioPlayer.PlayOneShot(fireClip);
				SimpleHaptic();
			}
			
			if(transform.eulerAngles.z is > 45 and < 75 && _currentAmmo < ammoMax){
				Reload();
			}
		}

		private void Fire(){
			_currentAmmo--;
		}

		[Button]
		private void Reload(){
			_currentAmmo = ammoMax;
			_recoil.enabled = false;
			var targetAngle = transform.eulerAngles;
			targetAngle.z += 360;
			transform.DORotate(targetAngle, 0.3f, RotateMode.FastBeyond360).OnComplete(() => {
				_recoil.enabled = true;
			});
		}

		private void SimpleHaptic(){
			OVRInput.SetControllerVibration(100, 1, OVRInput.Controller.RTouch);
			Invoke(nameof(StopHaptic), 0.15f);
		}

		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
		}
	}
}