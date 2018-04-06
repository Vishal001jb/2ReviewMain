package md5e3a693f588173924be4f4eeb0d479edf;


public class userinput
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("_2ReviewEmployeeSideHomeScreen.Activity.userinput, 2ReviewEmployeeSideHomeScreen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", userinput.class, __md_methods);
	}


	public userinput ()
	{
		super ();
		if (getClass () == userinput.class)
			mono.android.TypeManager.Activate ("_2ReviewEmployeeSideHomeScreen.Activity.userinput, 2ReviewEmployeeSideHomeScreen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
