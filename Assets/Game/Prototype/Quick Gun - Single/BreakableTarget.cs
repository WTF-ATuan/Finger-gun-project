﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single{
	public class BreakableTarget : MonoBehaviour{
		[SerializeField] private int destroyCount = 1;
		private int _hitCount;

		public void OnCollisionEnter(Collision collision){
			if(!collision.gameObject.TryGetComponent(out Projectile projectile)) return;
			_hitCount++;
			if(_hitCount >= destroyCount){
				transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
			}
		}
	}
}