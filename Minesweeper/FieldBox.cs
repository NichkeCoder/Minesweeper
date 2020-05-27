using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
    public class FieldBox : Button
    {
        private MainForm mainForm;

        public bool OK { get; set; }
        public bool Selected { get; set; }
        public bool Clickable { get; set; }
        public bool Marked { get; set; }

        private void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Clickable)
                {
                    ((FieldBox)sender).ForeColor = Color.Black;
                    if (!((FieldBox)sender).OK)
                    {
                        mainForm.ShowMines();
                        MessageBox.Show("Opppps! You stepped on a mine!");
                        mainForm.InitializeTable();
                    }
                    else
                    {
                        int pos = mainForm.boxDict[(FieldBox)sender];
                        int x = pos % mainForm.width;
                        int y = pos / mainForm.width;
                        int surroundMines = mainForm.CountMines(x, y);
                        mainForm.FloodFill(x, y);

                        if (surroundMines != 0) ((FieldBox)sender).Text = surroundMines.ToString();
                        Selected = true;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!Selected)
                {
                    if (!Marked)
                    {
                        ((FieldBox)sender).Text = "+";
                        ((FieldBox)sender).ForeColor = Color.White;
                        ((FieldBox)sender).BackColor = Color.IndianRed;
                    }
                    else
                    {
                        ((FieldBox)sender).Text = "";
                        ((FieldBox)sender).BackColor = Color.White;
                    }
                    Marked = !Marked;
                    Clickable = !Clickable;
                }
            }
        }

        public FieldBox(MainForm mf, int x, int y)
        {
            mainForm = mf;
            UseCompatibleTextRendering = true;

            this.Location = new Point(x, y);
            this.Size = new Size(25, 25);
            this.MouseDown += OnClick;
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
}
