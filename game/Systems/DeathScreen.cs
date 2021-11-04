using System;
using System.Windows.Forms;
using test_roguelike.Core;

namespace test_roguelike.Systems
{
    public partial class DeathScreen : Form
    {
        public DeathScreen()
        {
            InitializeComponent();
        }

        private void DeathScreen_Load(object sender, EventArgs e)
        {
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            Game._rootConsole.Close();
            Game.Player = new Player();
            Game.Main();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Game._rootConsole.Close();
        }
    }
}
