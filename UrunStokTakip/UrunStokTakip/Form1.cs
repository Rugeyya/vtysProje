using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrunStokTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; database=dburunler; user ID=postgres; password=040905");
        private void btnListCategory_Click(object sender, EventArgs e)
        {
            string sorgu = "select * from categories";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut1=new NpgsqlCommand ("insert into categories (categoryid,categoryname) values (@p1,@p2)", baglanti);
            komut1.Parameters.AddWithValue("@p1", int.Parse( txtCategoryId.Text));
            komut1.Parameters.AddWithValue("@p2", txtCategoryName.Text);
            komut1.ExecuteNonQuery(); // değişiklikleri veritabanına yansıtmak içindir
            baglanti.Close();
            MessageBox.Show("Kategoriler ekleme işlemi başarıyla gerçekleştirildi");
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from categories", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DisplayMember = "categoryname"; // ön tarafta ad seçicez 
            cmbCategory.ValueMember = "categoryid"; // ama arka tarafta id ile çalışacak
            cmbCategory.DataSource = dt;
            baglanti.Close();
        }
    }
}
