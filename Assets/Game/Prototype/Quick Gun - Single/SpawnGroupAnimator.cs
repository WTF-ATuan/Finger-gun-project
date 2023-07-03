using System;
using System.Collections.Generic;
using Core;
using Game.Project;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Prototype.Quick_Gun___Single{
	public class SpawnGroupAnimator : MonoBehaviour{
		[TitleGroup("During Setting")] [MinMaxSlider(0, 20, true)]
		public Vector2 duringMinMax = new(0.5f, 1f);

		[InlineButton("AutoGetChildSpawners")] public List<Spawner> spawnerList;

		private ColdDownTimer _timer;

		private void Start(){
			_timer = new ColdDownTimer();
		}

		private void FixedUpdate(){
			if(!_timer.CanInvoke()) return;
			Spawn();
			var randomDuring = Random.Range(duringMinMax.x, duringMinMax.y);
			_timer.ModifyDuring(randomDuring);
			_timer.Reset();
		}

		private void Spawn(){
			var randomIndex = Random.Range(0, spawnerList.Count);
			var spawner = spawnerList[randomIndex];
			spawner.enabled = false;
			spawner.enabled = true;
		}

		private void AutoGetChildSpawners(){
			spawnerList.Clear();
			for(var i = 0; i < transform.childCount; i++){
				var child = transform.GetChild(i);
				if(child.TryGetComponent(out Spawner spawner)){
					spawnerList.Add(spawner);
				}
			}
		}
	}
}