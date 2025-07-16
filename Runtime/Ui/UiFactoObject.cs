using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using fwp.industries;

/// <summary>
/// this object will be attached to a canvas
/// </summary>
abstract public class UiFactoObject : fwp.braka.Mono, iFactoryObject
{
	abstract public string GetCandidateName();

	protected override void build()
	{
		base.build();

		var c = GetComponentInParent<Canvas>();
		if (c == null) c = FindAnyObjectByType<Canvas>();

		Debug.Assert(c != null, "no canvas?", this);

		transform.SetParent(c.transform);

		clear();
	}

	virtual public void clear()
	{
		gameObject.SetActive(false);
	}

	public void OnRecycled()
	{
		clear();
	}
}
