using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.AddressableAssets; // pkg : Unity.Addressables
using UnityEngine.ResourceManagement.AsyncOperations; // pkg : Unity.ResourceManager

using Object = UnityEngine.Object;

public class AddrFetcher
{
	static AddrFetcher _instance;
	static public AddrFetcher instance
	{
		get
		{
			if (_instance == null) _instance = new AddrFetcher();
			return _instance;
		}
	}

	Dictionary<string, AddrPair> pairs = new Dictionary<string, AddrPair>();

	public class AddrPair
	{
		public AsyncOperationHandle<GameObject> asyncOp;
		public GameObject addrBlob;

		public List<Action<Object>> asyncQueue;

		public void queue(Action<Object> callback)
		{
			if (asyncQueue == null) asyncQueue = new List<Action<Object>>();
			asyncQueue.Add(callback);
		}

		public void assign(GameObject blob)
		{
			addrBlob = blob;

			// un-pile queue
			if (asyncQueue != null)
			{
				IndustriesVerbosity.sLog("@" + addrBlob.name + " de-piling x" + asyncQueue.Count);
				foreach (var q in asyncQueue)
				{
					q.Invoke(addrBlob);
				}

				asyncQueue.Clear();
				asyncQueue = null;
			}

		}

		public void free()
		{
			Addressables.Release(addrBlob);

			if (asyncQueue != null)
			{
				asyncQueue.Clear();
				asyncQueue = null;
			}

			if (asyncOp.IsValid() && !asyncOp.IsDone)
			{
				Debug.LogError("freeing while fetching ??");
			}
		}
	}

	string solvePath(string addrKey)
	{
		//return "Assets/" + addrKey + ".prefab";
		return addrKey;
	}

	public void prime(string path)
	{
		path = solvePath(path);
		fetchAddr(path, null);
	}

	void fetchAddr(string path, Action<GameObject> onPresence = null)
	{
		IndustriesVerbosity.sLog("<b>fetch</b> ... @" + path);

		AddrPair pair = new AddrPair();
		pairs.Add(path, pair);

		pair.asyncOp = Addressables.LoadAssetAsync<GameObject>(path);

		pair.asyncOp.Completed += (obj) =>
		{
			switch (obj.Status)
			{
				case AsyncOperationStatus.Succeeded:

					IndustriesVerbosity.sLog("<b>fetch</b>&success @" + path);

					pair.assign(obj.Result);

					onPresence?.Invoke(pair.addrBlob);

					break;
				case AsyncOperationStatus.Failed:
				default:
					Debug.LogError("<b>fetch</b>&failure " + obj.Status + " @ " + path);
					break;
			}

		};

	}

	void getBlob(string path, Action<Object> onPresence)
	{
		if (pairs.ContainsKey(path))
		{
			var p = pairs[path];

			if (p.asyncOp.IsValid() && !p.asyncOp.IsDone)
			{
				//Debug.LogError($"addr async @{path} is still fetching");
				//Debug.LogError("can't re-ask for another that quickly");

				IndustriesVerbosity.swLog("addr:queue-ing callback @" + path);
				p.queue(onPresence);
			}
			else
			{
				//IndustriesVerbosity.sLog("addr:pairs already has @" + path);
				onPresence(pairs[path].addrBlob);
			}

		}
		else
		{
			fetchAddr(path, onPresence);
		}
	}

	public void instantiate(string path, Action<Object> onPresence)
	{
		Debug.Assert(onPresence != null, "wrong implem, should have callback here");

		path = solvePath(path);

		getBlob(path, (blob) =>
		{
			Object copy = null;

			if (blob == null) Debug.LogWarning("blob is null @" + path);
			else
			{
				copy = GameObject.Instantiate(blob);
			}

			onPresence(copy);
		});
	}
}
