using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountryImageCurrency
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            
        }

        public void YavruForm(Form _Form)
        {
            if (!YavruFormAktif(_Form))
            {
                _Form.MdiParent = this;
                _Form.Show();
            }

        }

        public bool YavruFormAktif(Form Form)
        {
            bool FormAcik = false;

            if (MdiChildren.Count() > 0)
            {
                foreach (var item in MdiChildren)
                {
                    if (Form.Name == item.Name)
                    {
                        item.Activate();
                        FormAcik = true;
                    }
                }
            }
            return FormAcik;
        }

        private void btnGoster_Click(object sender, EventArgs e)
        {
            //Birden fazla mdichild form açılmasını engellemek için
            FrmGoster fGosterForm = new FrmGoster();
            fGosterForm.Name = "fGoster";
            YavruForm(fGosterForm);

            // Standart mdiChild form açmak için
            //FrmGoster fGosterForm = new FrmGoster();
            //fGosterForm.MdiParent = this;
            //fGosterForm.Show();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            FrmEkle fEkleForm = new FrmEkle();
            fEkleForm.Name = "fEkle";
            YavruForm(fEkleForm);
        }


    }
}
