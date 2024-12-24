using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrunStokTakip
{
    public partial class FrmUrun : Form
    {
        public FrmUrun()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; database=dburunler; user ID=postgres; password=040905");
        private void btnListProducts_Click(object sender, EventArgs e)
        {
            string sorgu = "select * from products";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void FrmUrun_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from products", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DisplayMember = "productname"; // ön tarafta ad seçicez 
            cmbCategory.ValueMember = "productid"; // ama arka tarafta id ile çalışacak
            cmbCategory.DataSource = dt;
            baglanti.Close();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut = new NpgsqlCommand("insert into products (productid,productname,unitprice,currentstock,criticalstock,categoryid) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
            komut.Parameters.AddWithValue("@p1", int.Parse(txtProductId.Text));
            komut.Parameters.AddWithValue("@p2", txtProductName.Text);
            komut.Parameters.AddWithValue("@p3", double.Parse(txtUnitPrice.Text));
            komut.Parameters.AddWithValue("@p4", int.Parse(numericUpDown1.Value.ToString()));
            komut.Parameters.AddWithValue("@p5", int.Parse(txtCriticalStock.Text));
            komut.Parameters.AddWithValue("@p6", int.Parse(cmbCategory.SelectedValue.ToString()));
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün kaydı başarıyla gerçekleştirildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut2 = new NpgsqlCommand("delete from products where productid=@p1", baglanti);
            komut2.Parameters.AddWithValue("@P1", int.Parse(txtProductId.Text));
            komut2.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün silme başarıyla gerçekleştirildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Stop);

        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut3 = new NpgsqlCommand("update products set productname=@p1,currentstock=@p2,criticalstock=@p3,unitprice=@p4 where productid=@p5", baglanti);
            komut3.Parameters.AddWithValue("@p1", txtProductName.Text);
            komut3.Parameters.AddWithValue("@p2", int.Parse(numericUpDown1.Value.ToString()));
            komut3.Parameters.AddWithValue("@p3", int.Parse(txtCriticalStock.Text));
            komut3.Parameters.AddWithValue("@p4", double.Parse(txtUnitPrice.Text));
            komut3.Parameters.AddWithValue("@p5", int.Parse(txtProductId.Text));
            komut3.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün güncelleme başarıyla gerçekleştirildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut4 = new NpgsqlCommand("select * from productlist", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut4);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            // ComboBox'tan seçilen ürünün ProductID'sini alıyoruz
            if (cmbCategory.SelectedValue != null)
            {
                baglanti.Open();

                // Seçilen ürünün bilgilerini sorguluyoruz
                string sorgu = "SELECT productid, productname, unitprice, currentstock, criticalstock FROM products WHERE productid = @p1";
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@p1", int.Parse(cmbCategory.SelectedValue.ToString()));

                // Sonuçları almak için bir DataTable kullanıyoruz
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Sonuçları DataGridView'e gösteriyoruz
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bir ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }





    }
}