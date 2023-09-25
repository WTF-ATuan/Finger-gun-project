using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Inferno_Dragon_Boss{
	public class LookAtTarget : MonoBehaviour{
		[SerializeField] private Transform target;
		[SerializeField] private bool yOffset;


		private void FixedUpdate(){
			if(yOffset){
				transform.DODynamicLookAt(target.position, 0.1f, AxisConstraint.Y);
			}
			else{
				transform.DODynamicLookAt(target.position, 0.1f);
			}
		}
	}
}