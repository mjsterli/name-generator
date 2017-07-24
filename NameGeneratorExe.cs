using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Tyler.Sterling.NameGenerator
{
   public enum HookType
  {
      WH_JOURNALRECORD = 0,
      WH_JOURNALPLAYBACK = 1,
      WH_KEYBOARD = 2,
      WH_GETMESSAGE = 3,
      WH_CALLWNDPROC = 4,
      WH_CBT = 5,
      WH_SYSMSGFILTER = 6,
      WH_MOUSE = 7,
      WH_HARDWARE = 8,
      WH_DEBUG = 9,
      WH_SHELL = 10,
      WH_FOREGROUNDIDLE = 11,
      WH_CALLWNDPROCRET = 12,        
      WH_KEYBOARD_LL = 13,
      WH_MOUSE_LL = 14
  }

  public class NameGeneratorExe : Form
  {
    NotifyIcon  TrayIcon;
    ContextMenu TrayMenu;

    [STAThread]
    public static void Main( )
    {
      Application.Run( new NameGeneratorExe( ) );
    }

    public NameGeneratorExe( )
    {
      TrayMenu = new ContextMenu( );
      TrayMenu.MenuItems.Add( "Generate Name", GenerateName );
      TrayMenu.MenuItems.Add( "Exit", Exit );

      TrayIcon = new NotifyIcon( );
      TrayIcon.Text = "Name Generator";
      TrayIcon.Icon = new Icon( SystemIcons.Application, 40, 40 );
      TrayIcon.ContextMenu = TrayMenu;
      TrayIcon.Visible = true;

      Form f = new Form( );
      f.Visible = false;
      f.KeyPress += OnKeyPress;
      f.Focus( );
    }

    protected override void OnLoad( EventArgs e )
    {
      Visible       = false;
      ShowInTaskbar = false;

      base.OnLoad( e );
    }

    protected override void Dispose( bool disposing )
    {
      if( disposing )
        TrayIcon.Dispose( );

      base.Dispose( disposing );
    }

    protected static extern IntPtr SetWindowsHookEx( HookType code, 
    {
    }
    void OnKeyPress( object sender, KeyPressEventArgs e )
    {
      if( ( ( Control.ModifierKeys & Keys.Control ) == Keys.Control ) && e.KeyChar == 'Q' )
        GenerateName( this, e );

      base.OnKeyPress( e );
    }

    private void Exit( object sender, EventArgs e )
    {
      TrayIcon.Visible = false;
      Application.Exit( );
    }

    private void GenerateName( object sender, EventArgs e )
    {
      NameGenerator.Name name = NameGenerator.GetRandomName( );
      TrayIcon.BalloonTipText = string.Format( "{0} {1}. {2}", name.FirstName, name.MiddleInitial, name.LastName );
      TrayIcon.ShowBalloonTip( 10000 );
    }

  }
}

//http://alanbondo.wordpress.com/2008/06/22/creating-a-system-tray-app-with-c/