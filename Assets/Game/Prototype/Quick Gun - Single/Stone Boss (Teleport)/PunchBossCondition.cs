using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossCondition : StateMachineBehaviour{
		[SerializeField] private Transform player;

		[SerializeField] private Collider detectTrigger;
		[SerializeField] private float turningDuration = 6.5f;
		private Animator _animator;


		private int _hitCount;
		private static readonly int Hard = Animator.StringToHash("Hard");
		private static readonly int Light = Animator.StringToHash("Light");

		private bool _lightAttackFlag;
		private bool _hardAttackFlag;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			_animator = animator;
			if(!PlayerInRange()){
				TurnToPlayer();
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			if(_hitCount > 2){
				animator.SetBool(Hard, PlayerInRange());
				_hardAttackFlag = true;
			}
			else{
				animator.SetBool(Light, PlayerInRange());
				_lightAttackFlag = true;
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
			animator.transform.DOKill();
			animator.SetBool(Hard, false);
			animator.SetBool(Light, false);
			if(_lightAttackFlag) _hitCount += 1;
			if(_hardAttackFlag) _hitCount = 0;
		}

		private void TurnToPlayer(){
			var rotation = Quaternion.LookRotation((player.position - _animator.transform.position).normalized);
			var eulerAngles = _animator.transform.eulerAngles;
			eulerAngles.y = rotation.eulerAngles.y + 180;
			_animator.transform.DORotate(eulerAngles, turningDuration).SetEase(Ease.OutQuad);
			// _animator.transform.DODynamicLookAt(player.position, turningDuration, AxisConstraint.Y)
			// 		.SetEase(Ease.OutQuad);
		}

		private bool PlayerInRange(){
			var contains = detectTrigger.bounds.Contains(player.position);
			return contains;
		}
	}
}