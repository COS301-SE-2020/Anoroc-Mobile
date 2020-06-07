package crc649948b0e780c45f6b;


public class MyAccessTokenTracker
	extends com.facebook.AccessTokenTracker
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCurrentAccessTokenChanged:(Lcom/facebook/AccessToken;Lcom/facebook/AccessToken;)V:GetOnCurrentAccessTokenChanged_Lcom_facebook_AccessToken_Lcom_facebook_AccessToken_Handler\n" +
			"";
		mono.android.Runtime.register ("AnorocMobileApp.Droid.Resources.services.MyAccessTokenTracker, AnorocMobileApp.Android", MyAccessTokenTracker.class, __md_methods);
	}


	public MyAccessTokenTracker ()
	{
		super ();
		if (getClass () == MyAccessTokenTracker.class)
			mono.android.TypeManager.Activate ("AnorocMobileApp.Droid.Resources.services.MyAccessTokenTracker, AnorocMobileApp.Android", "", this, new java.lang.Object[] {  });
	}


	public void onCurrentAccessTokenChanged (com.facebook.AccessToken p0, com.facebook.AccessToken p1)
	{
		n_onCurrentAccessTokenChanged (p0, p1);
	}

	private native void n_onCurrentAccessTokenChanged (com.facebook.AccessToken p0, com.facebook.AccessToken p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
