using Game.Project;
using HelloPico2.InteractableObjects;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Inferno_Dragon_Boss{
	public class DragonBoss : MonoBehaviour{
		[SerializeField] private Spawner fireBallSpawner;


		//HitBox
		[BoxGroup("HitBox")] [SerializeField] private Collider bodyHitBox;
		[BoxGroup("HitBox")] [SerializeField] private Collider headHitBox;


		//Animation
		[SerializeField] private AnimationClip fireBallClip;
		[SerializeField] private float fireBallColdDown = 10;
		private Animator _animator;

		private ColdDownTimer _fireballBehaviorTimer;

		[BoxGroup("Health")] public Image hpBar;
		[BoxGroup("Health")] public float hp = 100;


		private void Start(){
			_animator = GetComponentInChildren<Animator>();
			bodyHitBox.OnCollisionEnterAsObservable().Subscribe(x => Hit(x, 0.1f));
			headHitBox.OnCollisionEnterAsObservable().Subscribe(x => Hit(x, 0.5f));
			_fireballBehaviorTimer = new ColdDownTimer(fireBallColdDown);
		}

		private void Hit(Collision obj, float damage){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			hp -= damage;
			if(hp < 0){
				fireBallSpawner.gameObject.SetActive(false);
				_animator.SetBool("Dead", true);
				_animator.enabled = false;
			}

			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0, hp / 100), right.y);
			hpBar.rectTransform.offsetMax = right;
		}


		private void Update(){
			FireballBehavior();
		}

		private void FireballBehavior(){
			if(!_fireballBehaviorTimer.CanInvoke()) return;
			fireBallSpawner.enabled = false;
			// ReSharper disable once Unity.InefficientPropertyAccess
			fireBallSpawner.enabled = true;
			_animator.Play(fireBallClip.name);
			_fireballBehaviorTimer.Reset();
		}
	}
}