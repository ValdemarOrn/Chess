using Chess.Lib.MoveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	delegate void CallbackDelegate(IntPtr key, IntPtr value);

	public class Manager
	{
		public const string BeginMessage = "%%BEGIN%%";
		public const string EndMessage = "%%END%%";

		public const string SearchData = "SearchData";
		public const string Ply = "Ply";
		public const string Score = "Score";
		public const string Nodes = "Nodes";
		public const string PV = "PV";

		public static void InitLibrary()
		{
			new Eval();
			new Moves();
			new Zobrist();

			new Pawn();
			new Rook();
			new Knight();
			new Bishop();
			new King();
			new Queen();

			TTable.Init(256);

			cb = Callback;
			var ptr = Marshal.GetFunctionPointerForDelegate(cb);
			Manager.SetCallback(ptr);
			SendMessage = DefaultHandler;
		}

		public static Action<string, Dictionary<string, string>> SendMessage { get; set; }

		static CallbackDelegate cb;

		static string MessageType;
		static Dictionary<string, string> MessageData;

		static unsafe void Callback(IntPtr keyPtr, IntPtr valuePtr)
		{
			byte* b = (byte*)keyPtr;
			var key = Helpers.GetString(b);

			b = (byte*)valuePtr;
			var value = Helpers.GetString(b);

			if (key == BeginMessage)
			{
				MessageData = new Dictionary<string, string>();
				MessageType = value;
			}
			else if (key == EndMessage)
				SendMessage(MessageType, MessageData);
			else if (MessageData.ContainsKey(key))
				MessageData[key] = MessageData[key] + value;
			else
				MessageData[key] = value;
		}

		static void DefaultHandler(string type, Dictionary<string, string> data)
		{
			foreach(var kvp in data)
				Console.Write(kvp.Key + " " + kvp.Value + " ");

			Console.Write("\n");
		}

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Manager_SetCallback", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetCallback(IntPtr callbackPtr);
	}
}
