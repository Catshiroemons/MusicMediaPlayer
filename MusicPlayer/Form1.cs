using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            player.uiMode = "none";
            track_volume.Value = 90;
            lbl_titlesong.Visible = false;
            lbl_author.Visible = false;
            lbl_album.Visible = false;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            this.BackColor = Color.FromArgb(31, 77, 87);
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            player.settings.volume = track_volume.Value;
            lbl_volume.Text = track_volume.Value.ToString() + "%";
        }
        string[] paths, files;

        private void track_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.URL = paths[track_list.SelectedIndex];
            player.Ctlcontrols.play();
            try
            {
                var file = TagLib.File.Create(paths[track_list.SelectedIndex]);
                var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                pic_art.Image = Image.FromStream(new MemoryStream(bin));
                lbl_titlesong.Visible = true;
                lbl_titlesong.Text = (file.Tag.Title);
                lbl_author.Visible = true;
                lbl_author.Text = (file.Tag.FirstPerformer);
                lbl_album.Visible = true;
                lbl_album.Text = (file.Tag.Album);

            }
            catch { }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.play();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if(track_list.SelectedIndex<track_list.Items.Count-1)
            {
                track_list.SelectedIndex = track_list.SelectedIndex + 1;
            }
            else
            {
                string message = "This is the last song!";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons);
            }
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            if(track_list.SelectedIndex>0)
            {
                track_list.SelectedIndex = track_list.SelectedIndex - 1;
            }
            else
            {
                string message = "This is the first song!";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(player.playState==WMPLib.WMPPlayState.wmppsPlaying)
            {
                p_bar.Maximum = (int)player.Ctlcontrols.currentItem.duration;
                p_bar.Value = (int)player.Ctlcontrols.currentPosition;
                try
                {
                    lbl_track_start.Text = player.Ctlcontrols.currentPositionString;
                    lbl_track_end.Text = player.Ctlcontrols.currentItem.durationString.ToString();
                }
                catch
                {

                }
            }

            
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void p_bar_MouseDown(object sender, MouseEventArgs e)
        {
            player.Ctlcontrols.currentPosition = player.currentMedia.duration * e.X / p_bar.Width;
        }

        private void txt_search_MouseClick(object sender, MouseEventArgs e)
        {
            txt_search.Text = "";
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            int result = track_list.FindString(txt_search.Text);
            if (0<=result)
            {
                track_list.SelectedIndex = result;
            }
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if(ofd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                files = ofd.FileNames;
                //paths = ofd.FileNames;
                paths = (paths ?? Enumerable.Empty<string>()).Concat(ofd.FileNames).ToArray();
                for (int x = 0; x < files.Length; x++)
                {
                   // track_list.Items.Add(files[x]);
                   TagLib.File file = TagLib.File.Create(files[x]);
                    track_list.Items.Add(file.Tag.Title);
                }
            }
            
        }
    }
}
