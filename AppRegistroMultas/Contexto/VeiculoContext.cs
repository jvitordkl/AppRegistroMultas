using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //para ter acesso ao MessageBox
using MySql.Data.MySqlClient;//Para trabalhar com as classes de conexão com o banco dados Mysql
using AppRegistroMultas.Models; //Para ter acesso as classes dentro da pasta Models



namespace AppRegistroMultas.Contexto
{
    public class VeiculoContext
    {

        private string dados_conexao;
        private MySqlConnection conexao = null;

        // Método construtor para carregar as informações dentro do objeto "conexao" para conectar com o banco MySql
        public VeiculoContext()
        {
            dados_conexao = "server=localhost;port=3306;database=bdmultas;user=root;password=root;Persist Security Info = False; Connect Timeout=300;";
            conexao = new MySqlConnection(dados_conexao);
        }// Fim do construtor

        public List<Veiculo> ConsultarVeiculo() // Método para consultar veiculos
        {
            List<Veiculo> listaDeVeiculosParaExportar = new List<Veiculo>(); // Para retornar os veiculos consultados
            string sql = "SELECT * FROM veiculo"; // Comando de consulta no MySql

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Reponsável pela consulta no MySql
                conexao.Open(); // Abre uma conexao com o MySql
                MySqlDataReader dados = comando.ExecuteReader(); // Realiza a consulta e armazena no objeto dados

                // Laço responsável por percorrer todos os registros consultados no MySql
                while (dados.Read())
                {
                    Veiculo veiculo = new Veiculo();
                    veiculo.Id = Convert.ToInt32(dados["id"]);
                    veiculo.Modelo = dados["modelo"].ToString();
                    veiculo.Marca = dados["marca"].ToString();
                    veiculo.Placa = dados["placa"].ToString();
                    veiculo.Ano = dados["ano"].ToString();
                    listaDeVeiculosParaExportar.Add(veiculo);
                }
                conexao.Close(); // Fecha conexao
            }
            catch (Exception Ex) // Executa caso algum erro aconteça
            {
                MessageBox.Show("Erro ao consultar veiculos!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao consultar veiculos");
            }
            return listaDeVeiculosParaExportar; // Retornando a lista dos veiculos consultados
        }// Fim do método de consultar veiculos

        public void InserirVeiculo(Veiculo veiculo)
        {
            // Comandos para inserir o veiculo no MySql
            string sql = "Insert INTO veiculo (modelo,marca,placa,ano) VALUES (@modelo,@marca,@placa, @ano)";

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Comando que vai inserir o veiculo no MYSQL

                // Adicionando parametros para evitar SQL Injection
                comando.Parameters.AddWithValue("@modelo", veiculo.Modelo);
                comando.Parameters.AddWithValue("@marca", veiculo.Marca);
                comando.Parameters.AddWithValue("@placa", veiculo.Placa);
                comando.Parameters.AddWithValue("@ano", veiculo.Ano);

                conexao.Open(); // Abre a conexao com o MySql
                int linhasAfetadas = comando.ExecuteNonQuery(); // Executa e mostra as linhas que foram afetadas
            }
            catch (Exception Ex) // Executa caso algum erro aconteça
            {
                MessageBox.Show("Erro ao cadastrar o veiculo!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao inserir veiculo");
            }
            finally // Executa independente se der erro ou não
            {
                conexao.Close(); // Fecha a conexao
            }
        }// Fim do método de inserir veiculo

        public void DeletarVeiculo(int id) // Metodo para deletar veiculo
        {
            // Comandos para deletar o veiculo no MySql
            string sql = "DELETE FROM veiculo where id = @id";

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Comando que vai deletar o veiculo no MYSQL
                // Adicionando parametros para evitar SQL Injection
                comando.Parameters.AddWithValue("@id", id);
                conexao.Open(); // Abre a conexao com o MySql
                int linhasAfetadas = comando.ExecuteNonQuery(); // Executa e deleta o cadastro
            }
            catch (Exception Ex) // Informa caso algum erro aconteça
            {
                MessageBox.Show("Erro ao deletar o veiculo!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao deletar o veiculo!");
            }
            finally // Executa independente se der erro ou não
            {
                conexao.Close(); // Fecha a conexao
            }
        }

        public void AtualizarVeiculo(Veiculo veiculo) // Metodo para atualizar veiculo
        {
            // Comandos para atualizar o veiculo no MySql
            string sql = "UPDATE veiculo SET MODELO = @modelo,MARCA=@marca,PLACA=@placa,ANO=@ano WHERE ID=@id";

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Comando que vai atualizar o veiculo no MYSQL

                // Adicionando parametros para evitar SQL Injection
                comando.Parameters.AddWithValue("@modelo", veiculo.Modelo);
                comando.Parameters.AddWithValue("@marca", veiculo.Marca);
                comando.Parameters.AddWithValue("@placa", veiculo.Placa);
                comando.Parameters.AddWithValue("@ano", veiculo.Ano);
                comando.Parameters.AddWithValue("@id", veiculo.Id);

                conexao.Open(); // Abre a conexao com o MySql
                int linhasAfetadas = comando.ExecuteNonQuery(); // Executa e atualiza o veiculo
            }
            catch (Exception Ex) // Informa caso algum erro acontece
            {
                MessageBox.Show("Erro ao cadastrar o veiculo!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao atualizar o veiculo!");
            }
            finally // Executa independente se ter erro ou não
            {
                conexao.Close(); // Fecha a conexao
            }
        }
    }//fim  da classe

} //fim do nameSpace

