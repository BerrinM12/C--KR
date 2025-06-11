using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _1
{
    public partial class Form1 : Form
    {
        public List<Kotki> kotkiList = new List<Kotki>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string filepath = "kotki.txt";
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string Ime = row.Cells[0].Value.ToString().Trim() ?? "";
                    string Poroda = row.Cells[1].Value.ToString();
                    int Vuzrast = Convert.ToInt32(row.Cells[2].Value);
                    string Pol = row.Cells[3].Value.ToString().Trim() ?? "";
                    string Stopanin = row.Cells[4].Value.ToString().Trim() ?? "";
                    string Pasport = row.Cells[5].Value.ToString().Trim() ?? "";

                    sw.WriteLine($"{Ime}\t{Poroda}\t{Vuzrast}\t{Pol}\t{Stopanin}\t{Pasport}");
                }
            }

            MessageBox.Show("Записани успешно в kotki.txt!");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Kotki> sortedList = new List<Kotki>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string ime = row.Cells[0].Value?.ToString().Trim() ?? "";
                string poroda = row.Cells[1].Value?.ToString().Trim() ?? "";
                string pol = row.Cells[3].Value?.ToString().Trim() ?? "";
                string stopanin = row.Cells[4].Value?.ToString().Trim() ?? "";

                Kotki kotka = new Kotki
                {
                    Ime = ime,
                    Poroda = poroda,
                    Pol = pol,
                    Stopanin = stopanin
                };

                sortedList.Add(kotka);
            }

            string kriteriiStopanin = textBox1.Text.Trim();
            string kriteriiPoroida = textBox2.Text.Trim();   
            string kriteriiPol = textBox3.Text.Trim();       

            var filtriraniKotki = sortedList.Where(k =>
                (string.IsNullOrWhiteSpace(kriteriiStopanin) || k.Stopanin.Equals(kriteriiStopanin, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(kriteriiPoroida) || k.Poroda.Equals(kriteriiPoroida, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(kriteriiPol) || k.Pol.Equals(kriteriiPol, StringComparison.OrdinalIgnoreCase))
            ).OrderBy(k => k.Stopanin).ToList();

            listBox1.Items.Clear();
            foreach (var kotka in filtriraniKotki)
            {
                listBox1.Items.Add($"{kotka.Ime}\t{kotka.Poroda}\t{kotka.Pol}\t{kotka.Stopanin}");
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filepath = "updated_kotki.txt";
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                foreach (var item in listBox1.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }

            MessageBox.Show("Данните от списъка са записани успешно в updated_kotki.txt!");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();

            form2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string filepath = "kotki.txt";

            if (File.Exists(filepath))
            {
                dataGridView1.Rows.Clear();

                string[] lines = File.ReadAllLines(filepath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length >= 6)
                    {
                        dataGridView1.Rows.Add(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]);
                    }
                }

                MessageBox.Show("Данните са заредени от файл!");
            }
            else
            {
                MessageBox.Show("Файлът не съществува!");
            }
        }
    }
}
