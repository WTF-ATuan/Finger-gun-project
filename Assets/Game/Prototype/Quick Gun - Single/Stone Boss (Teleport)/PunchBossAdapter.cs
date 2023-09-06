using System.Collections.Generic;
using System.Linq;
using Game.Project;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossAdapter : MonoBehaviour{
		[Required] [BoxGroup("Basic")] public Transform player;
		[Required] [BoxGroup("Basic")] public Collider detectTrigger;
		[Required] [BoxGroup("Basic")] public float bossStartHp = 100;
		[Required] [BoxGroup("Basic")] public float bossTurningDuration = 5;

		[BoxGroup("Weakness")] public Collider weaknessCore;
		[BoxGroup("Weakness")] public List<Collider> amplifiers;
		[BoxGroup("Weakness")] public float healthDuration = 2;
		[BoxGroup("Weakness")] public GameObject weaknessHitVFX;

		[BoxGroup("Health")] public Image hpBar;

		[ReadOnly] public int hitCount;
		[ReadOnly] public float bossHp;
		[ReadOnly] public int bossCurrentStage = 1;

		[FoldoutGroup("Event")] public UnityEvent enableSecondStage;
		[FoldoutGroup("Event")] public UnityEvent enableBossDead;

		private ColdDownTimer _timer;
		private Animator _animator;

		private void Start(){
			weaknessCore.OnCollisionEnterAsObservable().Subscribe(OnWeaknessCoreHit);
			amplifiers.ForEach(x =>
					x.OnCollisionEnterAsObservable().Subscribe(collision => OnAmplifierHit(collision, x)));
			bossCurrentStage = 1;
			bossHp = bossStartHp;
			_timer = new ColdDownTimer(healthDuration);
			_animator = GetComponent<Animator>();
		}

		[Button]
		[FoldoutGroup("Event")]
		private void TestHit(float amount = 10){
			DamageBoss(amount);
		}

		private void OnWeaknessCoreHit(Collision obj){
			if(!obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			var vfxClone = Instantiate(weaknessHitVFX, obj.GetContact(0).point,
				Quaternion.Euler(obj.GetContact(0).normal));
			Destroy(vfxClone, 0.35f);
			DamageBoss();
		}

		private void OnAmplifierHit(Collision obj, Collider amplifier){
			if(!obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			var vfxClone = Instantiate(weaknessHitVFX, obj.GetContact(0).point,
				Quaternion.Euler(obj.GetContact(0).normal));
			Destroy(vfxClone, 0.35f);
			amplifier.gameObject.SetActive(false);
		}

		private void ReHealthBoss(){
			var activeAmplifierCount = amplifiers.FindAll(x => x.gameObject.activeSelf).Count;
			bossHp = Mathf.Clamp(bossHp + activeAmplifierCount * 2, 0, bossStartHp);
			UpdateHpBar();
		}

		private void DamageBoss(float damageAmount = 7.5f){
			bossHp = Mathf.Clamp(bossHp - damageAmount, 0, bossStartHp);
			UpdateHpBar();
			if(bossHp > 0) return;
			bossCurrentStage += 1;
			switch(bossCurrentStage){
				case 2:
					_animator.SetTrigger($"Next");
					enableSecondStage?.Invoke();
					hpBar.color = Color.yellow;
					amplifiers.ForEach(x => x.gameObject.SetActive(true));
					break;
				case 3:
					_animator.SetTrigger($"Dead");
					enableBossDead?.Invoke();
					return;
			}

			bossHp = bossStartHp;
			UpdateHpBar();
		}

		private void UpdateHpBar(){
			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0, bossHp / bossStartHp), right.y);
			hpBar.rectTransform.offsetMax = right;
		}

		public void ModifyTurnDuration(float duration){
			bossTurningDuration = duration;
		}

		private void FixedUpdate(){
			if(!_timer.CanInvoke()) return;
			ReHealthBoss();
			_timer.Reset();
		}

		#region Trigger_By_Animation

		public void LightAttack(){
			hitCount += 1;
		}

		public void HardAttack(){
			hitCount = 0;
		}

		private bool _preAttackInRange;

		public void PreAttackCheck(){
			_preAttackInRange = detectTrigger.bounds.Contains(player.position);
		}

		public void PostAttackCheck(){
			if(_preAttackInRange && !detectTrigger.bounds.Contains(player.position)){
				EventAggregator.Publish(new DodgeAction());
			}
		}

		#endregion
	}

	public class DodgeAction{ }
}