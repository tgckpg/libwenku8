﻿using System;
using System.IO;
using System.Text;
using System.Net;

using Net.Astropenguin.Loaders;

namespace GR.AdvDM
{
	sealed class SHttpRequest : HttpRequest
	{
		public SHttpRequest( Uri RequestUri )
			: base( RequestUri ) { }

		override protected void CreateRequest()
		{
			base.CreateRequest();
			WCMessage.Headers.Add( "User-Agent", WHttpRequest.UA );
		}
	}
}