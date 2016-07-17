﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wenku8.Model.REST
{
    using AdvDM;
    using Config;

    sealed class SharersRequest
    {
        public string ACCESS_TOKEN = "";
        public string SECRET = "";

        public Uri Server = new Uri( "http://w10srv.astropenguin.net/" );

        private readonly string LANG = Properties.LANGUAGE;

        public enum StatusType
        {
            INVALID_SCRIPT = -1
        }

        public PostData ReserveId()
        {
            return new PostData(
                "SHHUB_RESERVE_UUID"
                , Compost(
                    "action", "reserve-uuid"
                    , "access_token", ACCESS_TOKEN
                )
            );
        }

        public PostData ScriptUpload( string Id, string ScriptData, string Name, string Zone, string[] Types, string[] Tags = null )
        {
            List<string> Params = new List<string>( new string[] {
                "action", "upload"
                , "uuid", Id
                , "secret", SECRET
                , "data", ScriptData
                , "name", Name
                , "access_token", ACCESS_TOKEN
            } );

            ZoneTypeTags( Params, new string[] { Zone }, Types, Tags );

            return new PostData( Id, Compost( Params.ToArray() ) );
        }

        public PostData ScriptDownload( string Id )
        {
            return new PostData(
                Id, Compost(
                    "action", "download"
                    , "uuid", Id
                    , "access_token", ACCESS_TOKEN
                )
            );
        }

        public PostData StatusReport( string Id, string StatusType, string Desc = "" )
        {
            return new PostData(
                Id, Compost(
                    "action", "status-report"
                    , "uuid", Id
                    , "type", StatusType
                    , "desc", Desc
                )
            );
        }


        public PostData ScriptRemove( string Id )
        {
            return new PostData(
                Id, Compost(
                    "action", "remove"
                    , "uuid", Id
                    , "access_token", ACCESS_TOKEN
                )
            );
        }

        public PostData Register( string Username, string Passwd, string Email )
        {
            return new PostData(
                "SHHUB_REGISTER"
                , Compost(
                    "action", "register"
                    , "user", Username
                    , "passwd", Passwd
                    , "email", Email
                )
            );
        }

        public PostData Login( string Username, string Passwd )
        {
            return new PostData(
                "SHHUB_LOGIN"
                , Compost(
                    "action", "login"
                    , "username", Username
                    , "passwd", Passwd
                )
            );
        }

        public PostData Logout()
        {
            return new PostData( "SHHUB_LOGOUT", Compost( "action", "logout" ) );
        }

        public PostData Search( string Query )
        {
            /**
             * Here is how the query is parsed
             * Split ':' into groups
             *   <-: is the property to filter
             *   :-> is the filter value
             **/

            string[] QString = Query.Split( ':' );
            int l = QString.Length - 1;

            // Default searches script name
            if ( l < 1 ) return new PostData( "SHHUB_SEARCH", Compost( "action", "search", "name", Query ) );

            List<string> Queries = new List<string>( new string[] { "action", "search" } );

            for ( int i = 0; i < l; i++ )
            {
                string QName = QString[ i ];

                string PropFilter = QName.Substring( QName.LastIndexOf( ' ' ) + 1 );
                string FilterVal = QString[ i + 1 ].Trim();

                if ( i + 2 <= l )
                {
                    FilterVal = FilterVal.Substring( 0, FilterVal.LastIndexOf( ' ' ) );
                }
                switch ( PropFilter )
                {
                    case "tag":
                        PropFilter += "s";
                        break;
                    case "zones":
                    case "types":
                        PropFilter = PropFilter.Substring( 0, PropFilter.Length - 1 );
                        break;
                }

                Queries.Add( PropFilter );
                Queries.Add( FilterVal );
            }

            if ( 2 < Queries.Count() )
            {
                int NameIndex = QString[ 0 ].LastIndexOf( ' ' );
                if ( ~NameIndex != 0 )
                {
                    Queries.Add( "name" );
                    Queries.Add( QString[ 0 ].Substring( 0, NameIndex ).Trim() );
                }
            }

            return new PostData( "SHHUB_SEARCH", Compost( Queries.ToArray() ) );
        }

        private string Compost( params string[] Pairs )
        {
            int l = Pairs.Length;
#if DEBUG
            if ( l % 2 != 0 || l == 0 )
            {
                throw new ArgumentException( "Arguments does not seems to be in pairs" );
            }
#endif
            string Composted = "lang=" + LANG + "&t=" + DateTime.UtcNow.Ticks.ToString();

            for ( int i = 0; i < l; i++ )
            {
                Composted += ( i % 2 == 0 ? "&" : "=" ) + Uri.EscapeDataString( Pairs[ i ] );
            }

            return Composted;
        }

        private void ZoneTypeTags( IList<string> Params, string[] Zones, string[] Types, string[] Tags )
        {
            if( Zones != null )
            foreach ( string Zone in Zones )
            {
                Params.Add( "zone" );
                Params.Add( Zone );
            }

            if( Types != null )
            foreach ( string Type in Types )
            {
                Params.Add( "type" );
                Params.Add( Type );
            }

            if( Tags != null )
            foreach ( string Tag in Tags )
            {
                Params.Add( "tag" );
                Params.Add( Tag );
            }
        }

    }
}