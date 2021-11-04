using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_roguelike.Systems
{
    public partial class Checkpoint2 : Form
    {
        public Checkpoint2()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            CheckpointFinal end = new CheckpointFinal();
            end.Show();
        }
    }
}
