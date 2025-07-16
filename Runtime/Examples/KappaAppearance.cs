using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace fwp.braka
{

	public class KappaAppearance : KappaMono
	{
		public void fetchUi<T>(string uid, Action<T> onCompletion = null) where T : UiObject
		{
			string path = "UI/" + uid;
			AddrFetcher.instance.instantiate(path, (copy) =>
			{
				var uio = (copy as GameObject).GetComponent<T>();
				Debug.Assert(uio != null, "fail@" + path + "<" + typeof(T) + ">");
				onCompletion?.Invoke(uio);
			});
		}

	}
}