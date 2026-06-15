using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppRegistroMultas.Models;
using AppRegistroMultas.Contexto;

namespace AppRegistroMultas.Formulario
{
    public partial class FormCadastro : Form
    {
        public FormCadastro()
        {
            InitializeComponent();
        }

        private void Limpar()
        {
            txtModelo.Clear();
            txtMarca.Clear();
            txtPlaca.Clear();
            txtAno.Clear();
            txtModelo.Select();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = "Faltou preencher:";
                if (txtModelo.Text == "")
                {
                    mensagem += "\nModelo";
                }
                if (txtMarca.Text == "")
                {
                    mensagem += "\nMarca";
                }
                if (txtPlaca.Text == "")
                {
                    mensagem += "\nPlaca";
                }
                if (txtAno.Text == "")
                {
                    mensagem += "\nAno";
                }
                if(mensagem == "Faltou preencher:")
                {
                    Veiculo veiculo = new Veiculo();
                    veiculo.Modelo = txtModelo.Text.ToString();
                    veiculo.Marca = txtMarca.Text.ToString();
                    veiculo.Placa = txtPlaca.Text.ToString();
                    veiculo.Ano = txtAno.Text.ToString();

                    VeiculoContext contexto = new VeiculoContext();
                    contexto.InserirVeiculo(veiculo);
                    Limpar();
                    MessageBox.Show("Veiculo cadastrado com sucesso!", "ADS-IFRO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(mensagem, "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception Ex)
            {

            }
        }
    }
}
