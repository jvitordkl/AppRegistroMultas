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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AppRegistroMultas.Models;
using AppRegistroMultas.Contexto;

namespace AppRegistroMultas.Formulario
{
    public partial class FormConsultaVeiculo : Form
    {
        int contExc = 0;
        List<Veiculo> listaVeiculo = new List<Veiculo>();
        public FormConsultaVeiculo()
        {
            InitializeComponent();
            VeiculoContext context = new VeiculoContext(); // Preparou a conexao
            listaVeiculo = context.ListarVeiculo(); // Conectou e buscou no banco
            cbVeiculo.DataSource = listaVeiculo.ToList();
            cbVeiculo.DisplayMember = "Modelo";
            cbVeiculo.SelectedIndex = -1;
        }

        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var linhaSelect = cbVeiculo.SelectedIndex;
            if (linhaSelect > -1 && contExc > 0)
            {
                MultaContext contexto = new MultaContext();
                List<Multa> listarMultas = new List<Multa>();
                var veiculoSelect = listaVeiculo[linhaSelect];
                txtModelo.Text = veiculoSelect.Modelo;
                txtMarca.Text = veiculoSelect.Marca;
                txtPlaca.Text = veiculoSelect.Placa;
                txtAno.Text = veiculoSelect.Ano;
                listarMultas = contexto.ListarMultas(veiculoSelect.Id);
                dtTabela.DataSource = listarMultas.ToList();
            }
            contExc++;
        }
    }
}
