using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualStudioVideo
{
    public partial class PlayerWindow : UserControl
    {
        public PlayerWindow()
        {
            InitializeComponent();
        }

        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            //IF THE USER RIGHT CLICKED THEN SHOW THE CONTEXT MENU
            if (e.nButton == 2)
            {
                Point mouseLocation = this.PointToScreen(new Point(e.fX, e.fY));
                contextMenuStrip1.Show(mouseLocation);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DISPLAY THE OPEN FILE DIALOG AND ALLOW THE USER TO SELECT A FILE
            if (openDialog.ShowDialog() != DialogResult.Cancel)
                axWindowsMediaPlayer1.URL = openDialog.FileName;
        }

        private void hideControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //REVERSE THE CHECK STATE ON THE MENU ITEM
            hideControlsToolStripMenuItem.Checked = !hideControlsToolStripMenuItem.Checked;

            //HIDE OR DISPLAY THE CONTROLS ACCORDING TO THE CHECK STATE
            if (hideControlsToolStripMenuItem.Checked)
                axWindowsMediaPlayer1.uiMode = "none";
            else
                axWindowsMediaPlayer1.uiMode = "full";
        }

        private void axWindowsMediaPlayer1_KeyPressEvent(object sender, AxWMPLib._WMPOCXEvents_KeyPressEvent e)
        {
            Char Key = Char.ToUpper((char)e.nKeyAscii);

            //CHECK TO SEE WHICH KEY THE USER HIT THEN ACT ACCORDINGLY
            switch (Key)
            {
                case 'C':
                    hideControlsToolStripMenuItem_Click(null, null);
                    break;
                case 'P':
                    PlayPause();
                    break;
                case 'O':
                    openToolStripMenuItem_Click(null, null);
                    break;
                case ' ':
                    PlayPause();
                    break;
            }
        }

        private void PlayPause()
        {
            //IF THE PLAYER IS PLAYING THEN PAUSE OTHERWISE PLAY
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            else
                axWindowsMediaPlayer1.Ctlcontrols.play();
        }
    }
}
