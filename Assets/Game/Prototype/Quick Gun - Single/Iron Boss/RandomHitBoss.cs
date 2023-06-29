using System;
using Game.Project;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Prototype.Quick_Gun___Single.Iron_Boss{
	public class RandomHitBoss : MonoBehaviour{
		[SerializeField] private AnimationClip leftPunch;
		[SerializeField] private AnimationClip centerPunch;
		[SerializeField] private AnimationClip rightPunch;
		[SerializeField] private float duration = 15f;

		private ColdDownTimer _timer;
		private Animator _animator;

		private void Start(){
			_timer = new ColdDownTimer(duration);
			_animator = GetComponent<Animator>();
		}

		private void Update(){
			if(!_timer.CanInvoke()){
				return;
			}

			var range = Random.Range(0, 3);
			switch(range){
				case 0:
					_animator.Play(leftPunch.name);
					break;
				case 1:
					_animator.Play(centerPunch.name);
					break;
				case 2:
					_animator.Play(rightPunch.name);
					break;
			}

			_timer.Reset();
		}
	}
}