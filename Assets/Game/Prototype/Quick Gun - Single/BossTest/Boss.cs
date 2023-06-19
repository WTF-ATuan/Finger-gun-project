using System;
using Sirenix.Utilities;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single{
	public class Boss : MonoBehaviour{
		[SerializeField] private GameObject bossCore;
		[SerializeField] private GameObject[] bossBody;
		[SerializeField] private GameObject shield;
		public string currentState = "Defence";
		public Image hpBar;
		public float hp = 100;
		private float _dizzyTimer;

		private void Start(){
			bossCore.OnCollisionEnterAsObservable().Subscribe(OnBossCoreHit);
			bossBody.ForEach(x => x.OnCollisionEnterAsObservable().Subscribe(OnBossBodyHit));
		}

		private void OnBossCoreHit(Collision obj){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			currentState = "Dizzy";
			hp -= 10;
		}

		private void OnBossBodyHit(Collision obj){
			if(!obj.gameObject.TryGetComponent(out Projectile projectile)) return;
			if(currentState == "Defence"){
				hp -= 0.5f;
			}

			if(currentState == "Dizzy"){
				hp -= 2f;
			}
		}

		private void Update(){
			if(currentState == "Defence"){
				shield.transform.eulerAngles += new Vector3(0, 0, 1) * (Time.deltaTime * 30f);
			}

			if(currentState == "Dizzy"){
				_dizzyTimer += Time.fixedDeltaTime;
				if(_dizzyTimer >= 2.5f){
					currentState = "Defence";
					_dizzyTimer = 0;
				}
			}

			// 0 = 100 5 = 0
			var right = hpBar.rectTransform.offsetMax;
			right = new Vector3(Mathf.Lerp(-5, 0 , hp /100), right.y);
			hpBar.rectTransform.offsetMax = right;
		}
	}
}