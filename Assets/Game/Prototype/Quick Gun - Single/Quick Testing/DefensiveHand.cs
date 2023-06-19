using System;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Art_object{
	public class DefensiveHand : MonoBehaviour{
		/// <summary>
		/// priority
		/// - hit sound active
		/// - play hit particle
		/// - player hold grip can active and disable defensive hand
		/// </summary>
		[SerializeField] private AudioClip hitClip;

		[SerializeField] private GameObject hitVFX;

		private AudioSource _audioSource;

		private void Start(){
			_audioSource = gameObject.AddComponent<AudioSource>();
		}

		private void Update(){
			var leftHandGrip = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
		}

		private void OnTriggerEnter(Collider other){
			if(!other.TryGetComponent(out Projectile projectile)) return;
			_audioSource.PlayOneShot(hitClip);
			var attachPoint = other.ClosestPoint(transform.position);
			var vfxClone = Instantiate(hitVFX, attachPoint, Quaternion.identity);
			Destroy(projectile.gameObject);
			Destroy(vfxClone, 1.5f);
		}
	}
}