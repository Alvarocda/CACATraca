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
                LeEPadronizaArquivoAsync();
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
                        CriarArquivoPadronizado.WriteLine("" + texto[0].PadLeft(16, '0') + texto[1].PadRight(40, ' ').Substring(0,35) + "00110");
                        ContadorRegistros = ContadorRegistros + 1;
                        LabelTotal.Text = "" + ContadorRegistros;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                CriarArquivoPadronizado.Close();
                Process.Start("explorer.exe", "C:\\");
                MessageBox.Show("Padronização concluida\n" +
                    "ARQUIVO ESTA EM: C:\\");
            }
            catch(Exception e)
            {
                MessageBox.Show(""+ e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void ajudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Ajuda().Show();
        }
    }
}
