using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class TurnBehavior : StateMachineBehaviour{
		private StoneBoss _bossRoot;
		public bool isTurnRight = true;


		private void Awake(){
			_bossRoot = FindObjectOfType<StoneBoss>();
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			var duration = stateInfo.length;
			var eulerAngles = _bossRoot.transform.eulerAngles;
			var rotateAngle = isTurnRight ? 90f : -90f;
			_bossRoot.transform.DORotate(new Vector3(eulerAngles.x, eulerAngles.y + rotateAngle, eulerAngles.z),
						duration)
					.SetEase(Ease.InCubic);
		}
	}
}