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
		public Vector2 spawnDuringMinMax = new(0.5f, 1f);

		[InlineButton("AutoGetChildSpawners")] public List<Spawner> spawnerList;

		[TitleGroup("Wave Setting")] public bool spawnLoop = true;
		[TitleGroup("Wave Setting")] public float during = 5;

		[TitleGroup("Wave Setting")] public float delayOpenTime = 10;

		private bool _active;
		private ColdDownTimer _spawnerDurationTimer;
		private ColdDownTimer _waveDurationTimer;
		private int _spawnCount;

		private void OnEnable(){
			_spawnerDurationTimer = new ColdDownTimer();
			_waveDurationTimer = new ColdDownTimer(during);
		}

		private void OnDisable(){
			spawnerList.ForEach(x => x.enabled = false);
			_spawnCount = 0;
		}

		private void FixedUpdate(){
			if(spawnLoop){
				SpawnWithLoop();
			}
			else{
				if(!_spawnerDurationTimer.CanInvoke()) return;
				if(_spawnCount >= spawnerList.Count) return;
				_spawnCount++;
				Spawn();
				var randomDuring = Random.Range(spawnDuringMinMax.x, spawnDuringMinMax.y);
				_spawnerDurationTimer.ModifyDuring(randomDuring);
				_spawnerDurationTimer.Reset();
			}
		}

		private void SpawnWithLoop(){
			if(_active){
				if(_waveDurationTimer.CanInvoke()){
					_active = false;
					_waveDurationTimer.ModifyDuring(delayOpenTime);
					_waveDurationTimer.Reset();
				}

				if(!_spawnerDurationTimer.CanInvoke()) return;
				Spawn();
				var randomDuring = Random.Range(spawnDuringMinMax.x, spawnDuringMinMax.y);
				_spawnerDurationTimer.ModifyDuring(randomDuring);
				_spawnerDurationTimer.Reset();
			}
			else{
				if(!_waveDurationTimer.CanInvoke()){
					return;
				}

				_active = true;
				_waveDurationTimer.ModifyDuring(during);
				_waveDurationTimer.Reset();
			}
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