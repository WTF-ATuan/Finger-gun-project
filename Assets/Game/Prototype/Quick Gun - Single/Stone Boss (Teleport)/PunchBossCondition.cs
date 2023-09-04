using System;
using DG.Tweening;
using Game.Project;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossCondition : StateMachineBehaviour{
		[SerializeField] private float specialAttackDuration = 20f;


		private Transform _player;
		private Collider _detectTrigger;
		private Animator _animator;
		private PunchBossAdapter _adapter;


		private int _hitCount;
		private static readonly int Hard = Animator.StringToHash("Hard");
		private static readonly int Light = Animator.StringToHash("Light");
		private static readonly int Special = Animator.StringToHash("Special");

		private ColdDownTimer _specialAttackTimer;

		private void Awake(){
			_specialAttackTimer = new ColdDownTimer(specialAttackDuration);
		}

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
			if(_adapter.bossCurrentStage == 1){
				FirstStage(animator);
			}
			else{
				animator.SetBool(_adapter.hitCount > 1 ? Hard : Light, PlayerInRange());
				if(!_specialAttackTimer.CanInvoke()) return;
				_animator.SetTrigger(Special);
				_specialAttackTimer.Reset();
			}
		}

		private void FirstStage(Animator animator){
			animator.SetBool(_adapter.hitCount > 2 ? Hard : Light, PlayerInRange());
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
			animator.SetBool(Hard, false);
			animator.SetBool(Light, false);
		}

		private void TurnToPlayer(){
			var rotation = Quaternion.LookRotation((_player.position - _animator.transform.position).normalized);
			var eulerAngles = _animator.transform.eulerAngles;
			eulerAngles.y = rotation.eulerAngles.y + 180;
			_animator.transform.DORotate(eulerAngles, _adapter.bossTurningDuration).SetEase(Ease.OutQuad);
		}

		private bool PlayerInRange(){
			var contains = _detectTrigger.bounds.Contains(_player.position);
			return contains;
		}
	}
}