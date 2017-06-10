package md5836cbedc49b955fe9bbd696d60980cb1;


public class Main_menu_admin
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("FlashCardsPort.Droid.Main_menu_admin, FlashCardsPort.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Main_menu_admin.class, __md_methods);
	}


	public Main_menu_admin () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Main_menu_admin.class)
			mono.android.TypeManager.Activate ("FlashCardsPort.Droid.Main_menu_admin, FlashCardsPort.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
