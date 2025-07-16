using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	/// <summary>
	/// manage all MonoB based methods
	/// without Start/Update
	/// </summary>
	abstract public partial class Mono : MonoBehaviour, iLogger, iDebug
	{
		[SerializeField]
		iLogger.LogLevel verbosity = iLogger.LogLevel.none;

		protected void overrideVerbosity(iLogger.LogLevel verbosity)
		{
			this.verbosity = verbosity;
		}

		/// <summary>
		/// can override to add some verbosity filter
		/// </summary>
		virtual public bool IsVerbose(iLogger.LogLevel lvl) => verbosity >= lvl;
		public string GetIdentity() => name;

		Transform _tr;
		public Transform tr => _tr;

		private void Awake()
		{
			_tr = transform;
			build();
		}

		virtual protected void build()
		{
		}

		private void OnDestroy()
		{
			destroy();
		}

		virtual protected void destroy()
		{ }

		protected string _stringify = string.Empty;
		virtual public string stringify()
		{
			_stringify = stamp();
			if (!enabled) _stringify += " !enabled";
			return _stringify;
		}

		[ContextMenu("log.stringify")]
		void cmLogStringify() => Debug.Log(stringify(), this);

		/// <summary>
		/// verbose sensible
		/// </summary>
		protected void log(string content, object target = null) => BrakaLogger.log(this, content, target);
		protected void logw(string content, object target = null) => BrakaLogger.logw(this, content, target);
		protected void logc(string content, object target = null) => BrakaLogger.logChatty(this, content, target);

		/// <summary>
		/// unfiltered logs
		/// </summary>
		protected void ulog(string msg, object target = null) => Debug.Log(msg, target as Object);

		/// <summary>
		/// identitifcation
		/// </summary>
		virtual public string stamp() => $"<{GetType()}>{name}";
	}

}
