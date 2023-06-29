using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Iron_Boss{
	public class TargetMover : MonoBehaviour{
		[SerializeField] [Required] private Transform controlTarget;
		[SerializeField] private List<Transform> pathNode;
		[SerializeField] private float duration = 2f;
		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
		[SerializeField] private bool debugLine;
		[SerializeField] private Color debugColor = Color.green;

		private void Start(){
			NextPath(0);
		}

		private void NextPath(int index){
			if(index >= pathNode.Count) index = 0;
			controlTarget.DOMove(pathNode[index].position, duration)
					.OnComplete(() => NextPath(index + 1))
					.SetEase(curve);
			controlTarget.transform.DOLookAt(pathNode[index].position, 0.5f);
		}

		private void OnDrawGizmos(){
			if(!debugLine){
				return;
			}

			Gizmos.color = debugColor;
			Gizmos.DrawWireCube(controlTarget.position, Vector3.one * 0.3f);
			for(var i = 0; i < pathNode.Count; i++){
				Gizmos.DrawWireCube(pathNode[i].position, Vector3.one * 0.3f);
				var nextIndex = i + 1;
				if(nextIndex >= pathNode.Count) nextIndex = 0;
				Gizmos.DrawLine(pathNode[i].position, pathNode[nextIndex].position);
			}
		}
	}
}