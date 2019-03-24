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
    public partial class FrmGoster : Form
    {
        public FrmGoster()
        {
            InitializeComponent();
        }

        byte[] Resim = null;

        private void FrmGoster_Load(object sender, EventArgs e)
        {
            Comboboxlistele();
        }

        void Comboboxlistele()
        {
            SqlConnection LocalBaglanti = ConnectString.ConnectLocal();

            SqlCommand SqlCmdParaBirim = new SqlCommand("Select parabirim From tbl_UlkeParaBirim Order by sira", LocalBaglanti);
            SqlDataReader SqlReadBirim = SqlCmdParaBirim.ExecuteReader();

            while (SqlReadBirim.Read())
            {
                cbxPbirim.Items.Add(SqlReadBirim[0].ToString());
            }

            SqlReadBirim.Close();
            SqlCmdParaBirim.Dispose();
            LocalBaglanti.Close();
            
        }

        private void cbxPbirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection LocalBaglanti = ConnectString.ConnectLocal())
            {
                SqlCommand SqlCmdDetay = new SqlCommand("Select * From tbl_UlkeParaBirim Where parabirim = '" + cbxPbirim.SelectedItem.ToString() + "'", LocalBaglanti);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmdDetay);
                DataTable DtableDetay = new DataTable();
                SqlAdapter.Fill(DtableDetay);

                if (DtableDetay.Rows.Count > 0)
                {
                    txtUlkeTr.Text = DtableDetay.Rows[0].Field<string>("ulke");
                    txtUlkeEn.Text = DtableDetay.Rows[0].Field<string>("country");
                    txtParaBirim.Text = DtableDetay.Rows[0].Field<string>("currency");
                    txtParaSimge.Text = DtableDetay.Rows[0].Field<string>("simge");

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
