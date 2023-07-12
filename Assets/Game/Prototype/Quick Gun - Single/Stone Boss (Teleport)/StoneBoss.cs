using System.Collections.Generic;
using Game.Project;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class StoneBoss : MonoBehaviour{
		[Required] public Transform player;

		private Animator _animator;

		[SerializeField] [MinMaxSlider(-180, 180, true)]
		private Vector2 attackAngleRange = new(-45, 45);

		[FormerlySerializedAs("attackRange")] [SerializeField]
		private float attackDistance = 10;

		[BoxGroup("DetectData")] public float angleBetweenPlayer;
		[BoxGroup("DetectData")] public float distanceBetweenPlayer;


		[BoxGroup("Hit")] public List<Collider> footColliderList;
		[BoxGroup("Hit")] public GameObject hitVFX;
		[BoxGroup("Hit")] public AudioClip hitSFX;
		[BoxGroup("Hit")] public float footHealth = 25;

		public ColdDownTimer ThrowStoneTimer;


		private void Start(){
			_animator = GetComponent<Animator>();
			ThrowStoneTimer = new ColdDownTimer(10f);
			footColliderList.ForEach(x => x.OnCollisionEnterAsObservable()
					.Subscribe(FootHit));
		}

		private void FootHit(Collision col){
			if(!col.gameObject.TryGetComponent(out Projectile projectile)) return;
			var contactPoint = col.GetContact(0);
			var vfxClone = Instantiate(hitVFX, contactPoint.point, Quaternion.Euler(contactPoint.normal));
			vfxClone.AddComponent<AudioSource>().PlayOneShot(hitSFX);
			Destroy(vfxClone, 1f);
			footHealth -= 1;
		}

		private void Update(){
			UpdateAnimationState();
			DetectAttackRange();
		}

		private void DetectAttackRange(){
			var playerPosition = player.position;
			var bossPosition = transform.position;
			var bossToPlayer = playerPosition - bossPosition;
			distanceBetweenPlayer = Vector3.Distance(bossPosition, playerPosition);
			angleBetweenPlayer = (Mathf.Atan2(bossToPlayer.z, bossToPlayer.x) * Mathf.Rad2Deg +
				transform.eulerAngles.y - 90) % 360;
		}

		private void UpdateAnimationState(){
			_animator.SetBool($"Throw", CanThrow());
			_animator.SetBool($"Dizzy", IsDizzy());
			_animator.SetBool($"TurnRight", NeedTurn(true));
			_animator.SetBool($"TurnLeft", NeedTurn(false));
			_animator.SetBool($"StepAttack", CanStep());
		}

		private bool NeedTurn(bool isRight){
			var playerInAngle = attackAngleRange.x < angleBetweenPlayer && angleBetweenPlayer < attackAngleRange.y;
			if(playerInAngle){
				return false;
			}

			if(angleBetweenPlayer < 0){
				return isRight;
			}

			return !isRight;
		}

		private bool IsDizzy(){
			return footHealth > 0;
		}

		private bool CanStep(){
			if(!ThrowStoneTimer.CanInvoke()) return false;
			var playerInAngle = attackAngleRange.x < angleBetweenPlayer && angleBetweenPlayer < attackAngleRange.y;
			var playerInDistance = distanceBetweenPlayer < attackDistance;
			return playerInAngle && playerInDistance;
		}

		private bool CanThrow(){
			if(!ThrowStoneTimer.CanInvoke()) return false;
			var playerInAngle = attackAngleRange.x < angleBetweenPlayer && angleBetweenPlayer < attackAngleRange.y;
			var playerInDistance = distanceBetweenPlayer > attackDistance;
			return playerInAngle && playerInDistance;
		}

		private void OnDrawGizmos(){
			if(!player) return;
			Gizmos.color = Color.green;
			var position = transform.position;
			var minAngle = (attackAngleRange.x - transform.eulerAngles.y + 90) * Mathf.Deg2Rad;
			var minX = position.x + attackDistance * Mathf.Cos(minAngle);
			var minZ = position.z + attackDistance * Mathf.Sin(minAngle);
			Gizmos.DrawLine(position, new Vector3(minX, position.y, minZ));
			var maxAngle = (attackAngleRange.y - transform.eulerAngles.y + 90) * Mathf.Deg2Rad;
			var maxX = position.x + attackDistance * Mathf.Cos(maxAngle);
			var maxZ = position.z + attackDistance * Mathf.Sin(maxAngle);
			Gizmos.DrawLine(position, new Vector3(maxX, position.y, maxZ));
		}
	}
}