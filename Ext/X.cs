﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using SType = System.Type;
namespace GR.Ext
{
	class X
	{
		private static Dictionary<string, SType> Resolved = new Dictionary<string, SType>();
		private static Dictionary<string, object> InstanceStore = new Dictionary<string, object>();

		public static bool Exists { get; private set; }

		public static void Init()
		{
			try
			{
				object o = Instance<object>( XProto.Init ) != null;
				Exists = o != null;
			}
			catch ( DllNotFoundException ) { }
		}

		public static T Instance<T>( string Name, params object[] args )
		{
			object instance = Activator.CreateInstance( Type( Name ), args );
			return ( T ) instance;
		}

		public static T Singleton<T>( string Name, params object[] args )
		{
			if ( InstanceStore.ContainsKey( Name ) ) return ( T ) InstanceStore[ Name ];

			object instance = Activator.CreateInstance( Type( Name ), args );
			InstanceStore[ Name ] = instance;

			return ( T ) instance;
		}

		public static T Call<T>( string Name, string MethodName, params object[] args )
		{
			return ( T ) Method( Name, MethodName ).Invoke( null, args );
		}

		internal static T Static<T>( string Name, string PropName )
		{
			return ( T ) Prop( Name, PropName ).GetValue( null );
		}

		internal static T Const<T>( string Name, string ConstName )
		{
			return ( T ) Field( Type( Name ), ConstName ).GetValue( null );
		}

		internal static SType Type( string Name )
		{
			if ( Resolved.ContainsKey( Name ) ) return Resolved[ Name ];

			SType t = SType.GetType( Name );
			Resolved[ Name ] = t ?? throw new DllNotFoundException( "Extension dll is not present" );
			return t;
		}

		public static MethodInfo Method( string Name, string MethodName )
		{
			return Method( Type( Name ), MethodName );
		}

		public static PropertyInfo Prop( string Name, string PropName )
		{
			return Prop( Type( Name ), PropName );
		}

		public static PropertyInfo Prop( SType obj, string PropName )
		{
			PropertyInfo PInfo = obj.GetProperty(
				PropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance );

			if( PInfo == null )
			{
				throw new KeyNotFoundException( "No such property" );
			}

			return PInfo;
		}

		public static FieldInfo Field( SType obj, string FieldName )
		{
			FieldInfo FInfo = obj.GetField( FieldName );

			if( FInfo == null )
			{
				throw new KeyNotFoundException( "No such field" );
			}

			return FInfo;
		}

		public static MethodInfo Method( SType obj, string MethodName )
		{
			MethodInfo MInfo = obj.GetMethod( MethodName );
			if( MInfo == null )
			{
				throw new KeyNotFoundException( "No such method" );
			}

			return MInfo;
		}
	}

	public static class XExt
	{
		public static bool XTest( this object Obj, string Name )
		{
			SType t = X.Type( Name );
			SType o = Obj.GetType();
			return t.Equals( o ) || o.GetTypeInfo().IsSubclassOf( t );
		}

		public static T XProp<T>( this object Obj, string Prop )
		{
			return ( T ) X.Prop( Obj.GetType(), Prop ).GetValue( Obj );
		}

		public static T XField<T>( this object Obj, string Prop )
		{
			return ( T ) X.Field( Obj.GetType(), Prop ).GetValue( Obj );
		}

		public static void XSetProp( this object Obj, string Prop, object Value )
		{
			X.Field( Obj.GetType(), Prop ).SetValue( Obj, Value );
		}

		public static void XCall( this object Obj, string Method, params object[] args )
		{
			X.Method( Obj.GetType(), Method ).Invoke( Obj, args );
		}

		public static T XCall<T>( this object Obj, string Method, params object[] args )
		{
			return ( T ) X.Method( Obj.GetType(), Method ).Invoke( Obj, args );
		}

		public static Task XCallAsync( this object Obj, string Method, params object[] args )
		{
			return ( Task ) X.Method( Obj.GetType(), Method ).Invoke( Obj, args );
		}

		public static Task<T> XCallAsync<T>( this object Obj, string Method, params object[] args )
		{
			return ( Task<T> ) X.Method( Obj.GetType(), Method ).Invoke( Obj, args );
		}
	}

}