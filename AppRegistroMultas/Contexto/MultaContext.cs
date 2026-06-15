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
    public class MultaContext
    {

        private string dados_conexao;
        private MySqlConnection conexao = null;

        // Método construtor para carregar as informações dentro do objeto conexao
        public MultaContext()
        {
            dados_conexao = "server=localhost;port=3306;database=bdmultas;user=root;password=root;Persist Security Info = False;Connect Timeout=300;";
            conexao = new MySqlConnection(dados_conexao);
        }// Fim do método construtor

        public List<Multa> ConsultarMultas(int id) // Método de consultar multas
        {
            List<Multa> listaDeMultasParaExportar = new List<Multa>(); // Lista onde será armazenada as multas consultadas
            string sql = "SELECT * FROM multa where veiculoId = @id"; // Comando SQL de consulta baseado no id do veiculo
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Quem realizará a consulta
                comando.Parameters.AddWithValue("@id", id); // Adicionando parametro para evitar SQL injection
                conexao.Open(); // Abre a conexao
                MySqlDataReader dados = comando.ExecuteReader(); // Executa o comando SQL no banco e armazena no objeto dados

                // loop que converte os dados para que o C# aceite e adiciona na lista
                while (dados.Read()) 
                {
                    Multa multa = new Multa();
                    multa.Id = Convert.ToInt32(dados["id"]);
                    multa.Descricao = dados["descricao"].ToString();
                    multa.ValorMulta = Convert.ToDecimal(dados["valorMulta"].ToString());
                    multa.VeiculoId = Convert.ToInt32(dados["veiculoID"].ToString());
                    listaDeMultasParaExportar.Add(multa);
                }

                conexao.Close(); // Fecha a conexao
            }
            catch (Exception Ex) // Informa caso aconteça algum erro durante a consulta
            {
                MessageBox.Show("Erro ao realizar a consulta!","ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao consultar as multas!");
            }
            return listaDeMultasParaExportar; // Retorna a lista para o formulário que está requisitando
        } // fim do método de consultar multas

        public void InserirMulta(Multa multa) // Método para inserir multa
        {
            // Comando SQL de inserir multa
            string sql = "INSERT INTO multa (descricao,valorMulta,veiculoID) VALUES (@descricao,@valorMulta,@veiculoID)";
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Quem irá inserir a multa

                // Adicionando parametros para impedir SQL Injection
                comando.Parameters.AddWithValue("@descricao", multa.Descricao);
                comando.Parameters.AddWithValue("@valorMulta", multa.ValorMulta);
                comando.Parameters.AddWithValue("@veiculoId", multa.VeiculoId);

                conexao.Open(); // Abre a conexao
                int linhasAfetadas = comando.ExecuteNonQuery(); // Realiza o cadastro da multa
            }
            catch (Exception Ex) // Informa caso ocorra algum erro ao cadastrar a multa
            {
                MessageBox.Show("Erro ao inserir multa!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("erro ao inserir multa!");
            }
            finally // Execuratá independente se ocorrer erro ou não
            {
                conexao.Close(); // Fecha a conexão
            }
        } // Fim do método para inserir multa

        public void DeletarMulta(int id)  // Método de deletar a multa
        {
            // Comandos para inserir o veiculo no MySql
            string sql = "DELETE FROM multa where id = @id";

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Comando que vai deletar o veiculo no MYSQL
                // Adicionando parametros para evitar SQL Injection
                comando.Parameters.AddWithValue("@id", id);
                conexao.Open(); // Abre a conexao com o MySql
                int linhasAfetadas = comando.ExecuteNonQuery(); // Executa o comando SQL de deletar multa
            }
            catch (Exception Ex) // Informa caso algum erro ocorra
            {
                MessageBox.Show("Erro ao deletar a multa!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao deletar multa!");
            }
            finally // Executará independente se ocorrer erro ou não
            {
                conexao.Close(); // Fecha a conexao
            }
        }//Fim do método de deletar a multa

        public void AtualizarMulta(Multa multa)
        {
            // Comandos para editar o veiculo no MySql
            string sql = "UPDATE multa SET DESCRICAO = @descricao, VALORMULTA=@valorMulta WHERE ID=@id";

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao); // Comando que vai atualizar o veiculo no MYSQL

                // Adicionando parametros para evitar SQL Injection
                comando.Parameters.AddWithValue("@descricao", multa.Descricao);
                comando.Parameters.AddWithValue("@valorMulta", multa.ValorMulta);
                comando.Parameters.AddWithValue("@id", multa.Id);
                conexao.Open(); // Abre a conexao com o MySql
                int linhasAfetadas = comando.ExecuteNonQuery(); // Executa o comando SQL e atualiza no banco
            }
            catch (Exception Ex) // Informa caso aconteça algum erro
            {
                MessageBox.Show("Erro ao atualiza a multa!", "ADS-IFRO",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Erro ao atualizar multa!");
            }
            finally // Executará independete se ocorrer erro
            {
                conexao.Close(); // Fecha a conexao
            }
        }
    }//Fim da classe
}
