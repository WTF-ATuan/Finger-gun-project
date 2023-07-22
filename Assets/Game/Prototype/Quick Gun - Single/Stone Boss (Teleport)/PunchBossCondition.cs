using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossCondition : StateMachineBehaviour{
		[SerializeField] private float turningDuration = 6.5f;
		private Transform _player;
		private Collider _detectTrigger;
		private Animator _animator;
		private PunchBossAdapter _adapter;


		private int _hitCount;
		private static readonly int Hard = Animator.StringToHash("Hard");
		private static readonly int Light = Animator.StringToHash("Light");


		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			_animator = animator;
			_adapter = animator.GetComponent<PunchBossAdapter>();
			_player = _adapter.player;
			_detectTrigger = _adapter.detectTrigger;
			if(!PlayerInRange()){
				TurnToPlayer();
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			if(_adapter.hitCount > 2){
				animator.SetBool(Hard, PlayerInRange());
			}
			else{
				animator.SetBool(Light, PlayerInRange());
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
			animator.SetBool(Hard, false);
			animator.SetBool(Light, false);
		}

		private void TurnToPlayer(){
			var rotation = Quaternion.LookRotation((_player.position - _animator.transform.position).normalized);
			var eulerAngles = _animator.transform.eulerAngles;
			eulerAngles.y = rotation.eulerAngles.y + 180;
			_animator.transform.DORotate(eulerAngles, turningDuration).SetEase(Ease.OutQuad);
		}

		private bool PlayerInRange(){
			var contains = _detectTrigger.bounds.Contains(_player.position);
			return contains;
		}
	}
}