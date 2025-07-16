using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	public interface iLogger
	{

		public enum LogLevel
		{
			none = 0,
			verbose = 1,
			deep = 2,
		}

		public bool IsVerbose(LogLevel lvl);

		/// <summary>
		/// string identifing
		/// </summary>
		public string GetIdentity();
	}

	/// <summary>
	/// static class to manage log methods
	/// </summary>
	static public class BrakaLogger
	{
		static public string stampPrefix()
		{
			string prefix = "(ru)";
			if (Application.isEditor) prefix = "(ed)";
			if (!Application.isPlaying) prefix = "!" + prefix;
			prefix += "@" + Time.frameCount;
			return prefix;
		}

		public const string space = " ";
		public const string separator = " | ";

		static public void logw(iLogger owner, string content, object target = null)
		{
			if (!owner.IsVerbose(iLogger.LogLevel.verbose)) return;
			if (target == null) target = owner;
			Debug.LogWarning(stamp(owner) + content, target as Object);
		}

		/// <summary>
		/// verbosity != none
		/// </summary>
		static public void log(iLogger owner, string content, object target = null)
		{
			if (!owner.IsVerbose(iLogger.LogLevel.verbose)) return;
			if (target == null) target = owner;
			Debug.Log(stamp(owner) + content, target as Object);
		}

		/// <summary>
		/// need deep level
		/// </summary>
		static public void logChatty(iLogger owner, string content, object target = null)
		{
			if (!owner.IsVerbose(iLogger.LogLevel.deep)) return;
			log(owner, content, target);
		}

		static public string stamp(iLogger log)
		{
			return stampPrefix() + separator + log.GetIdentity() + separator;
		}

	}

}