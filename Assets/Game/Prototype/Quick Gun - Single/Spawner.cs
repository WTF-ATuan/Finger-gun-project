﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Project;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core{
	public class Spawner : MonoBehaviour, ISpawner{
		[Required] public GameObject spawnPrefab;


		[TitleGroup("RotateSetting")]
		[OnValueChanged("EditSpawnPosition")]
		[InfoBox("改動數值將會覆寫已調整好的位置資訊", InfoMessageType.Warning)]
		public float spawnRadius = 5f;

		[TitleGroup("RotateSetting")] [OnValueChanged("EditSpawnPosition")]
		public int spawnCount = 5;

		[SerializeField] [TitleGroup("During Setting")] [MinMaxSlider(0, 20, true)]
		public Vector2 duringMinMax = new Vector2(0.5f, 1f);

		public bool clearCloneListWhenSpawnOver = false;
		public bool spawnLoop = false;


		[TitleGroup("Edit")] public bool edit;

		[TitleGroup("Edit")] [ShowIf("edit")] [ReadOnly]
		public List<Transform> spawnPointList = new List<Transform>();

		[TitleGroup("Edit")] [ShowIf("edit")] [SerializeField] [ReadOnly]
		private List<GameObject> cloneList = new List<GameObject>();

		[TitleGroup("Edit")] [ShowIf("edit")] [SerializeField]
		private Color gizmosColor = Color.green;

		private ColdDownTimer _timer;

		public Action<GameObject> OnSpawn{ get; set; }

		public List<Vector3> SpawnPoint => spawnPointList.Select(x => x.position).ToList();
		public List<GameObject> CloneList => cloneList;

		private void OnEnable(){
			_timer = new ColdDownTimer();
			spawnCount = spawnPointList.Count;
		}

		private void OnDisable(){
			cloneList.RemoveAll(x => x == null);
			if(clearCloneListWhenSpawnOver){
				cloneList.Clear();
			}
		}

		private void FixedUpdate(){
			if(!_timer.CanInvoke()) return;
			Spawn();
			var randomDuring = Random.Range(duringMinMax.x, duringMinMax.y);
			_timer.ModifyDuring(randomDuring);
			_timer.Reset();
		}

		private void Spawn(){
			var spawnIndex = cloneList.Count;
			if(spawnIndex >= spawnCount){
				if(spawnLoop){
					cloneList.RemoveAll(x => x == null);
					cloneList.Clear();
					spawnIndex = 0;
				}
				else{
					return;
				}
			}

			var spawnPoint = spawnPointList[spawnIndex];
			var spawnObject = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
			spawnObject.SetActive(true);
			OnSpawn?.Invoke(spawnObject);
			cloneList.Add(spawnObject);
		}

		public void EditSpawnPosition(){
			spawnPointList.ForEach(x => DestroyImmediate(x.gameObject));
			spawnPointList.Clear();
			var angle = 360 / spawnCount;
			for(var i = 0; i < spawnCount; i++){
				var point = CreatePoint(spawnRadius, angle * (i + 1));
				point.name = $"Edit Point [ index : {i} radius : {spawnRadius}]";
				spawnPointList.Add(point.transform);
			}
		}

		private GameObject CreatePoint(float radius, float angle){
			var spawnerTransform = transform;
			var position = spawnerTransform.position;
			var spawnPosition = position + radius * Vector3.up;
			var vectorAngle = angle * Vector3.forward;
			var rotatePosition = Quaternion.Euler(vectorAngle) * (spawnPosition - position) + position;
			var spawnPoint = new GameObject{
				transform ={
					position = rotatePosition,
					parent = spawnerTransform
				}
			};
			return spawnPoint;
		}

		private void OnDrawGizmos(){
			if(spawnPointList.Exists(x => x == null)) spawnPointList.RemoveAll(x => x == null);

			if(!edit){
				if(spawnPointList.Count < 1) return;
				spawnPointList.ForEach(x => DestroyImmediate(x.gameObject));
				spawnPointList.Clear();
				return;
			}

			Gizmos.color = gizmosColor;

			foreach(var spawnPoint in spawnPointList){
				Gizmos.matrix = spawnPoint.localToWorldMatrix;
				if(!spawnPrefab){
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);
				}
				else{
					var meshFilter = spawnPrefab.GetComponent<MeshFilter>();
					if(meshFilter){
						Gizmos.DrawWireMesh(meshFilter.sharedMesh, Vector3.zero, Quaternion.identity,
							spawnPrefab.transform.localScale);
					}
					else{
						Gizmos.DrawWireCube(Vector3.zero, spawnPrefab.transform.localScale);
					}
				}
			}
		}
	}
}