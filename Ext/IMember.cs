﻿using System.Threading.Tasks;
using Windows.Foundation;

namespace GR.Ext
{
	enum MemberStatus
	{
		LOGGED_IN = 1
		, LOGGED_OUT = 2
		, RE_LOGIN_NEEDED = 4
	}

	interface IMember
	{
		MemberStatus Status { get; set; }

		bool IsLoggedIn { get; }
		bool WillLogin { get; }
		bool CanRegister { get; }

		string CurrentAccount { get; }
		string CurrentPassword { get; }
		string ServerMessage { get; }

		event TypedEventHandler<object, MemberStatus> OnStatusChanged;

		Task<bool> Register();
		void Logout();
		void Login( string Name, string Passwd, bool Remember = false );
	}
}