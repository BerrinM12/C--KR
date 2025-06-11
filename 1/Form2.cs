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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _1
{
    public partial class Form2 : Form
    {
        public List<Kotki> kotkiList = new List<Kotki>();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

                MessageBox.Show("Данните са заредени!");
            }
            else
            {
                MessageBox.Show("Файлът не съществува!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string polKriterii = textBox1.Text.Trim(); 

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string pol = row.Cells[3].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(polKriterii) || !pol.Equals(polKriterii, StringComparison.OrdinalIgnoreCase))
                {
                    row.Visible = false;
                }
                else
                {
                    row.Visible = true;
                }
            }

            MessageBox.Show("Данните са филтрирани по пол!");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            kotkiList.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow || row.Cells[0].Value == null) continue;

                string ime = row.Cells[0].Value.ToString();
                string poroda = row.Cells[1].Value?.ToString() ?? "";
                string vuzrastStr = row.Cells[2].Value?.ToString() ?? "0";
                string pol = row.Cells[3].Value?.ToString() ?? "";
                string stopanin = row.Cells[4].Value?.ToString() ?? "";
                string pasport = row.Cells[5].Value?.ToString() ?? "";

                int vuzrast = int.TryParse(vuzrastStr, out int v) ? v : 0;

                kotkiList.Add(new Kotki
                {
                    Ime = ime,
                    Poroda = poroda,
                    Vuzrast = vuzrast,
                    Pol = pol,
                    Stopanin = stopanin,
                    Pasport = pasport
                });
            }

            string porodaKriterii = textBox2.Text.Trim();

            var filtriraniKotki = kotkiList
                .Where(k => string.IsNullOrWhiteSpace(porodaKriterii) ||
                            k.Poroda.Equals(porodaKriterii, StringComparison.OrdinalIgnoreCase))
                .GroupBy(k => k.Poroda)
                .Select(g => new { Poroda = g.Key, Broi = g.Count() })
                .OrderByDescending(g => g.Broi)
                .ToList();

            listBox1.Items.Clear();
            if (filtriraniKotki.Count == 0)
            {
                listBox1.Items.Add("Няма намерени котки по този критерий.");
            }
            else
            {
                foreach (var item in filtriraniKotki)
                {
                    listBox1.Items.Add($"{item.Poroda}: {item.Broi} котки");
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filepath = "filtrirani_kotki.txt";

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Няма данни за запис!");
                return;
            }

            using (StreamWriter sw = new StreamWriter(filepath))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string Ime = row.Cells[0].Value.ToString();
                    string Poroda = row.Cells[1].Value.ToString();
                    int Vuzrast = Convert.ToInt32(row.Cells[2].Value);
                    string Pol = row.Cells[3].Value.ToString();
                    string Stopanin = row.Cells[4].Value.ToString();
                    string Pasport = row.Cells[5].Value.ToString();

                    sw.WriteLine($"{Ime}\t{Poroda}\t{Vuzrast}\t{Pol}\t{Stopanin}\t{Pasport}");
                }
            }

            MessageBox.Show("Филтрираните данни са записани в нов файл!");
        }

    }
}
