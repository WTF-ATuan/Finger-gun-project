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
		public List<Transform> teleportPointList;
		public Transform[] teleportTriggers = new Transform[4];
		[Required] public Transform playerTransform;
		public float teleportDuration = 0.5f;
		
		private void Start(){
			teleportTriggers.ForEach(x =>
					x.OnCollisionEnterAsObservable()
							.Subscribe(_ => {
								var direction = (x.transform.position - playerTransform.position).normalized;
								Teleport(direction);
								x.DOShakePosition(0.5f);
							}));
		}

		private void Teleport(Vector3 direction){
			var playerPosition = playerTransform.position + direction * 7.5f;
			Transform closestPoint = null;
			var closestDistance = Mathf.Infinity;

			foreach(var teleportPoint in teleportPointList){
				var distance = Vector3.Distance(playerPosition, teleportPoint.position);
				if(!(distance < closestDistance)) continue;
				closestDistance = distance;
				closestPoint = teleportPoint;
			}

			if(closestPoint == null) return;
			var teleportPosition = closestPoint.position;
			teleportPosition.y = playerTransform.position.y;
			playerTransform.DOMove(teleportPosition, teleportDuration)
					.SetEase(Ease.OutCubic);
		}
	}
}