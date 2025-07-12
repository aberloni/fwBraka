using UnityEngine;

namespace fwp.braka
{
	public interface iDebug
	{
		public bool isVerbose();
		public void log(string msg, object tar = null);
		public void logw(string msg, object tar = null);
		public string stringify();
	}
}