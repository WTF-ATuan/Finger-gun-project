using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class DizzyBehavior : StateMachineBehaviour{
		private StoneBoss _bossRoot;

		private void Awake(){
			_bossRoot = FindObjectOfType<StoneBoss>();
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
			int layerIndex){
			_bossRoot.footHealth = 25;
		}
	}
}