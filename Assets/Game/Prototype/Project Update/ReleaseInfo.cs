using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Game.Prototype.Project_Update{
	[Serializable]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class ReleaseInfo{
		public string tag_name;
		public List<AssetInfo> assets;
		public string apk_url{
			get{
				foreach(var asset in assets.Where(asset => asset.name.EndsWith(".apk"))){
					return asset.browser_download_url;
				}

				throw new Exception("Can,t Find Apk Url, Please check release asset");
			}
		}

		[Serializable]
		public class AssetInfo{
			public string name;
			public string browser_download_url;
		}
	}
}