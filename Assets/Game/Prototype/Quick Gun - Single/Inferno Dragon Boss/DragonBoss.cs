using DG.Tweening;
using Game.Project;
using Core;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Inferno_Dragon_Boss{
	public class DragonBoss : MonoBehaviour{
		[BoxGroup("Core")] [SerializeField] private Collider coreHitBox;
		[BoxGroup("Core")] [SerializeField] private Collider coreHitBoxSecondStage;
		[BoxGroup("Core")] [SerializeField] private AudioClip coreHitSound;
		[BoxGroup("Core")] [SerializeField] private GameObject coreHitParticle;

		[BoxGroup("Blocker")] [SerializeField] private Spawner blockSpawner;

		[BoxGroup("Blocker")] [SerializeField] private AnimationClip throwClip;

		[BoxGroup("Group Blocker")] [SerializeField]
		private SpawnGroupAnimator blockGroupAnimator;

		[BoxGroup("Group Blocker")] [SerializeField]
		private AnimationClip groupBlockClip;


		[BoxGroup("FireBall")] [SerializeField]
		private Spawner fireBallSpawner;

		[BoxGroup("FireBall")] [SerializeField]
		private AnimationClip fireBallClip;

		[BoxGroup("FireBall")] [SerializeField]
		private float fireBallColdDown = 10;

		[BoxGroup("Health")] public Image hpBar;
		[BoxGroup("Health")] public float hp = 100;

		[SerializeField] private UnityEvent stageTwoEvent;
		[SerializeField] private UnityEvent onDeadEvent;


		private Animator _animator;
		private AudioSource _audioSource;
		private ColdDownTimer _fireballBehaviorTimer;
		private bool _stageTwo;
		private int _attackCount;

		private void Start(){
			_animator = GetComponentInChildren<Animator>();
			_audioSource = GetComponent<AudioSource>();
			coreHitBox.OnCollisionEnterAsObservable().Subscribe(x => {
				if(!x.gameObject.TryGetComponent(out Projectile projectile)) return;
				CoreHit(coreHitBox);
			});
			coreHitBoxSecondStage.OnCollisionEnterAsObservable().Subscribe(x => {
				if(!x.gameObject.TryGetComponent(out Projectile projectile)) return;
				CoreHit(coreHitBoxSecondStage);
			});
			coreHitBoxSecondStage.gameObject.SetActive(false);
			coreHitBoxSecondStage.enabled = false;
			_fireballBehaviorTimer = new ColdDownTimer(fireBallColdDown);
		}

		[Button]
		private void Hit(float damage){
			hp -= damage;
			if(hp < 60){
				_animator.SetBool($"Fly", true);
				coreHitBox.transform.DOScale(0, 1.5f).OnComplete(() => coreHitBox.gameObject.SetActive(false));
				coreHitBoxSecondStage.gameObject.SetActive(true);
				stageTwoEvent?.Invoke();
				_stageTwo = true;
			}

			if(hp < 0){
				fireBallSpawner.gameObject.SetActive(false);
				_animator.SetBool($"Dead", true);
				onDeadEvent?.Invoke();
				enabled = false;
			}

			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0, hp / 100), right.y);
			hpBar.rectTransform.offsetMax = right;
		}

		private void CoreHit(Collider hitBox){
			_audioSource.PlayOneShot(coreHitSound);
			var vfxClone = Instantiate(coreHitParticle, hitBox.transform.position, Quaternion.identity);
			hitBox.gameObject.SetActive(false);
			hitBox.transform.DOShakeScale(1f).OnComplete(() => hitBox.gameObject.SetActive(true));
			Destroy(vfxClone, 1.5f);
			Hit(15);
		}


		private void Update(){
			if(!_stageTwo){
				if(_attackCount < 3){
					FireballBehavior();
				}
				else{
					ThrowObject();
				}
			}
			else{
				ThrowGroupBlocker();
			}
		}

		private void FireballBehavior(){
			if(!_fireballBehaviorTimer.CanInvoke()) return;
			fireBallSpawner.enabled = false;
			// ReSharper disable once Unity.InefficientPropertyAccess
			fireBallSpawner.enabled = true;
			_animator.Play(fireBallClip.name);
			_fireballBehaviorTimer.Reset();
			_attackCount++;
		}

		private void ThrowObject(){
			if(!_fireballBehaviorTimer.CanInvoke()) return;
			blockSpawner.enabled = false;
			// ReSharper disable once Unity.InefficientPropertyAccess
			blockSpawner.enabled = true;
			_animator.Play(throwClip.name);
			_attackCount = 0;
			_fireballBehaviorTimer.Reset();
		}

		private void ThrowGroupBlocker(){
			if(!_fireballBehaviorTimer.CanInvoke()) return;
			coreHitBoxSecondStage.enabled = true;
			var localRotation = coreHitBoxSecondStage.transform.localEulerAngles;
			localRotation.y += 180;
			coreHitBoxSecondStage.transform.DOLocalRotate(localRotation, 3f)
					.OnComplete(() => coreHitBoxSecondStage.enabled = false);
			blockGroupAnimator.enabled = false;
			blockGroupAnimator.enabled = true;
			_animator.Play(groupBlockClip.name);
			_fireballBehaviorTimer.ModifyDuring(15);
			_fireballBehaviorTimer.Reset();
		}
	}
}