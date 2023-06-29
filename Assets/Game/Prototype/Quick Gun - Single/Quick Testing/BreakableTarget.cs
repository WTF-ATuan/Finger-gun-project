using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single{
	public class BreakableTarget : MonoBehaviour{
		[SerializeField] private int destroyCount = 1;
		private int _hitCount;
		[SerializeField] private GameObject brokenVFX;


		public void OnCollisionEnter(Collision collision){
			if(!collision.gameObject.TryGetComponent(out Projectile projectile)) return;
			_hitCount++;
			if(_hitCount >= destroyCount){
				EventAggregator.Publish(new TargetHitEvent());
				transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
				if(!brokenVFX) return;
				var vfxClone = Instantiate(brokenVFX, transform.position, Quaternion.identity);
				Destroy(vfxClone, 1.5f);
			}
		}
	}

	public class TargetHitEvent : EventArgs{
		public int HitCount;
	}
}