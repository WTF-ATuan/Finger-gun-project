﻿using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
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

		[SerializeField] private GameObject shield;
		[SerializeField] private GameObject emitter;

		private AudioSource _audioSource;

		private Vector3 _shieldOriginScale;

		private void Start(){
			_audioSource = gameObject.AddComponent<AudioSource>();
			shield.OnTriggerExitAsObservable().Subscribe(OnShieldHit);
			shield.OnTriggerStayAsObservable().Subscribe(OnShieldHitStay);
			_shieldOriginScale = shield.transform.localScale;
		}

		private void OnShieldHitStay(Collider obj){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)){
				DOTween.Kill(obj.transform);
				obj.transform.position = shield.transform.position;
				obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
				obj.GetComponent<Rigidbody>().useGravity = false;
			}
		}

		private void Update(){
			ActiveShieldByGrip();
		}

		private void ActiveShieldByGrip(){
			var leftHandGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
			var shieldMinScale = _shieldOriginScale * 0.2f;
			var lerpShieldScale = Vector3.Lerp(shieldMinScale, _shieldOriginScale, leftHandGrip);
			shield.transform.localScale = lerpShieldScale;
		}

		private void OnShieldHit(Collider other){
			if(!other.TryGetComponent(out Projectile projectile)) return;
			_audioSource.PlayOneShot(hitClip);
			var attachPoint = other.ClosestPoint(shield.transform.position);
			var vfxClone = Instantiate(hitVFX, attachPoint, Quaternion.identity);
			Destroy(vfxClone, 1.5f);
		}
	}
}