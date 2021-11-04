using System;
using System.Windows.Forms;
using test_roguelike.Core;

namespace test_roguelike.Systems
{
    public partial class CheckpointFinal : Form
    {
        public CheckpointFinal()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Game._rootConsole.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            Game._rootConsole.Close();
            Game.Player = new Player();
            Game.Main();
        }
    }
}
