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

namespace VeriSifreleme_13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        Random rast = new Random();
        SqlConnection baglan = new SqlConnection(@"");

        private void Form1_Load(object sender, EventArgs e)
        {
            //6 Karakterlik rastgele bir kod oluşturuluyor.
            Listele();
            string passcode = new string(Enumerable.Repeat("0123456789", 6).Select(r => r[rast.Next(r.Length)]).ToArray());
            txtPASSCODE.Text = passcode;
        }

        void Listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_VERİLER", baglan);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnSAVE_Click(object sender, EventArgs e)
        {
            //Textbox'lara girilen tüm bilgiler şifrelenerek veritabanına kaydediliyor. Ve şifreli formatta veritabanında saklanıyor.
            string AD = txtAD.Text;
            byte[] adVeri = ASCIIEncoding.ASCII.GetBytes(AD);
            string adSifre = Convert.ToBase64String(adVeri);

            string SOYAD = txtSOYAD.Text;
            byte[] soyadVeri = ASCIIEncoding.ASCII.GetBytes(SOYAD);
            string soyadSifre = Convert.ToBase64String(soyadVeri);

            string EMAİL = txtEMAİL.Text;
            byte[] emailVeri = ASCIIEncoding.ASCII.GetBytes(EMAİL);
            string emailSifre = Convert.ToBase64String(emailVeri);

            string SİFRE = txtSİFRE.Text;
            byte[] sifreVeri = ASCIIEncoding.ASCII.GetBytes(SİFRE);
            string sifreSifre = Convert.ToBase64String(sifreVeri);

            string PASSCODE = txtPASSCODE.Text;
            byte[] passcodeVeri = ASCIIEncoding.ASCII.GetBytes(PASSCODE);
            string passcodeSifre = Convert.ToBase64String(passcodeVeri);


            if (AD != "" && SOYAD != "" && EMAİL != "" && SİFRE != "" && PASSCODE != "") //Veritabanına şifreli bilgileri kaydetme işlemi
            {
                baglan.Open();
                SqlCommand cmd = new SqlCommand("insert into TBL_VERİLER (AD,SOYAD,EMAİL,SİFRE,PASSCODE) values (@a1, @a2, @a3, @a4, @a5)", baglan);
                cmd.Parameters.AddWithValue("@a1", adSifre);
                cmd.Parameters.AddWithValue("@a2", soyadSifre);
                cmd.Parameters.AddWithValue("@a3", emailSifre);
                cmd.Parameters.AddWithValue("@a4", sifreSifre);
                cmd.Parameters.AddWithValue("@a5", passcodeSifre);
                cmd.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Veriler başarıyla güvenli şifrelenmiş formatta kaydedildi.", "İşlem Başarılı");
                Listele();
                txtAD.Clear();
                txtSOYAD.Clear();
                txtEMAİL.Clear();
                txtSİFRE.Clear();
                txtPASSCODE.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secim = dataGridView1.CurrentRow.Index;
            txtAD.Text = dataGridView1.Rows[secim].Cells["AD"].Value.ToString();
            txtSOYAD.Text = dataGridView1.Rows[secim].Cells["SOYAD"].Value.ToString();
            txtEMAİL.Text = dataGridView1.Rows[secim].Cells["EMAİL"].Value.ToString();
            txtSİFRE.Text = dataGridView1.Rows[secim].Cells["SİFRE"].Value.ToString();
            txtPASSCODE.Text = dataGridView1.Rows[secim].Cells["PASSCODE"].Value.ToString();

        }
        //Şifrelenmiş verileri Datagrid'den seçerek textbox'lara aktarma yoluyla butona tıklayarak şifreler çözülüyor.
        private void btnDESİFRE_Click(object sender, EventArgs e)
        {
            string ADcoz = txtAD.Text;
            byte[] adDizi = Convert.FromBase64String(ADcoz);
            string adcozum = ASCIIEncoding.ASCII.GetString(adDizi);
            txtAD.Text = adcozum;

            string SOYADcoz = txtSOYAD.Text;
            byte[] soyadDizi = Convert.FromBase64String(SOYADcoz);
            string soyadcozum = ASCIIEncoding.ASCII.GetString(soyadDizi);
            txtSOYAD.Text = soyadcozum;

            string EMAİLcoz = txtEMAİL.Text;
            byte[] emailDizi = Convert.FromBase64String(EMAİLcoz);
            string emailcozum = ASCIIEncoding.ASCII.GetString(emailDizi);
            txtEMAİL.Text = emailcozum;

            string SİFREcoz = txtSİFRE.Text;
            byte[] sifreDizi = Convert.FromBase64String(SİFREcoz);
            string sifrecozum = ASCIIEncoding.ASCII.GetString(sifreDizi);
            txtSİFRE.Text = sifrecozum;

            string PASSCODEcoz = txtPASSCODE.Text;
            byte[] passcodeDizi = Convert.FromBase64String(PASSCODEcoz);
            string passcodecozum = ASCIIEncoding.ASCII.GetString(passcodeDizi);
            txtPASSCODE.Text = passcodecozum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtAD.Clear();
            txtSOYAD.Clear();
            txtEMAİL.Clear();
            txtSİFRE.Clear();
            string passcode = new string(Enumerable.Repeat("0123456789", 6).Select(r => r[rast.Next(r.Length)]).ToArray());
            txtPASSCODE.Text = passcode;
            txtAD.Focus();
        }

    }
}
