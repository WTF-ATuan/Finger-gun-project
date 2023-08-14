using System;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossAdapter : MonoBehaviour{
		[Required] [BoxGroup("Basic")] public Transform player;
		[Required] [BoxGroup("Basic")] public Collider detectTrigger;
		[Required] [BoxGroup("Basic")] public float bossStartHp = 100;
		[Required] [BoxGroup("Basic")] public float bossTurningDuration = 5;

		[BoxGroup("Weakness")] public BoxCollider weaknessFront;
		[BoxGroup("Weakness")] public BoxCollider weaknessBack;
		[BoxGroup("Weakness")] public GameObject weaknessHitVFX;

		[BoxGroup("Health")] public Image hpBar;

		[ReadOnly] public int hitCount;
		[ReadOnly] public float bossHp;
		[ReadOnly] public int bossCurrentStage = 1;

		[FoldoutGroup("Event")] public UnityEvent enableHalfHealth;
		[FoldoutGroup("Event")] public UnityEvent enableSecondStage;
		[FoldoutGroup("Event")] public UnityEvent enableThirdStage;
		[FoldoutGroup("Event")] public UnityEvent enableBossDead;

		private void Start(){
			weaknessFront.OnCollisionEnterAsObservable().Subscribe(OnWeaknessHit);
			weaknessBack.OnCollisionEnterAsObservable().Subscribe(OnWeaknessHit);
			bossCurrentStage = 1;
			bossHp = bossStartHp;
		}

		[Button]
		[FoldoutGroup("Event")]
		private void TestHit(float amount = 10){
			DamageBoss(amount);
		}

		private void OnWeaknessHit(Collision obj){
			if(!obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			var vfxClone = Instantiate(weaknessHitVFX, obj.GetContact(0).point,
				Quaternion.Euler(obj.GetContact(0).normal));
			Destroy(vfxClone, 0.35f);
			DamageBoss();
		}

		private void DamageBoss(float damageAmount = 7.5f){
			bossHp -= damageAmount;
			if(bossHp < 50 && bossCurrentStage == 1){
				enableHalfHealth?.Invoke();
			}
			UpdateHpBar();
			if(bossHp > 0) return;
			bossCurrentStage += 1;
			switch(bossCurrentStage){
				case 2:
					enableSecondStage?.Invoke();
					hpBar.color = Color.yellow;
					break;
				case 3:
					enableThirdStage?.Invoke();
					hpBar.color = Color.red;
					break;
				case 4:
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

		#region Trigger_By_Animation

		public void LightAttack(){
			hitCount += 1;
		}

		public void HardAttack(){
			hitCount = 0;
		}

		#endregion
	}
}