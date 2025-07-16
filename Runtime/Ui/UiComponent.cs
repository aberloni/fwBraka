using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

	public class UiComponent : MonoUi, iUIComponent
	{
		protected iUIMediator mediator;

		public virtual void SetMediator(iUIMediator mediator)
		{
			this.mediator = mediator;
		}

		protected void SendNotification(string notif)
		{
			if (mediator == null)
			{
				Debug.LogWarning("no mediator set", this);
				return;
			}

			Debug.Log(name + " -> " + notif, this);

			mediator.Notify(this, notif);
		}
	}

}
