using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace CountryImageCurrency
{
    public partial class FrmEkle : Form
    {
        public FrmEkle()
        {
            InitializeComponent();
        }

        string DosyaYolu = "";
        byte[] Resim = null;

        private void FrmEkle_Load(object sender, EventArgs e)
        {

        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            DosyaAc.Title = "Resim Seç";
            DosyaAc.Filter = "Png Dosyası (*.png)|*.png|Jpeg Dosyası (*.jpg)|*.jpg|Gif Dosyası (*.gif)|*.gif|Tif Dosyası (*.tif)|*.tif";
            DosyaAc.InitialDirectory = Application.StartupPath + @"\flag";

            if (DosyaAc.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(DosyaAc.FileName);
                DosyaYolu = DosyaAc.FileName.ToString();
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            txtGosterSira.Text = string.Empty;
            txtParaBirim.Text = string.Empty;
            txtPara.Text = string.Empty;
            txtParaSimge.Text = string.Empty;
            txtUlkeEn.Text = string.Empty;
            txtUlkeTr.Text = string.Empty;
            pictureBox1.Image = null;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(DosyaYolu, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);

            byte[] resim = br.ReadBytes((int)fs.Length);

            br.Close();

            fs.Close();

            using (SqlConnection LocalBaglanti = ConnectString.ConnectLocal())
            {
                SqlCommand SqlCmd = new SqlCommand
                {
                    CommandText = "INSERT into tbl_UlkeParaBirim VALUES(@bayrak, @ulke, @country, @parabirim, @currency, @sira, @simge)"
                };
                SqlCmd.Parameters.Clear();
                SqlCmd.Parameters.Add("@bayrak", SqlDbType.Image, resim.Length).Value = resim;
                SqlCmd.Parameters.AddWithValue("@ulke", txtUlkeTr.Text);
                SqlCmd.Parameters.AddWithValue("@country", txtUlkeEn.Text);
                SqlCmd.Parameters.AddWithValue("@parabirim", txtPara.Text);
                SqlCmd.Parameters.AddWithValue("@currency", txtParaBirim.Text);
                SqlCmd.Parameters.AddWithValue("@sira", txtGosterSira.Text);
                SqlCmd.Parameters.AddWithValue("@simge", txtParaSimge.Text);

                SqlCmd.Connection = LocalBaglanti;
                SqlCmd.ExecuteNonQuery();
            };
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            using (SqlConnection LocalBaglanti = ConnectString.ConnectLocal())
            {
                SqlCommand SqlCmdDetay = new SqlCommand("Select * From tbl_UlkeParaBirim Where parabirim = '" + txtPara.Text + "'", LocalBaglanti);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmdDetay);
                DataTable DtableDetay = new DataTable();
                SqlAdapter.Fill(DtableDetay);

                if (DtableDetay.Rows.Count > 0)
                {
                    txtUlkeTr.Text = DtableDetay.Rows[0].Field<string>("ulke");
                    txtUlkeEn.Text = DtableDetay.Rows[0].Field<string>("country");
                    txtParaBirim.Text = DtableDetay.Rows[0].Field<string>("currency");
                    txtParaSimge.Text = DtableDetay.Rows[0].Field<string>("simge").Trim();
                    txtGosterSira.Text = Convert.ToString(DtableDetay.Rows[0].Field<int>("sira"));

                    Resim = (byte[])DtableDetay.Rows[0]["bayrak"];
                    using (MemoryStream ms = new MemoryStream(Resim))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                SqlAdapter.Dispose();
                LocalBaglanti.Close();
            }
        }
    }
}
