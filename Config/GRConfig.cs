﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

using Net.Astropenguin.Messaging;

namespace GR.Config
{
	class GRConfig
	{
		public static Messenger ConfigChanged = new Messenger();

		public static Scopes.Theme Theme => new Scopes.Theme();
		public static Scopes.GRSystem System => new Scopes.GRSystem();
		public static Scopes.ContentReader ContentReader => new Scopes.ContentReader();
	}
}