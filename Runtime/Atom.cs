using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	abstract public class Atom : iLogger, iDebug
	{
		virtual public bool IsVerbose(iLogger.LogLevel lvl) => false;

		public string GetIdentity() => stringify();

		virtual public string stringify()
		{
			return GetType().ToString();
		}

		protected void log(string msg, object tar = null)
		{
			if (tar == null) tar = this;
			iLogger il = tar as iLogger;
			BrakaLogger.log(il != null ? il : this, msg);
		}

		protected void logChatty(string msg, object tar = null)
		{
			if (tar == null) tar = this;
			iLogger il = tar as iLogger;
			BrakaLogger.logChatty(il != null ? il : this, msg);
		}
	}

}