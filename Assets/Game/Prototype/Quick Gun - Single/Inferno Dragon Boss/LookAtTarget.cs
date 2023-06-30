using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Inferno_Dragon_Boss{
	public class LookAtTarget : MonoBehaviour{
		[SerializeField] private Transform target;

		private void FixedUpdate(){
			transform.DODynamicLookAt(target.position, 0.1f);
		}
	}
}