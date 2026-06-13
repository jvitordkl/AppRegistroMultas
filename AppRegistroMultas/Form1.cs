using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppRegistroMultas.Formulario;

namespace AppRegistroMultas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btCadastroVeiculo_Click(object sender, EventArgs e)
        {
            FormCadastro formVeiculo = new FormCadastro();
            formVeiculo.ShowDialog();
        }

        private void btCadastroMulta_Click(object sender, EventArgs e)
        {
            FormCadastroMulta formCadastroMulta = new FormCadastroMulta();
            formCadastroMulta.ShowDialog();
        }

        private void btConsulta_Click(object sender, EventArgs e)
        {
            FormConsultaVeiculo formConsultaVeiculo = new FormConsultaVeiculo();
            formConsultaVeiculo.ShowDialog();
        }

        private void btEditarVeiculo_Click(object sender, EventArgs e)
        {
            FormEditarVeiculo formEditarVeiculo = new FormEditarVeiculo();
            formEditarVeiculo.ShowDialog();
        }
    }
}
