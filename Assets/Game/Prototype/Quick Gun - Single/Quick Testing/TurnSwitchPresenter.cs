using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Prototype.Quick_Gun___Single{
	public class TurnSwitchPresenter : MonoBehaviour{
		public UnityEvent turn1Over;
		public UnityEvent turn2Over;

		public int goal = 5;
		private int _goalCount;
		private int _currentTurn = 1;

		public GameObject playerGameObject;

		public TMP_Text goalText;


		private void Start(){
			EventAggregator.OnEvent<TargetHitEvent>().Subscribe(OnTargetHit);
		}

		private void OnTargetHit(TargetHitEvent obj){
			_goalCount += 1;
			goalText.text = _goalCount + "/" + goal;
			if(_goalCount < goal) return;
			switch(_currentTurn){
				case 1:
					turn1Over?.Invoke();
					break;
				case 2:
					turn2Over?.Invoke();
					break;
			}

			_goalCount = 0;
			goal += 5;
			_currentTurn++;
		}

		public void MovePlayerForward(Transform target){
			playerGameObject.transform.DOMove(target.position, 1.5f);
		}
	}
}