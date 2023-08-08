using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Prototype.Pistol{
	public class ShotGunBinder : MonoBehaviour{
		[TitleGroup("Setting")] [SerializeField]
		private GameObject projectile;

		[TitleGroup("Setting")] [SerializeField]
		private float projectileImpulse = 300;

		[TitleGroup("Setting")] [SerializeField]
		private int ammoMax = 3;

		[TitleGroup("Setting")] [SerializeField]
		private Transform[] muzzles;

		[TitleGroup("View")] [SerializeField] private AudioClip fireClip;
		[TitleGroup("View")] [SerializeField] private AudioClip emptyClip;
		[TitleGroup("View")] [SerializeField] private AudioClip reloadClip;
		[TitleGroup("View")] [SerializeField] private GameObject fireVFX;
		[TitleGroup("View")] [SerializeField] private TMP_Text ammoCountText;


		private PistolRecoil _recoil;
		private AudioSource _audioPlayer;
		private int _currentAmmo;


		private void Start(){
			_recoil = GetComponent<PistolRecoil>();
			_audioPlayer = gameObject.AddComponent<AudioSource>();
			ModifyCurrentAmmo(ammoMax);
		}

		private void Update(){
			var openingFire =
					OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
			if(openingFire){
				Fire();
			}

			if(transform.eulerAngles.z is > 40 and < 70 && _currentAmmo < ammoMax){
				Reload();
			}
		}

		private void Fire(){
			if(_currentAmmo < 1){
				_audioPlayer.PlayOneShot(emptyClip);
				return;
			}

			ModifyCurrentAmmo(_currentAmmo - 1);
			foreach(var muzzle in muzzles){
				var bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
				bullet.GetComponent<Rigidbody>().velocity = muzzle.forward * projectileImpulse;
			}

			var vfxClone = Instantiate(fireVFX, muzzles[0].position, muzzles[0].rotation);
			Destroy(vfxClone, 1.5f);
			_recoil.Recoil();
			_audioPlayer.PlayOneShot(fireClip);
			SimpleHaptic();
		}

		[Button]
		private void Reload(){
			ModifyCurrentAmmo(ammoMax);
			_recoil.enabled = false;
			var targetAngle = transform.eulerAngles;
			targetAngle.z += 360;
			transform.DORotate(targetAngle, 0.3f, RotateMode.FastBeyond360)
					.OnComplete(() => { _recoil.enabled = true; });
			_audioPlayer.PlayOneShot(reloadClip);
		}

		private void ModifyCurrentAmmo(int amount){
			_currentAmmo = Mathf.Clamp(amount, 0, ammoMax);
			ammoCountText.text = _currentAmmo.ToString();
		}

		private void SimpleHaptic(){
			OVRInput.SetControllerVibration(500, 0.9f, OVRInput.Controller.RTouch);
			Invoke(nameof(StopHaptic), 0.2f);
		}

		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
		}

		private void OnDrawGizmos(){
			if(muzzles.Length < 1) return;
			foreach(var muzzle in muzzles){
				Gizmos.color = Color.green;
				Gizmos.DrawRay(muzzle.position, muzzle.forward);
			}
		}
	}
}