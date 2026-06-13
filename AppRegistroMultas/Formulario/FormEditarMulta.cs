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
        List<Multa> listarMultas = new List<Multa>();
        int id = -1; // Variavel que será utilizada para apagar ou editar multa
        public FormEditarMulta()
        {
            InitializeComponent();
            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
            listaVeiculo = context.ListarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList(); // Insere os veiculos no ComboBox
            cbVeiculo.DisplayMember = "Modelo"; // Organiza os veiculos do ComboBox com base no modelo
            cbVeiculo.SelectedIndex = -1; // Limpa o comboBox o deixando vazio
        }
        int veiculoId;
        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        public void AtualizarMultas()
        {
            // Insere no DataGridView todas as multas do veiculo selecionado
            MultaContext contexto = new MultaContext();
            listarMultas = contexto.ListarMultas(veiculoId);
            dtTabela.DataSource = listarMultas.ToList();

            // Insere no ComboBox de multas opções das multas dos veiculo organizados pelo ID
            cbMulta.DataSource = listarMultas.ToList();
            cbMulta.DisplayMember = "Id";
            cbMulta.SelectedIndex = -1;
            txtDescricao.Clear();
            txtValor.Clear();
            id = -1; // Define que nenhuma multa foi selecionada
        }

        private void cbMulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            var linhaSelect = cbMulta.SelectedIndex; // Armazena qual multa foi selecionada no ComboBox
            if (linhaSelect > -1 && contExcMulta > 0)  // Valida se existe alguma multa no ComboBox e se foi selecionado alguma multa
            {
                MultaContext contexto = new MultaContext();
                var multaSelect = listarMultas[linhaSelect];
                txtDescricao.Text = multaSelect.Descricao;
                txtValor.Text = multaSelect.ValorMulta.ToString();
                id = multaSelect.Id; // Armazena o id da multa
            }
            contExcMulta++;
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
            if (id <= 0)
            {
                MessageBox.Show("Nenhuma multa selecionada!", "ADS-IFRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var escolha = MessageBox.Show("Realmente deseja apagar esse dado?", "ADS-IFRO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (escolha == DialogResult.Yes)
                {
                    MultaContext contexto = new MultaContext();
                    contexto.DeletarMulta(id);
                    AtualizarMultas();
                    cbMulta.SelectedIndex = -1;
                    txtDescricao.Clear();
                    txtValor.Clear();
                    id = -1; // Re-define multa para nenhuma seleiconada
                }
            }
        }
    }
}
