using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class StoneBoss : MonoBehaviour{
		[Required] [SerializeField] private Transform player;

		private Animator _animator;

		[SerializeField] [MinMaxSlider(-180, 180, true)]
		private Vector2 attackAngleRange = new(-45, 45);

		[FormerlySerializedAs("attackRange")] [SerializeField]
		private float attackDistance = 10;

		private float _angleBetweenPlayer;
		private float _distanceBetweenPlayer;


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
			var bossToPlayer = playerPosition - bossPosition;
			_distanceBetweenPlayer = Vector3.Distance(bossPosition, playerPosition);
			_angleBetweenPlayer = Mathf.Atan2(bossToPlayer.z, bossToPlayer.x) * Mathf.Rad2Deg + 90;
		}

		private void UpdateAnimationState(){
			_animator.SetBool($"Throw", CanThrow());
			_animator.SetBool($"Dizzy", IsDizzy());
			_animator.SetBool($"TurnRight", NeedTurn(true));
			_animator.SetBool($"TurnLeft", NeedTurn(false));
		}

		private bool NeedTurn(bool isRight){
			var playerInAngle = attackAngleRange.x < _angleBetweenPlayer && _angleBetweenPlayer < attackAngleRange.y;
			if(playerInAngle){
				return false;
			}

			if(_angleBetweenPlayer < 0){
				return isRight;
			}
			return !isRight;
		}

		private bool IsDizzy(){
			return false;
		}

		private bool CanThrow(){
			var playerInAngle = attackAngleRange.x < _angleBetweenPlayer && _angleBetweenPlayer < attackAngleRange.y;
			var playerInDistance = _distanceBetweenPlayer > attackDistance;
			return playerInAngle && playerInDistance;
		}

		private void OnDrawGizmos(){
			if(!player) return;
			Gizmos.color = Color.green;
			var position = transform.position;
			var minAngle = (attackAngleRange.x - 90) * Mathf.Deg2Rad;
			var minX = position.x + attackDistance * Mathf.Cos(minAngle);
			var minZ = position.z + attackDistance * Mathf.Sin(minAngle);
			Gizmos.DrawLine(position, new Vector3(minX, position.y, minZ));
			var maxAngle = (attackAngleRange.y - 90) * Mathf.Deg2Rad;
			var maxX = position.x + attackDistance * Mathf.Cos(maxAngle);
			var maxZ = position.z + attackDistance * Mathf.Sin(maxAngle);
			Gizmos.DrawLine(position, new Vector3(maxX, position.y, maxZ));
		}
	}
}