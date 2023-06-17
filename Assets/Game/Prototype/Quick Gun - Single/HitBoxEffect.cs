using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single{
	public class HitBoxEffect : MonoBehaviour{
		// Camera Red VFX 
		// Controller Vibration
		[SerializeField] private RawImage hitEffectImage;
		[SerializeField] private AudioClip hitSound;

		private AudioSource _audioSource;

		private void Start(){
			_audioSource = gameObject.AddComponent<AudioSource>();
		}

		private void OnTriggerEnter(Collider other){
			if(!other.TryGetComponent(out Projectile projectile)) return;
			CameraHitEffect();
			ControllerHitEffect();
			_audioSource.PlayOneShot(hitSound);
			Destroy(projectile.gameObject);
		}

		[Button]
		private void CameraHitEffect(){
			hitEffectImage.gameObject.SetActive(true);
			hitEffectImage.DOFade(1 , 0.5f)
					.OnComplete(() => {
						hitEffectImage.gameObject.SetActive(false);
						hitEffectImage.DOFade(0.1f, 0);
					});
		}

		[Button]
		private void ControllerHitEffect(){
			OVRInput.SetControllerVibration(1, 1f, OVRInput.Controller.RTouch);
			OVRInput.SetControllerVibration(1, 1f, OVRInput.Controller.LTouch);
			Invoke(nameof(StopHaptic), 0.35f);
		}

		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
		}
	}
}