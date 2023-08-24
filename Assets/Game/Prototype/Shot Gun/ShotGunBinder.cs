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
		public int ammoMax = 3;

		[TitleGroup("Setting")] [SerializeField]
		private Transform[] muzzles;

		[TitleGroup("Setting")] [SerializeField]
		private bool isRight = true;

		[TitleGroup("Setting")] [SerializeField]
		private Vector2 reloadAngleRange = new(40, 70);


		[TitleGroup("View")] [SerializeField] private AudioClip fireClip;
		[TitleGroup("View")] [SerializeField] private AudioClip emptyClip;
		[TitleGroup("View")] [SerializeField] private AudioClip reloadClip;
		[TitleGroup("View")] [SerializeField] private GameObject fireVFX;
		[TitleGroup("View")] [SerializeField] private TMP_Text ammoCountText;
		[TitleGroup("View")] [SerializeField] private Vector3 reloadingRotateDirection = Vector3.forward;


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
					OVRInput.GetDown(isRight
							? OVRInput.Button.SecondaryIndexTrigger
							: OVRInput.Button.PrimaryIndexTrigger);
			if(openingFire){
				Fire();
			}

			if(transform.eulerAngles.z > reloadAngleRange.x && transform.eulerAngles.z < reloadAngleRange.y &&
			   _currentAmmo < ammoMax){
				Reload();
			}
		}

		private void Fire(){
			if(_currentAmmo < 1){
				_audioPlayer.PlayOneShot(emptyClip);
				return;
			}
			// 換彈中不能射擊
			if(!_recoil.enabled) return;
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
			_recoil.enabled = false;
			var targetAngle = transform.eulerAngles;
			targetAngle += reloadingRotateDirection * 360;
			var calculateReloadingTime = CalculateReloadingTime();
			ModifyCurrentAmmo(ammoMax);
			transform.DORotate(targetAngle, calculateReloadingTime, RotateMode.FastBeyond360)
					.OnComplete(() => { _recoil.enabled = true; });
			_audioPlayer.PlayOneShot(reloadClip);
		}

		private float CalculateReloadingTime(){
			var ammoPercent = Mathf.Clamp01((float)_currentAmmo / ammoMax);
			return ammoPercent > 0.8f ? 0.2f : Mathf.Lerp(0.75f, 0.2f, ammoPercent);
		}

		public void ModifyCurrentAmmo(int amount){
			_currentAmmo = Mathf.Clamp(amount, 0, ammoMax);
			ammoCountText.text = _currentAmmo.ToString();
		}

		private void SimpleHaptic(){
			OVRInput.SetControllerVibration(500, 0.9f,
				isRight ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
			Invoke(nameof(StopHaptic), 0.2f);
		}

		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, isRight ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
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