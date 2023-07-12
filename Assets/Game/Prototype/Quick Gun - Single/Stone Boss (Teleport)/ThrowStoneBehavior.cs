using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class ThrowStoneBehavior : StateMachineBehaviour{
		private StoneBoss _bossRoot;

		private void Awake(){
			_bossRoot = FindObjectOfType<StoneBoss>();
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){ }

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			_bossRoot.ThrowStoneTimer.Reset();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			var playerPosition = _bossRoot.player.position;
			_bossRoot.transform.DODynamicLookAt(playerPosition, 0.3f, AxisConstraint.Y);
		}
	}
}