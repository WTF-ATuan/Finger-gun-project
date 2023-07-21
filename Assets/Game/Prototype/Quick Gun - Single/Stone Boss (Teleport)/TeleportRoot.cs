using System;
using System.Collections.Generic;
using DG.Tweening;
using InfimaGames.LowPolyShooterPack;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class TeleportRoot : MonoBehaviour{
		public Transform[] teleportTriggers = new Transform[4];
		[Required] public Transform playerTransform;
		public float teleportDuration = 0.5f;

		private void Start(){
			teleportTriggers.ForEach(trigger =>
					trigger.OnCollisionEnterAsObservable()
							.Subscribe(_ => {
								var closePoint = FindClosePoint(trigger);
								teleportTriggers.ForEach(
									teleport => teleport.GetComponent<Collider>().enabled = true);
								trigger.GetComponent<Collider>().enabled = false;
								Teleport(closePoint.position);
							}));
		}

		private Transform FindClosePoint(Transform trigger){
			var closestDistance = Mathf.Infinity;
			Transform closestPoint = null;
			foreach(var teleportPoint in teleportTriggers){
				var distance = Vector3.Distance(trigger.position, teleportPoint.position);
				if(!(distance < closestDistance)) continue;
				closestDistance = distance;
				closestPoint = teleportPoint;
			}

			return closestPoint;
		}

		private void Teleport(Vector3 teleportPosition){
			teleportPosition.y = playerTransform.position.y;
			playerTransform.DOMove(teleportPosition, teleportDuration)
					.SetEase(Ease.OutCubic);
		}

		[Button]
		private void Test(Transform trigger){
			var closePoint = FindClosePoint(trigger);
			Teleport(closePoint.position);
			trigger.DOShakeRotation(teleportDuration / 2);
		}
	}
}