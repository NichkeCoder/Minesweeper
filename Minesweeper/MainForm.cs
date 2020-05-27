using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MainForm : Form
    {
        public int mineCount = 45;
        public int width = 25;
        public int height = 15;

        private List<List<FieldBox>> boxList = new List<List<FieldBox>>();
        private List<List<bool>> visited = new List<List<bool>>();
        public Dictionary<FieldBox, int> boxDict = new Dictionary<FieldBox, int>();

        public MainForm()
        {
            InitializeComponent();
            DrawTable();
            InitializeTable();
        }

        private void DrawTable()
        {
            for (int y = 0; y < height; y++)
            {
                List<FieldBox> temp = new List<FieldBox>();
                List<bool> tempbool = new List<bool>();
                for (int x = 0; x < width; x++)
                {
                    FieldBox box = new FieldBox(this, x * 25, y * 25);
                    
                    temp.Add(box);
                    boxDict[box] = y * width + x;
                    Controls.Add(box);
                    tempbool.Add(false);
                }

                boxList.Add(temp);
                visited.Add(tempbool);
            }

            this.Size = new Size(width * 25 + 16, height * 25 + 39);
        }

        public void InitializeTable()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    boxList[y][x].OK = true;
                    boxList[y][x].Text = "";
                    boxList[y][x].BackColor = Color.White;
                    boxList[y][x].ForeColor = Color.Black;
                    boxList[y][x].Selected = false;
                    boxList[y][x].Clickable = true;
                    boxList[y][x].Marked = false;
                    visited[y][x] = false;
                }
            }

            int counter = 0;
            Random rnd = new Random();
            while (counter < mineCount)
            {
                int x;
                int y;
                do
                {
                    x = rnd.Next(0, width);
                    y = rnd.Next(0, height);
                } while (!boxList[y][x].OK);
                
                boxList[y][x].OK = false;
                counter++;
            }
        }

        public int CountMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i < width && x + i >= 0 && y + j < height && y + j >= 0 && !boxList[y + j][x + i].OK)
                        count++;
                }
            }
            boxList[y][x].BackColor = Color.DarkGray;
            return count;
        }

        public void FloodFill(int x, int y)
        {
            if (y >= height || x >= width || y < 0 || x < 0 || visited[y][x]) return;

            if (CountMines(x, y) > 0)
            {
                boxList[y][x].Text = CountMines(x, y).ToString();
                return;
            }

            visited[y][x] = true;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    boxList[y][x].BackColor = Color.DarkGray;
                    FloodFill(x + j, y + i);
                }
            }
        }

        public void ShowMines()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!boxList[i][j].OK)
                    {
                        boxList[i][j].ForeColor = Color.Red;
                        boxList[i][j].Text = "X";
                    }
                }
            }
        }
    }
}
