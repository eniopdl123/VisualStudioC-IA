using Projeto.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private static int sinal(double u)
        {

            return (u >= 0.0) ? 1 : -1;
        }
        public double[] recuperaPesos = new double[4];
        public int epoca = 0;
        private void btnAprender_Click(object sender, EventArgs e)
        {
            try
            {


                #region LIMPAR DADOS DE APRESENTACAO

                txtW0.Text = string.Empty;
                txtW1.Text = string.Empty;
                txtW2.Text = string.Empty;
                txtW3.Text = string.Empty;
               

                #endregion


                

                double[][] amostras = new double[][]{
                new double[]{-1.0,-1.0, -1.0, -1.0},
                new double[]{0.1, 0.3, 0.6, 0.5},
                new double[]{0.4, 0.7, 0.9, 0.7},
                new double[]{0.7, 0.2, 0.8, 0.1}
        };
                
                double[] saidas = new double[] { 1.0, -1.0, -1.0, 1.0 };//1 tagerina; -1 normal
                double[] pesos = new double[saidas.Length];
                
               Random r1 = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < pesos.Length; i++)
                {
                    pesos[i] = 1.0 / (r1.Next(100) + 1.0);
                    
                    
                }
                double taxaAprendizado = 0.05;
                string erro;

                do
                {

                    erro = "inexiste";

                    for (int k = 0, l= amostras.GetLength(0); k < l; k++)
                    {
                        double u = 0.0;

                        for (int i = 0; i < pesos.Length; i++)
                        {
                            u += pesos[i] * amostras[i][k];
                            
                        }


                        int y = sinal(u);
                        if (y != saidas[k])
                        {

                            double aprendizado = taxaAprendizado * (saidas[k] - y);

                            for (int i = 0; i < pesos.Length; i++)
                            {
                                pesos[i] = pesos[i] + aprendizado * amostras[i][k];
                                
                            }

                            erro = "existe";

                        }
                    }
                    epoca++;


                }
                

                while (erro.Equals("existe"));
                
                recuperaPesos[0] = pesos[0];
                recuperaPesos[1] = pesos[1];
                recuperaPesos[2] = pesos[2];
                recuperaPesos[3] = pesos[3];

            }catch(Exception ex){

               MessageBox.Show("verifique o erro no treino", ex.Message);
            
            }
            
            txtW0.Text = Convert.ToString(recuperaPesos[0]);
            txtW1.Text = Convert.ToString(recuperaPesos[1]);
            txtW2.Text = Convert.ToString(recuperaPesos[2]);
            txtW3.Text = Convert.ToString(recuperaPesos[3]);
            txtQuantidadeDeReestudos.Text = Convert.ToString(epoca);

            }

        
    
        private void btnResponder_Click(object sender, EventArgs e)
        {



            try
            {

                if (txtFosforo.Text == string.Empty || txtAcidez.Text == string.Empty || txtCalcio.Text == string.Empty)
                {
                    MessageBox.Show(" Preencha todos campos");
                    return;
                    
                }

                

                double Fosforo = Convert.ToDouble(txtFosforo.Text, new System.Globalization.CultureInfo(""));
                double Acidez = Convert.ToDouble(txtAcidez.Text, new System.Globalization.CultureInfo(""));
                double Calcio = Convert.ToDouble(txtCalcio.Text, new System.Globalization.CultureInfo(""));

                if ((Fosforo == 0.1 || Fosforo == 0.3 || Fosforo == 0.5 || Fosforo == 0.6) & (Acidez == 0.4 || Acidez == 0.7 || Acidez == 0.9))
                {



                    double[] amostras = new double[] { -1.0, Fosforo, Acidez, Calcio };
                    double[] pesos = new double[] { recuperaPesos[0], recuperaPesos[1], recuperaPesos[2], recuperaPesos[3] };
                    Form2 ft = new Form2();
                    Form3 f = new Form3();
                    double u = 0.0;
                    for (int k = 0; k < amostras.Length; k++)
                    {
                        u += pesos[k] * amostras[k];
                    }
                    int y = sinal(u);
                    if (y == -1)
                    {

                        f.Show();
                        MessageBox.Show("Os dados informados é Laranja Pera");

                        txtFosforo.Text = string.Empty;
                        txtAcidez.Text = string.Empty;
                        txtCalcio.Text = string.Empty;
                        f.Close();
                        

                    }
                    else if (y == 1)
                    {
                        ft.Show();

                        MessageBox.Show("Os dados informados é Laranja tangerina");
                    }

                    txtFosforo.Text = string.Empty;
                    txtAcidez.Text = string.Empty;
                    txtCalcio.Text = string.Empty;
                    
                    ft.Close();

                }
                else
                {
                    MessageBox.Show("Desculpa mais não sei a resposta");
                    txtFosforo.Text = string.Empty;
                    txtAcidez.Text = string.Empty;
                    txtCalcio.Text = string.Empty;

                    return;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("verifique o erro na resposta", ex.Message);

            }


            
        }
        private static Bitmap CaptureRegion(Rectangle rect)
        {

            Bitmap screenshot = new Bitmap(rect.Height, rect.Width, PixelFormat.Format32bppRgb);
            using (Graphics gr = Graphics.FromImage(screenshot))
            {
                gr.CopyFromScreen(rect.Location, Point.Empty, rect.Size);
            }

            return screenshot;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }



        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtFosforo.Text = string.Empty;
            txtAcidez.Text = string.Empty;
            txtCalcio.Text = string.Empty;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.BackgroundImage = CaptureRegion(this.DesktopBounds);
        }
    }
}