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

        public List<Multa> ListarMultas(int id)
        {
            List<Multa> listaDeMultasParaExportar = new List<Multa>();
            string sql = "SELECT * FROM multa where veiculoId = @id";
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);
                conexao.Open();
                MySqlDataReader dados = comando.ExecuteReader();
                while (dados.Read())
                {
                    Multa multa = new Multa();
                    multa.Id = Convert.ToInt32(dados["id"]);
                    multa.Descricao = dados["descricao"].ToString();
                    multa.ValorMulta = Convert.ToDecimal(dados["valorMulta"].ToString());
                    multa.VeiculoId = Convert.ToInt32(dados["veiculoID"].ToString());
                    listaDeMultasParaExportar.Add(multa);
                }

                conexao.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return listaDeMultasParaExportar;
        } // Fim do método ListarMultas

        public void InserirMulta(Multa multa)
        {
            string sql = "INSERT INTO multa (descricao,valorMulta,veiculoID) VALUES (@descricao,@valorMulta,@veiculoID)";
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@descricao", multa.Descricao);
                comando.Parameters.AddWithValue("@valorMulta", multa.ValorMulta);
                comando.Parameters.AddWithValue("@veiculoId", multa.VeiculoId);

                conexao.Open();
                int linhasAfetadas = comando.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                throw new Exception("Erro ao inserir multa!");
            }
            finally
            {
                conexao.Close();
            }
        }// Fim do método InserirMulta
    }//Fim da classe
}
