using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core{
	public interface ISpawner{
		public Action<GameObject> OnSpawn{ get; set; }
		public List<Vector3> SpawnPoint{ get;  }
	}
}