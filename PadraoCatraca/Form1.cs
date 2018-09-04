using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PadraoCatraca
{
    public partial class Form1 : Form
    {
        public int TotalNumerosRA { get; set; }
        public String UltimoUsuarioProcessado { get; set; }
        public Boolean ProcessoParcial { get; set; }        
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnPadrnizarClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TxtCaminhoArquivo.Text))
            {
                MessageBox.Show("Por favor, preencha corretamente o caminho para o arquvio .txt", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (String.IsNullOrEmpty(TxtQtdNumerosRA.Text))
                {
                    TxtQtdNumerosRA.Text = "16";
                }
                try
                {
                    TotalNumerosRA = Int32.Parse(TxtQtdNumerosRA.Text);
                    if(TotalNumerosRA <= 0)
                    {
                        MessageBox.Show("Por favor, insira um numero maior que 0", "Erro",  MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        LeEPadronizaArquivoAsync();
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show("Erro 666: Por favor, utilize apenas numeros no campo Qtd Numeros R.A\n"+ex.Message,"Erro",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                }                
            }            
        }
        private void BtnBuscarArquivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ProcuraArquivo = new OpenFileDialog();
            ProcuraArquivo.InitialDirectory = "C:\\";
            ProcuraArquivo.ShowDialog();
            TxtCaminhoArquivo.Text = ProcuraArquivo.FileName;
        }
        public void LeEPadronizaArquivoAsync()
        {
            StreamReader leitor;
            try
            {
                leitor = new StreamReader(TxtCaminhoArquivo.Text);
                String linha = null;
                StreamWriter CriarArquivoPadronizado = new StreamWriter("C:\\ArquivoPadronizado.txt");
                //MessageBox.Show("Debug 1");
                int ContadorRegistros = 0;
                while (null != (linha = leitor.ReadLine()))
                {
                    //MessageBox.Show("Entrou no While");
                    var texto = linha.Split(',');
                    try
                    {
                        if (ContadorRegistros < 4999)
                         {
                            byte[] byteTemporario;
                            byteTemporario = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes("" + texto[0].PadLeft(TotalNumerosRA, '0') + texto[1].PadRight(40, ' ').Substring(0, 40) + "00110");
                            CriarArquivoPadronizado.WriteLine(System.Text.Encoding.UTF8.GetString(byteTemporario));
                            //CriarArquivoPadronizado.WriteLine("" + texto[0].PadLeft(TotalNumerosRA, '0') + texto[1].PadRight(40, ' ').Substring(0, 40) + "00110");
                             ContadorRegistros = ContadorRegistros + 1;
                             LabelTotal.Text = "" + ContadorRegistros;
                             UltimoUsuarioProcessado = "" + texto[0].PadLeft(TotalNumerosRA, '0') + texto[1].PadRight(40, ' ').Substring(0, 40) + "00110";
                             ProcessoParcial = false;
                        }
                        else
                        {
                             LabelTotal.Text = "5000";
                             ProcessoParcial = true;
                             ContadorRegistros = ContadorRegistros + 1;
                        }                        
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Não foi possivel criar o arquivo no disco C\nErro.: "+e.Message);
                    }
                }
                if (ProcessoParcial)
                {
                    CriarArquivoPadronizado.Close();
                    ContadorRegistros = ContadorRegistros - 5000;
                    MessageBox.Show("Foram processados apenas os 5000 primerios usuarios pois é o limite maximo da catraca\n" +
                    "Quantidade de registros não processados: " + ContadorRegistros + "\n" +
                    "Ultimo usuario processado.: " + UltimoUsuarioProcessado + "\nARQUIVO ESTA EM C:\\", "Aviso",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    Process.Start("explorer.exe", "C:\\");
                }
                else
                {
                    CriarArquivoPadronizado.Close();
                    MessageBox.Show("Padronização concluida\n" +
                        "ARQUIVO ESTA EM: C:\\", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("explorer.exe", "C:\\");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Não foi possivel encontrar o arquivo a ser padronizado\nErro.: "+ e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }            
        }
        private void ajudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Ajuda().Show();
        }

        
    }
}
