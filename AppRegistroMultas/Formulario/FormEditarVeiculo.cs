using AppRegistroMultas.Contexto;
using AppRegistroMultas.Models;
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
    public partial class FormEditarVeiculo : Form
    {
        int contExc = 0;
        List<Veiculo> listaVeiculo = new List<Veiculo>();
        public FormEditarVeiculo()
        {
            InitializeComponent();
            AtualizarPagina();
        }

        public void AtualizarPagina()
        {
            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
            listaVeiculo = context.ListarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList();
            cbVeiculo.DisplayMember = "Modelo";
            cbVeiculo.SelectedIndex = -1;
        }
        int id;
        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var linhaSelect = cbVeiculo.SelectedIndex;
            if (linhaSelect > -1 && contExc > 0)
            {
                var veiculoSelect = listaVeiculo[linhaSelect];
                txtModelo.Text = veiculoSelect.Modelo;
                txtMarca.Text = veiculoSelect.Marca;
                txtPlaca.Text = veiculoSelect.Placa;
                txtAno.Text = veiculoSelect.Ano;
                id = veiculoSelect.Id;
            }
            contExc++;
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            cbVeiculo.SelectedIndex = -1;
            txtModelo.Clear();
            txtPlaca.Clear();
            txtMarca.Clear();
            txtAno.Clear();
            id = 0;
            txtModelo.Select();
        }

        private void btApagar_Click(object sender, EventArgs e)
        {
            if(id<=0)
            {
                MessageBox.Show("Nenhum veículo selecionado!", "ADS-IFRO", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MultaContext contextoMulta = new MultaContext();
                List<Multa> multas = contextoMulta.ListarMultas(id).ToList();

                var escolha = MessageBox.Show("Realmente deseja apagar esse dado?","ADS-IFRO",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (escolha == DialogResult.Yes)
                {
                    if (multas.Count > 0)
                    {
                        MessageBox.Show("Existem multas pendentes referênte a esse veiculo!", "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        VeiculoContext contexto = new VeiculoContext();
                        contexto.DeletarVeiculo(id);
                        AtualizarPagina();
                        id = -1;
                        cbVeiculo.SelectedIndex = -1;
                        txtModelo.Clear();
                        txtPlaca.Clear();
                        txtMarca.Clear();
                        txtAno.Clear();
                        txtModelo.Select();
                    }
                }
            }
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            if((txtModelo.Text != "" && txtMarca.Text!= "" && txtPlaca.Text!="" && txtAno.Text != "") && cbVeiculo.SelectedIndex > 0)
            {
                Veiculo veiculo = listaVeiculo[cbVeiculo.SelectedIndex];
                if(veiculo.Modelo != txtModelo.Text.ToString() ||
                    veiculo.Marca != txtMarca.Text.ToString() ||
                    veiculo.Placa != txtPlaca.Text.ToString() ||
                    veiculo.Ano != txtAno.Text.ToString())
                {
                    var escolha = MessageBox.Show("Realmente Deseja atualizar esse cadastro?", "ADS-IFRO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(escolha == DialogResult.Yes)
                    {
                        veiculo.Modelo = txtModelo.Text.ToString();
                        veiculo.Marca = txtMarca.Text.ToString();
                        veiculo.Placa = txtPlaca.Text.ToString();
                        veiculo.Ano = txtAno.Text.ToString();
                        VeiculoContext contexto = new VeiculoContext();
                        contexto.AtualizarVeiculo(veiculo);
                        MessageBox.Show("Veiculo Atualizado com sucesso", "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        VeiculoContext context = new VeiculoContext(); // Preparou a conexao
                        listaVeiculo = context.ListarVeiculo(); // Conectou e buscou no banco
                    }
                    else
                    {
                        MessageBox.Show("Veiculo NÃO foi atualizado!", "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Veiculo existe", "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
