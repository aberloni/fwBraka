using fwp.industries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO
/// use a factory for all UI items
/// </summary>
abstract public class UiObject : fwp.braka.Mono
{
	//abstract public string GetCandidateName();

	protected override void build()
	{
		base.build();

		var c = GameObject.FindAnyObjectByType<Canvas>();
		tr.SetParent(c.transform);

	}

	public void recycle()
	{
		//TODO : use factory

		gameObject.SetActive(false);
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
