using System;
using System.Net;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SystemConfiguration;
using MonoTouch.CoreFoundation;

public enum NetworkStatus 
{
	NotReachable,
	ReachableViaCarrierDataNetwork,
	ReachableViaWiFiNetwork
}

public static class Reachability 
{
	public static string HostName = "www.google.com";

	public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
	{
		bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;
		bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

		if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
			noConnectionRequired = true;

		return isReachable && noConnectionRequired;
	}

	public static bool IsHostReachable (string host)
	{
		if (host == null || host.Length == 0)
			return false;

		using (var r = new NetworkReachability (host)){
			NetworkReachabilityFlags flags;

			if (r.TryGetFlags (out flags)){
				return IsReachableWithoutRequiringConnection (flags);
			}
		}
		return false;
	}

	public static event EventHandler ReachabilityChanged;
	static void OnChange (NetworkReachabilityFlags flags)
	{
		var h = ReachabilityChanged;
		if (h != null)
			h (null, EventArgs.Empty);
	}

	static NetworkReachability adHocWiFiNetworkReachability;
	public static bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
	{
		if (adHocWiFiNetworkReachability == null){
			adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte [] {169,254,0,0}));
			adHocWiFiNetworkReachability.SetCallback (OnChange);
			adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
		}

		if (!adHocWiFiNetworkReachability.TryGetFlags (out flags))
			return false;

		return IsReachableWithoutRequiringConnection (flags);
	}

	static NetworkReachability defaultRouteReachability;
	static bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
	{
		if (defaultRouteReachability == null){
			defaultRouteReachability = new NetworkReachability (new IPAddress (0));
			defaultRouteReachability.SetCallback (OnChange);
			defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
		}
		if (!defaultRouteReachability.TryGetFlags (out flags))
			return false;
		return IsReachableWithoutRequiringConnection (flags);
	}	

	static NetworkReachability remoteHostReachability;
	public static NetworkStatus RemoteHostStatus ()
	{
		NetworkReachabilityFlags flags;
		bool reachable;

		if (remoteHostReachability == null)
		{
			remoteHostReachability = new NetworkReachability (HostName);

			reachable = remoteHostReachability.TryGetFlags (out flags);

			remoteHostReachability.SetCallback (OnChange);
			remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
		} 
		else
			reachable = remoteHostReachability.TryGetFlags (out flags);			

		if (!reachable)
			return NetworkStatus.NotReachable;

		if (!IsReachableWithoutRequiringConnection (flags))
			return NetworkStatus.NotReachable;

		if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
			return NetworkStatus.ReachableViaCarrierDataNetwork;

		return NetworkStatus.ReachableViaWiFiNetwork;
	}

	public static NetworkStatus InternetConnectionStatus ()
	{
		NetworkReachabilityFlags flags;
		bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
		if (defaultNetworkAvailable){
			if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
				return NetworkStatus.NotReachable;
		} else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
			return NetworkStatus.ReachableViaCarrierDataNetwork;
		else if (flags == 0)
			return NetworkStatus.NotReachable;
		return NetworkStatus.ReachableViaWiFiNetwork;
	}

	public static NetworkStatus LocalWifiConnectionStatus ()
	{
		NetworkReachabilityFlags flags;
		if (IsAdHocWiFiNetworkAvailable (out flags)){
			if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
				return NetworkStatus.ReachableViaWiFiNetwork;
		}
		return NetworkStatus.NotReachable;
	}
}

