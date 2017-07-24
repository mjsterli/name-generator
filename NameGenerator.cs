using System;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;
using System.Collections.Generic;

namespace Tyler.Sterling.NameGenerator
{
  public static class NameGenerator
  {
    public struct Name
    {
      public string FirstName
      {
        get;
        set;
      }

      public string MiddleInitial
      {
        get;
        set;
      }

      public string LastName
      {
        get;
        set;
      }
    }

    static string DBName = "RandomNames.mdb";
    static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=./RandomNames.mdb;";
    static List<Name> Names;

    static NameGenerator( )
    {
      if( File.Exists( DBName ) )
      {
        Names = new List<Name>( );
        using( OleDbConnection connection = new OleDbConnection( ConnectionString ) )
        {
          DataSet nameSet = new DataSet( );
          OleDbDataAdapter adapter = new OleDbDataAdapter( );
          adapter.SelectCommand = new OleDbCommand( "SELECT * FROM RandomNames", connection );
          adapter.Fill( nameSet, "Names" );

          LoadNames( nameSet );
        }

        Debug.WriteLine( "Connection Closed..." );
      }
    }

    static void LoadNames( DataSet names )
    {
      foreach( DataRow row in names.Tables["Names"].Rows )
      {
        Name name           = new Name( );
        name.FirstName      = row["firstName"].ToString( );
        name.MiddleInitial  = row["middleInitial"].ToString();
        name.LastName       = row["lastName"].ToString( );

        Names.Add( name );
      }
    }

    public static Name GetRandomName( )
    {
      Random rand = new Random( DateTime.Now.Millisecond );
      return Names[ rand.Next( 0, Names.Count )];
    }
  }
}