using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

	public interface iUIMediator
	{
		void Notify(iUIComponent sender, string eventName);
	}

	public interface iUIComponent
	{ }
}