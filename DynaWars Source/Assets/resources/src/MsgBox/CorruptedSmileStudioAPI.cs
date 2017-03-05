namespace CorruptedSmileStudio.MessageBox
{
	using System;
	using System.Runtime.CompilerServices;
	using UnityEngine;
	
	public enum DialogResult
	{
		None,
		Ok,
		Cancel,
		Abort,
		Retry,
		Ignore,
		Yes,
		No
	}

	public enum MessageBoxButtons
	{
		OK,
		OKCancel,
		AbortRetryIgnore,
		YesNoCancel,
		YesNo,
		RetryCancel
	}

	public enum MessageBoxDefaultButton
	{
		Button1,
		Button2,
		Button3
	}

	public enum MessageBoxIcon
	{
		None,
		Hand,
		Exclamation,
		Asterisk,
		Stop,
		Error,
		Warning,
		Information
	}

	public delegate void MessageCallback(DialogResult result);

	public class MessageItem
	{
		public MessageBoxButtons buttons;
		public MessageCallback callback;
		public string caption;
		public MessageBoxDefaultButton defaultButton;
		public GUIContent message;
		
		public MessageItem()
		{
			this.message = new GUIContent();
			this.caption = string.Empty;
			this.buttons = MessageBoxButtons.OK;
			this.defaultButton = MessageBoxDefaultButton.Button1;
		}
		
		public MessageItem(MessageCallback call, GUIContent content, string cap, MessageBoxButtons btns, MessageBoxDefaultButton defaultBtn)
		{
			this.message = content;
			this.caption = cap;
			this.buttons = btns;
			this.defaultButton = defaultBtn;
		}
	}

}


