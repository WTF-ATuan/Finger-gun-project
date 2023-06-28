using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Iron_Boss{
	public class TeleportButton : MonoBehaviour{
		[SerializeField] [Required] private Transform player;

		[SerializeField] [Required] private Transform teleportTarget;

		[SerializeField] private float duration = 2.5f;
		
		private void OnCollisionEnter(Collision collision){
			if(!collision.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			transform.DOShakeScale(0.2f);
			player.transform.DOMove(teleportTarget.position, duration)
					.SetEase(Ease.InOutCubic);

		}
	}
}