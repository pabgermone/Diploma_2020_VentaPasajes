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
    public partial class FormRegistro : Form
    {
        public FormRegistro()
        {
            InitializeComponent();
        }

        private void FormRegistro_Load(object sender, EventArgs e)
        {
            this.MdiParent = Program.mainForm;
            
            foreach(RolBLL rol in RolBLL.ListarRoles())
            {
                comboRol.Items.Add(rol);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text.Length != 0 && txtApellido.Text.Length != 0 && txtMail.Text.Length != 0 && txtUsuario.Text.Length != 0 && txtPass.Text.Length >= 8 && comboRol.SelectedItem != null)
            {
                UsuarioBLL usuario = new UsuarioBLL();

                usuario.Registro(txtNombre.Text, txtApellido.Text, txtMail.Text, txtUsuario.Text, txtPass.Text, (RolBLL)comboRol.SelectedItem);

                MessageBox.Show("Usuario registrado");

                this.Close();
            }
            else
            {
                MessageBox.Show("Falta completar algún campo o seleccionar un rol");
            }
        }
    }
}
