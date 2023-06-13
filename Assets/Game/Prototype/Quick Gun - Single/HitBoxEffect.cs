using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single{
	public class HitBoxEffect : MonoBehaviour{
		// Camera Red VFX 
		// Controller Vibration
		[SerializeField] private Image hitEffectImage;
		private void OnTriggerEnter(Collider other){
			if(!other.TryGetComponent(out Projectile projectile)) return;
			CameraHitEffect();
			ControllerHitEffect();
			Destroy(projectile.gameObject);
		}
		[Button]
		private void CameraHitEffect(){
			hitEffectImage.gameObject.SetActive(true);
			hitEffectImage.transform.DOScale(0.45f, 0.4f)
					.OnComplete(() => {
						hitEffectImage.gameObject.SetActive(false);
						hitEffectImage.transform.localScale = Vector3.one * 0.15f;
					});
		}
		[Button]
		private void ControllerHitEffect(){
			OVRInput.SetControllerVibration(1, 1f, OVRInput.Controller.RTouch);
			OVRInput.SetControllerVibration(1, 1f, OVRInput.Controller.LTouch);
			Invoke(nameof(StopHaptic) , 0.35f);
		}
		private void StopHaptic(){
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
		}
	}
}