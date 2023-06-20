using System;
using Game.Project;
using HelloPico2.InteractableObjects;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Inferno_Dragon_Boss{
	public class DragonBoss : MonoBehaviour{
		[SerializeField] private Spawner fireBallSpawner;
		[SerializeField] private Spawner enemySpawner;
		[SerializeField] private Spawner specialFireballSpawner;


		//HitBox
		[BoxGroup("HitBox")] [SerializeField] private Collider bodyHitBox;
		[BoxGroup("HitBox")] [SerializeField] private Collider headHitBox;
		[BoxGroup("HitBox")] [SerializeField] private Collider coreHitBox;


		//Animation
		[SerializeField] private AnimationClip fireBallClip;
		[SerializeField] private AnimationClip specialFireBallClip;
		private Animator _animator;

		private int _hitCount;
		private ColdDownTimer _fireballBehaviorTimer;
		private ColdDownTimer _coreHitBoxTimer;

		[BoxGroup("Health")] public Image hpBar;
		[BoxGroup("Health")] public float hp = 100;

		private void Start(){
			_fireballBehaviorTimer = new ColdDownTimer(6.5f);
			_coreHitBoxTimer = new ColdDownTimer(3);
			_animator = GetComponentInChildren<Animator>();
			coreHitBox.OnCollisionEnterAsObservable().Subscribe(OnCoreHit);
			bodyHitBox.OnCollisionEnterAsObservable().Subscribe(x => Hit(x, 0.1f));
			headHitBox.OnCollisionEnterAsObservable().Subscribe(x => Hit(x, 0.5f));
		}

		private void Hit(Collision obj, float damage){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			hp -= damage;
			if(hp < 0){
				fireBallSpawner.gameObject.SetActive(false);
				specialFireballSpawner.gameObject.SetActive(false);
				_animator.SetBool("Dead", true);
				_animator.enabled = false;
			}
			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0, hp / 100), right.y);
			hpBar.rectTransform.offsetMax = right;
		}

		private void OnCoreHit(Collision obj){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			hp -= 10;
			if(hp < 0){
				fireBallSpawner.gameObject.SetActive(false);
				specialFireballSpawner.gameObject.SetActive(false);
				_animator.SetBool("Dead", true);
				_animator.enabled = false;
			}

			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0, hp / 100), right.y);
			hpBar.rectTransform.offsetMax = right;
		}

		private void Update(){
			if(_hitCount >= 5){
				SpecialFireballBehavior();
				return;
			}

			if(enemySpawner.spawnCount >= 10){
				ThrowEnemyBehavior();
				return;
			}

			FireballBehavior();
			ActiveCoreHitBox();
		}

		private void ActiveCoreHitBox(){
			coreHitBox.gameObject.SetActive(!_coreHitBoxTimer.CanInvoke());
		}

		private void FireballBehavior(){
			if(!_fireballBehaviorTimer.CanInvoke()) return;
			fireBallSpawner.enabled = false;
			fireBallSpawner.enabled = true;
			_animator.Play(fireBallClip.name);
			_fireballBehaviorTimer.Reset();
			_hitCount++;
		}

		private void ThrowEnemyBehavior(){
			Debug.Log($"ThrowEnemyBehavior");
			enemySpawner.enabled = false;
		}

		private void SpecialFireballBehavior(){
			specialFireballSpawner.enabled = false;
			specialFireballSpawner.enabled = true;
			_animator.Play(specialFireBallClip.name);
			_fireballBehaviorTimer.Reset();
			_coreHitBoxTimer.Reset();
			_hitCount = 0;
		}
	}
}