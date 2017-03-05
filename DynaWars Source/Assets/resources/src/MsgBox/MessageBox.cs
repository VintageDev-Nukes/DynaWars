using CorruptedSmileStudio.MessageBox;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
	public Texture error;
	private static MessageBox i;
	public Texture info;
	private static List<MessageItem> items = new List<MessageItem>();
	public Rect messageBoxRect = new Rect(0f, 0f, 350f, 200f);
	public Texture warning;
	
	private void Awake()
	{
		i = this; //GameObject.Find("MenuScripts").GetComponent<MessageBox>();
	}
	
	private void Button(Rect place, string caption, DialogResult result)
	{
		if (GUI.Button(place, caption))
		{
			if (items[0].callback != null)
			{
				items[0].callback(result);
			}
			items.RemoveAt(0);
		}
	}
	
	private void Display(int id)
	{
		GUI.Label(new Rect(5f, 20f, this.messageBoxRect.width - 20f, this.messageBoxRect.height - 50f), items[0].message);
		this.DisplayButtons(items[0]);
	}
	
	private void DisplayButtons(MessageItem item)
	{
		switch (item.buttons)
		{
		case MessageBoxButtons.OK:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(this.messageBoxRect.width - 55f, this.messageBoxRect.height - 30f, 50f, 25f), "Ok", DialogResult.Ok);
			break;
			
		case MessageBoxButtons.OKCancel:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Ok", DialogResult.Ok);
			GUI.SetNextControlName("Button2");
			this.Button(new Rect(60f, this.messageBoxRect.height - 30f, 55f, 25f), "Cancel", DialogResult.Cancel);
			break;
			
		case MessageBoxButtons.AbortRetryIgnore:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Abort", DialogResult.Abort);
			GUI.SetNextControlName("Button2");
			this.Button(new Rect(60f, this.messageBoxRect.height - 30f, 50f, 25f), "Retry", DialogResult.Retry);
			GUI.SetNextControlName("Button3");
			this.Button(new Rect(115f, this.messageBoxRect.height - 30f, 55f, 25f), "Ignore", DialogResult.Ignore);
			break;
			
		case MessageBoxButtons.YesNoCancel:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Yes", DialogResult.Yes);
			GUI.SetNextControlName("Button2");
			this.Button(new Rect(60f, this.messageBoxRect.height - 30f, 50f, 25f), "No", DialogResult.No);
			GUI.SetNextControlName("Button3");
			this.Button(new Rect(115f, this.messageBoxRect.height - 30f, 55f, 25f), "Cancel", DialogResult.Cancel);
			break;
			
		case MessageBoxButtons.YesNo:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Yes", DialogResult.Yes);
			GUI.SetNextControlName("Button2");
			this.Button(new Rect(60f, this.messageBoxRect.height - 30f, 50f, 25f), "No", DialogResult.No);
			break;
			
		case MessageBoxButtons.RetryCancel:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Retry", DialogResult.Retry);
			GUI.SetNextControlName("Button2");
			this.Button(new Rect(60f, this.messageBoxRect.height - 30f, 55f, 25f), "Cancel", DialogResult.Cancel);
			break;
			
		default:
			GUI.SetNextControlName("Button1");
			this.Button(new Rect(5f, this.messageBoxRect.height - 30f, 50f, 25f), "Close", DialogResult.None);
			break;
		}
		GUI.FocusControl(item.defaultButton.ToString());
	}
	
	private void OnGUI()
	{
		if (items.Count > 0)
		{
			this.messageBoxRect.x = (((float) Screen.width) / 2f) - (this.messageBoxRect.width / 2f);
			this.messageBoxRect.y = (((float) Screen.height) / 2f) - (this.messageBoxRect.height / 2f);
			this.messageBoxRect = GUI.Window(0, this.messageBoxRect, new GUI.WindowFunction(this.Display), items[0].caption);
		}
	}
	
	public static void Show(MessageCallback callback, string message)
	{
		Show(callback, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
	}
	
	public static void Show(MessageCallback callback, string message, string caption)
	{
		Show(callback, message, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
	}
	
	public static void Show(MessageCallback callback, string message, string caption, MessageBoxButtons buttons)
	{
		Show(callback, message, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
	}
	
	public static void Show(MessageCallback callback, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		Show(callback, message, caption, buttons, icon, MessageBoxDefaultButton.Button1);
	}
	
	public static void Show(MessageCallback callback, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
	{
		MessageItem item = new MessageItem {
			caption = caption,
			buttons = buttons,
			defaultButton = defaultButton,
			callback = callback
		};
		item.message.text = message;
		switch (icon)
		{
		case MessageBoxIcon.Hand:
		case MessageBoxIcon.Stop:
		case MessageBoxIcon.Error:
			item.message.image = i.error;
			break;
			
		case MessageBoxIcon.Exclamation:
		case MessageBoxIcon.Warning:
			item.message.image = i.warning;
			break;
			
		case MessageBoxIcon.Asterisk:
		case MessageBoxIcon.Information:
			item.message.image = i.info;
			break;
		}
		items.Add(item);
	}
}
