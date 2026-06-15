using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppRegistroMultas.Models;
using AppRegistroMultas.Contexto;

namespace AppRegistroMultas.Formulario
{
    public partial class FormCadastroMulta : Form
    {
        int contExc = 0;
        List<Veiculo> listaVeiculo = new List<Veiculo>();
        public FormCadastroMulta()
        {
            InitializeComponent();
            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
            listaVeiculo = context.ConsultarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList();
            cbVeiculo.DisplayMember = "Modelo";
            cbVeiculo.SelectedIndex = -1;
            txtModelo.ReadOnly = true;
            txtMarca.ReadOnly = true;
            txtPlaca.ReadOnly = true;
            txtAno.ReadOnly = true;
        }
        public void Disponivel()
        {
            if (cbVeiculo.SelectedIndex >= 0)
            {
                txtDescricao.ReadOnly = false;
                txtValor.ReadOnly = false;
            }
            else
            {
                txtDescricao.ReadOnly = true;
                txtValor.ReadOnly = true;
            }
        }
        private int id;
        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Disponivel();
                var linhaSelect = cbVeiculo.SelectedIndex;
                if (linhaSelect > -1 && contExc > 0)
                {
                    var veiculoSelect = listaVeiculo[linhaSelect];
                    txtModelo.Text = veiculoSelect.Modelo;
                    txtMarca.Text = veiculoSelect.Marca;
                    txtPlaca.Text = veiculoSelect.Placa;
                    txtAno.Text = veiculoSelect.Ano;
                    id = veiculoSelect.Id;
                    Limpar();
                    dtTabela.DataSource = null;
                    listaMultas.Clear();
                }
                contExc++;
            }
            catch(Exception Ex)
            {

            }
        }

        List<Multa> listaMultas = new List<Multa>();

        private void Limpar()
        {
            txtDescricao.Clear();
            txtValor.Clear();
            txtDescricao.Select();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtValor.Text != "" && txtDescricao.Text != "" && id != 0)
                {
                    Multa multa = new Multa();
                    multa.Descricao = txtDescricao.Text.ToString();
                    multa.ValorMulta = Convert.ToDecimal(txtValor.Text);
                    multa.VeiculoId = Convert.ToInt32(id);
                    listaMultas.Add(multa);
                    dtTabela.DataSource = listaMultas.ToList();
                    Limpar();
                }
                else
                    if(!(id != 0))
                {
                    MessageBox.Show("Selecione um veiculo!", "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    if(txtValor.Text == "" || txtDescricao.Text == "")
                {
                    string mensagem = "Falta preencher:";
                    if (txtValor.Text == "")
                    {
                        mensagem += "\nValor";
                    }
                    if(txtDescricao.Text == "")
                    {
                        mensagem += "\nDescrição";
                    }
                    MessageBox.Show(mensagem, "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch(Exception Ex)
            {

            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
            listaMultas.Clear();
            dtTabela.DataSource = null;
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    MessageBox.Show("Selecione um veiculo!", "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    if (listaMultas.Count == 0)
                {
                    MessageBox.Show("Nenhuma multa adicionada!", "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MultaContext contexto = new MultaContext();
                    foreach (Multa multa in listaMultas)
                    {
                        contexto.InserirMulta(multa);
                    }
                    listaMultas.Clear();
                    dtTabela.DataSource = listaMultas.ToList();
                    MessageBox.Show("Multas adicionadas com sucesso!", "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception Ex)
            {

            }
        }
    }
}
