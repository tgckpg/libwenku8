﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

using Net.Astropenguin.Loaders;

namespace GR.GSystem
{
	static class Utils
	{
		public static void DoNothing( string arg1, string arg2, Exception arg3 ) { }
		public static void DoNothing( DRequestCompletedEventArgs arg1, string arg2 ) { }
		public static void DoNothing() { }

		public static string AutoByteUnit( ulong size )
		{
			double b = 1.0d * size;
			string unit = "Byte";
			if ( b > 1024 )
			{
				b /= 1024;
				unit = "KB";
			}
			if ( b > 1024 )
			{
				b /= 1024;
				unit = "MB";
			}
			if ( b > 1024 )
			{
				b /= 1024;
				unit = "GB";
			}
			b = Math.Round( b, 2 );
			return b.ToString() + " " + unit;
		}

		public static bool Numberstring( string n )
		{
			if ( n.Length == 0 ) return false;
			foreach ( char p in n )
			{
				if ( !char.IsDigit( p ) )
					return false;
			}
			return true;
		}

		public static DateTime GetDateTimeFromstring( string time )
		{
			if ( Numberstring( time ) && time.Length != 14 )
				throw new FormatException();
			return new DateTime(
				int.Parse( time.Substring( 0, 4 ) )
				, int.Parse( time.Substring( 4, 2 ) )
				, int.Parse( time.Substring( 6, 2 ) )
				, int.Parse( time.Substring( 8, 2 ) )
				, int.Parse( time.Substring( 10, 2 ) )
				, int.Parse( time.Substring( 12 ) )
			 );
		}

		public static string GetStringStream( Stream e )
		{
			string p;
			using ( StreamReader k = new StreamReader( e ) )
			{
				p = k.ReadToEnd();
			}
			return p;
		}

		internal static bool CompareVersion( string thisVer, string CurrentVer )
		{
			string[] k = thisVer.Split( '.' );
			string[] l = CurrentVer.Split( '.' );
			if ( int.Parse( k[ 3 ] ) >= int.Parse( l[ 3 ] )
				&& int.Parse( k[ 2 ] ) >= int.Parse( l[ 2 ] )
				&& int.Parse( k[ 1 ] ) >= int.Parse( l[ 1 ] )
				&& int.Parse( k[ 0 ] ) >= int.Parse( l[ 0 ] )
				 ) return true;
			return false;
		}

		internal static string Md5( string str )
		{
			return Md5( CryptographicBuffer.ConvertStringToBinary( str, BinaryStringEncoding.Utf8 ) );
		}

		public static int Md5Int( string str )
		{
			IBuffer Buff = CryptographicBuffer.ConvertStringToBinary( str, BinaryStringEncoding.Utf8 );
			HashAlgorithmProvider alg = HashAlgorithmProvider.OpenAlgorithm( HashAlgorithmNames.Md5 );
			byte[] bytes = alg.HashData( Buff ).ToArray();
			return bytes[ 0 ] << 24 | bytes[ 1 ] << 16 | bytes[ 2 ] << 8 | bytes[ 0 ];
		}

		internal static string Md5( IBuffer Buff )
		{
			HashAlgorithmProvider alg = HashAlgorithmProvider.OpenAlgorithm( HashAlgorithmNames.Md5 );
			return CryptographicBuffer.EncodeToHexString( alg.HashData( Buff ) );
		}

		internal async static Task<string> Sha1( IStorageFile File )
		{
			HashAlgorithmProvider alg = HashAlgorithmProvider.OpenAlgorithm( HashAlgorithmNames.Sha1 );

			CryptographicHash hash = alg.CreateHash();
			BasicProperties Prop = await File.GetBasicPropertiesAsync();

			IBuffer buff = new Windows.Storage.Streams.Buffer( 1048576 );
			IRandomAccessStream rStream = await File.OpenAsync( FileAccessMode.Read );

			await rStream.ReadAsync( buff, 1048576, InputStreamOptions.None );
			while ( 0 < buff.Length )
			{
				hash.Append( buff );
				await rStream.ReadAsync( buff, 1048576, InputStreamOptions.None );
			}

			return CryptographicBuffer.EncodeToHexString( hash.GetValueAndReset() );
		}

		private static readonly char[] BaseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

		public static string Base62( int value )
		{
			char[] buffer = new char[ Math.Max( ( int ) Math.Ceiling( Math.Log( value + 1, 62 ) ), 1 ) ];

			int i = buffer.Length;
			do
			{
				buffer[ --i ] = BaseChars[ value % 62 ];
				value = value / 62;
			}
			while ( value > 0 );
			return new string( buffer );
		}

		private static string DefaultMimeType = "application/octet-stream";

		private static readonly byte[] BOM_BMP = Encoding.ASCII.GetBytes( "BM" );
		private static readonly byte[] BOM_GIF = Encoding.ASCII.GetBytes( "GIF" );
		private static readonly byte[] BOM_PNG = new byte[] { 137, 80, 78, 71 };
		private static readonly byte[] BOM_JPG = new byte[] { 255, 216, 255, 224 };
		private static readonly byte[] BOM_JPG2 = new byte[] { 255, 216, 255, 225 };

		public static string GetMimeFromBytes( byte[] buffer )
		{
			if ( BOM_BMP.SequenceEqual( buffer.Take( BOM_BMP.Length ) ) )
				return "image/bmp";

			if ( BOM_GIF.SequenceEqual( buffer.Take( BOM_GIF.Length ) ) )
				return "image/gif";

			if ( BOM_PNG.SequenceEqual( buffer.Take( BOM_PNG.Length ) ) )
				return "image/png";

			if ( BOM_JPG.SequenceEqual( buffer.Take( BOM_JPG.Length ) ) )
				return "image/jpeg";

			if ( BOM_JPG2.SequenceEqual( buffer.Take( BOM_JPG2.Length ) ) )
				return "image/pjpeg";

			return DefaultMimeType;
		}

		public static bool APIv4 = ApiInformation.IsApiContractPresent( "Windows.Foundation.UniversalApiContract", 4, 0 );
		public static bool APIv5 = ApiInformation.IsApiContractPresent( "Windows.Foundation.UniversalApiContract", 5, 0 );

		public static async Task RestartOrExit( string LaunchArgs = "-restart" )
		{
			if ( APIv5 )
			{
				AppRestartFailureReason Reason = await CoreApplication.RequestRestartAsync( LaunchArgs );
				if ( Reason == AppRestartFailureReason.NotInForeground || Reason == AppRestartFailureReason.Other )
				{
					CoreApplication.Exit();
				}
			}
			else
			{
				CoreApplication.Exit();
			}
		}

	}
}