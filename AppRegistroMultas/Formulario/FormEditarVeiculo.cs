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
            listaVeiculo = context.ConsultarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList();
            cbVeiculo.DisplayMember = "Modelo";
            cbVeiculo.SelectedIndex = -1;
            Disponivel();
        }

        public void Disponivel()
        {
            if (cbVeiculo.SelectedIndex >= 0)
            {
                txtModelo.ReadOnly = false;
                txtMarca.ReadOnly = false;
                txtPlaca.ReadOnly = false;
                txtAno.ReadOnly = false;
            }
            else
            {
                txtModelo.ReadOnly = true;
                txtMarca.ReadOnly = true;
                txtPlaca.ReadOnly = true;
                txtAno.ReadOnly = true;
            }
        }
        int id;
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
                }
                contExc++;
            }
            catch(Exception Ex)
            {
                
            }
            
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            cbVeiculo.SelectedIndex = -1;
            txtModelo.Clear();
            txtPlaca.Clear();
            txtMarca.Clear();
            txtAno.Clear();
            id = -1;
            txtModelo.Select();
        }

        private void btApagar_Click(object sender, EventArgs e)
        {
            try
            {
                if (id <= 0)
                {
                    MessageBox.Show("Selecione um veículo!", "ADS-IFRO",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MultaContext contextoMulta = new MultaContext();
                    List<Multa> multas = contextoMulta.ConsultarMultas(id).ToList();
                    if (multas.Count > 0)
                    {
                        MessageBox.Show("Existem multas pendentes referênte a esse veiculo!", "ADS-IFRO",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var escolha = MessageBox.Show("Realmente deseja apagar esse dado?", "ADS-IFRO",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (escolha == DialogResult.Yes)
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
                            MessageBox.Show("Veiculo apagado com sucesso!", "ADS-IFRO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Veiculo não foi apagado!", "ADS-IFRO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                        
                }
            }
            catch(Exception Ex)
            {

            }
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtModelo.Text != "" && txtMarca.Text != "" && txtPlaca.Text != "" && txtAno.Text != "") && cbVeiculo.SelectedIndex >= 0)
                {
                    Veiculo veiculo = listaVeiculo[cbVeiculo.SelectedIndex];
                    if (veiculo.Modelo != txtModelo.Text.ToString() ||
                        veiculo.Marca != txtMarca.Text.ToString() ||
                        veiculo.Placa != txtPlaca.Text.ToString() ||
                        veiculo.Ano != txtAno.Text.ToString())
                    {
                        var escolha = MessageBox.Show("Realmente Deseja atualizar esse cadastro?", "ADS-IFRO",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (escolha == DialogResult.Yes)
                        {
                            veiculo.Modelo = txtModelo.Text.ToString();
                            veiculo.Marca = txtMarca.Text.ToString();
                            veiculo.Placa = txtPlaca.Text.ToString();
                            veiculo.Ano = txtAno.Text.ToString();
                            int index = cbVeiculo.SelectedIndex;
                            VeiculoContext contexto = new VeiculoContext();
                            contexto.AtualizarVeiculo(veiculo);
                            MessageBox.Show("Veiculo atualizado com sucesso", "ADS-IFRO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
                            listaVeiculo = context.ConsultarVeiculo(); // Conectou e buscou no banco

                            cbVeiculo.DataSource = listaVeiculo.ToList();
                            cbVeiculo.DisplayMember = "Modelo";
                            cbVeiculo.SelectedIndex = index;
                        }
                        else
                        {
                            MessageBox.Show("Veiculo não foi atualizado!", "ADS-IFRO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nada foi editado", "ADS-IFRO",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    if(cbVeiculo.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione um veiculo!", "ADS-IFRO",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception Ex)
            {

            }
            
        }
    }
}
