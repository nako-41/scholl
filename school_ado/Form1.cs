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




namespace school_ado
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

          string yol = "Data Source=DESKTOP-014OO3I;Initial Catalog=okanokul;Integrated Security=True";

        private void Form1_Load(object sender, EventArgs e)
        {
            string komutcumlesi = "select  * from personel";

            using (SqlConnection baglanti = new SqlConnection() )
            {
                baglanti.ConnectionString = yol;

                using (SqlCommand listelemekomutu = new SqlCommand(komutcumlesi, baglanti))
                {
                    baglanti.Open();

                    using (DataTable datatable = new DataTable())
                    {
                        datatable.Columns.Add("personelKimlikNo");
                        datatable.Columns.Add("Adı");
                        datatable.Columns.Add("Soyadı");

                        using (SqlDataReader rdr = listelemekomutu.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                DataRow row = datatable.NewRow();
                                row["personelKimlikNo"] = rdr["personelID"];
                                row["Adı"] = rdr["adi"];
                                row["Soyadı"] = rdr["soyadi"];
                                datatable.Rows.Add(row);

                            }
                        }

                        dataGridView1.DataSource = datatable;
                    }

                    baglanti.Close();


                    if (baglanti.State == System.Data.ConnectionState.Closed)
                    {
                        toolStripStatusLabel1.Text = "kapalı";
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "açık";
                    }

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 400;
                    dataGridView1.Columns[2].Width = 230;




                }
            }

        }

        private void btnara_Click(object sender, EventArgs e)
        {

            if (txtid.Text == "")
            {
                MessageBox.Show("lütfen aranacak veriyi giriniz");
                return;
            }

            string sorgu = "select * from personel where personelID=" + txtid.Text;

            using (SqlConnection baglanti = new SqlConnection())
            {
                baglanti.ConnectionString = yol;
                using (SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds, "personel");
                        dataGridView1.DataSource = ds.Tables["personel"];
                    }
                }
            }


        }

        private void btnekle_Click(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection())
            {
                baglanti.ConnectionString = yol;
                baglanti.Open();

                using (SqlCommand ekle = new SqlCommand())
                {
                    ekle.Connection = baglanti;
                    ekle.CommandType = CommandType.Text;
                    ekle.CommandText = "insert into personel(adi,soyadi)" +
                    "values(@FirstName,@LastName)";

                    ekle.Parameters.AddWithValue("@FirstName", txtad.Text);
                    ekle.Parameters.AddWithValue("@LastName", txtsoyad.Text);


                    if (ekle.ExecuteNonQuery() == 1)
                        MessageBox.Show("kayıt eklendi");
                    else MessageBox.Show("kayıt eklenemedi");

                }
                baglanti.Close();

            }

            Form1_Load(this, null);
        }

        private void btnguncelle_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "")
            {
                MessageBox.Show("lütfen aranacak veriyi giriniz");
                return;
            }


            using (SqlConnection baglanti = new SqlConnection())
            {

                baglanti.ConnectionString = yol;
                baglanti.Open();
                using (SqlCommand guncelle = new SqlCommand())
                {

                    guncelle.Connection = baglanti;
                    guncelle.CommandType = CommandType.Text;
                    guncelle.CommandText =
               "update personel set FirstName=@FirstName,LastName=@LastName where personelID=@ID";


                    guncelle.Parameters.AddWithValue("@FirstName", txtad.Text);
                    guncelle.Parameters.AddWithValue("@LastName", txtsoyad.Text);
                    guncelle.Parameters.AddWithValue("@ID", txtid.Text);

                    if (guncelle.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("kayıt guncellendi");
                    }
                    else
                    {
                        MessageBox.Show("kayıt güncellenemedi");
                    }

                }
                baglanti.Close();

            }
            Form1_Load(this, null);
        }
    
    void temizle()
    {
        txtad.Text = "";
        txtsoyad.Text = "";
        txtid.Text = "";

    }


    private void btnlist_Click(object sender, EventArgs e)
        {
            Form1_Load(this, null);
            temizle();
        }
  
        void Sil(int numara)
        {
            using (SqlConnection baglanti = new SqlConnection())
            {

                baglanti.ConnectionString = yol;
                baglanti.Open();
               string sql = "DELETE FROM personel WHERE personelID=@numara";
                using (SqlCommand komut = new SqlCommand(sql,baglanti))
                {
               
                    komut.Parameters.AddWithValue("@numara", numara);
                    if (komut.ExecuteNonQuery() == 1)
                    MessageBox.Show("kayıt silindi");
                    else
                        MessageBox.Show("kayıt silindi");
                }
                baglanti.Close();
            }

            

        }
    
        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drow in dataGridView1.SelectedRows)  //Seçili Satırları Silme
            {
                int numara = Convert.ToInt32(drow.Cells[0].Value);
                Sil(numara);
                Form1_Load(this, null);
            }
        }
    }
}
