using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class StoneBoss : MonoBehaviour{
		[Required] [SerializeField] private Transform player;

		private Animator _animator;


		private void Start(){
			_animator = GetComponent<Animator>();
		}

		private void Update(){
			UpdateAnimationState();
			DetectAttackRange();
		}

		private void DetectAttackRange(){
			var playerPosition = player.position;
			var bossPosition = transform.position;
			var distanceBetweenPlayer = Vector3.Distance(bossPosition, playerPosition);
			var bossToPlayer = playerPosition - bossPosition;
			var angleBetweenPlayer = Vector3.Dot(bossPosition, bossToPlayer);
			Debug.Log($"angleBetweenPlayer = {angleBetweenPlayer} + {distanceBetweenPlayer}");
		}

		private void UpdateAnimationState(){
			_animator.SetBool($"Throw", CanThrow());
			_animator.SetBool($"Dizzy", IsDizzy());
			_animator.SetBool($"TurnRight", NeedTurn(true));
			_animator.SetBool($"TurnLeft", NeedTurn(false));
		}

		private bool NeedTurn(bool isRight){
			return false;
		}

		private bool IsDizzy(){
			return false;
		}

		private bool CanThrow(){
			return false;
		}
	}
}