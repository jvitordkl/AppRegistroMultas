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

namespace AppRegistroMultas.Formulario
{
    public partial class FormEditarMulta : Form
    {
        int contExc = 0;
        int contExcMulta = -1;
        List<Veiculo> listaVeiculo = new List<Veiculo>();
        List<Multa> listaMulta = new List<Multa>();
        int id = -1; // Variavel que será utilizada para apagar ou editar multa
        public FormEditarMulta()
        {
            InitializeComponent();
            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
            listaVeiculo = context.ConsultarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList(); // Insere os veiculos no ComboBox
            cbVeiculo.DisplayMember = "Modelo"; // Organiza os veiculos do ComboBox com base no modelo
            cbVeiculo.SelectedIndex = -1; // Limpa o comboBox o deixando vazio
            txtModelo.ReadOnly = true;
            txtMarca.ReadOnly = true;
            txtPlaca.ReadOnly = true;
            txtAno.ReadOnly = true;
        }

        private void Disponivel()
        {
            if(cbVeiculo.SelectedIndex >= 0)
            {
                cbMulta.Enabled = true;
            }
            else
            {
                cbMulta.Enabled = false;
            }
            if(cbMulta.SelectedIndex >= 0)
            {
                txtValor.ReadOnly = false;
                txtDescricao.ReadOnly = false;
            }
            else
            {
                txtValor.ReadOnly = true;
                txtDescricao.ReadOnly = true;
            }
        }


        int veiculoId;
        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Disponivel();
                var linhaSelect = cbVeiculo.SelectedIndex; // Armazena qual veiculo foi selecionado
                if (linhaSelect > -1 && contExc > 0) // Valida se existe algum veiculo no ComboBox e se foi selecionado algum veiculo
                {
                    // Insere os dados do veiculo selecionado nos TextBox
                    var veiculoSelect = listaVeiculo[linhaSelect];
                    txtModelo.Text = veiculoSelect.Modelo;
                    txtMarca.Text = veiculoSelect.Marca;
                    txtPlaca.Text = veiculoSelect.Placa;
                    txtAno.Text = veiculoSelect.Ano;
                    veiculoId = veiculoSelect.Id;
                    AtualizarMultas();
                }
                contExc++;
            }
            catch(Exception ex)
            {

            }
        }

        public void AtualizarMultas()
        {
            try
            {
                // Insere no DataGridView todas as multas do veiculo selecionado
                MultaContext contexto = new MultaContext();
                listaMulta = contexto.ConsultarMultas(veiculoId);
                dtTabela.DataSource = listaMulta.ToList();

                // Insere no ComboBox de multas opções das multas dos veiculo organizados pelo ID
                cbMulta.DataSource = listaMulta.ToList();
                cbMulta.DisplayMember = "Id";
                cbMulta.SelectedIndex = -1;
                txtDescricao.Clear();
                txtValor.Clear();
                id = -1; // Define que nenhuma multa foi selecionada
            }
            catch(Exception Ex)
            {

            }
        }

        private void cbMulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Disponivel();
                var linhaSelect = cbMulta.SelectedIndex; // Armazena qual multa foi selecionada no ComboBox
                if (linhaSelect > -1 && contExcMulta > 0)  // Valida se existe alguma multa no ComboBox e se foi selecionado alguma multa
                {
                    MultaContext contexto = new MultaContext();
                    var multaSelect = listaMulta[linhaSelect];
                    txtDescricao.Text = multaSelect.Descricao;
                    txtValor.Text = multaSelect.ValorMulta.ToString();
                    id = multaSelect.Id; // Armazena o id da multa
                }
                contExcMulta++;
            }
            catch(Exception Ex){ }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            cbMulta.SelectedIndex = -1;
            txtDescricao.Clear();
            txtValor.Clear();
            id = -1; // Define que nenhuma multa foi selecionada
        }

        
        private void btApagar_Click(object sender, EventArgs e)
        {
            try
            {
                if(cbVeiculo.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione um veiculo!",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    if(cbMulta.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione uma multa!",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var escolha = MessageBox.Show("Realmente deseja apagar esse dado?", "ADS-IFRO",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (escolha == DialogResult.Yes)
                    {
                        MultaContext contexto = new MultaContext();
                        contexto.DeletarMulta(id);
                        AtualizarMultas();
                        cbMulta.SelectedIndex = -1;
                        txtDescricao.Clear();
                        txtValor.Clear();
                        id = -1; // Re-define multa para nenhuma seleiconada
                        MessageBox.Show("Multa apagada com sucesso!", "ADS-IFRO",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Multa não foi apagada!", "ADS-IFRO",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (cbVeiculo.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione um veiculo!",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    if(cbMulta.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione uma multa!",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    if (txtValor.Text != "" && txtDescricao.Text != "")
                {
                    int index = cbMulta.SelectedIndex;
                    Multa multa = listaMulta[index];
                    if (multa.Descricao != txtDescricao.Text || multa.ValorMulta != Convert.ToDecimal(txtValor.Text))
                    {
                        multa.ValorMulta = Convert.ToDecimal(txtValor.Text);
                        multa.Descricao = txtDescricao.Text.ToString();
                        var resposta = MessageBox.Show("Tem certeza que deseja atulizar essa multa?",
                            "ADS-IFRO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resposta == DialogResult.Yes)
                        {
                            MultaContext contexto = new MultaContext();
                            contexto.AtualizarMulta(multa);
                            MessageBox.Show("Multa atualizada com sucesso",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizarMultas();
                            cbMulta.SelectedIndex = index;
                        }
                        else
                        {
                            MessageBox.Show("Multa não foi atualizada",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nada foi editado!",
                            "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch(Exception Ex)
            {

            }
        }
    }
}
