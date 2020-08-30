using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.MdiParent = Program.mainForm;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UsuarioBLL usuario = new UsuarioBLL();

            if(usuario.Login(txtUsuario.Text, txtPass.Text))
            {
                MessageBox.Show("logueado");
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            new FormRegistro().Show();
        }
    }
}
