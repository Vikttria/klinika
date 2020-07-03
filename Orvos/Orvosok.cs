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

namespace Orvos
{
    public partial class Orvosok : Form
    {
        klinikaEntities entities;

        class OrvosAdat
        {
            public string orvosNeve;
            public string szakterulet;
            public int oradij;
            public string telefon;
            public string CSV => $"{orvosNeve};{szakterulet};{oradij};{telefon}";
        }

        public Orvosok()
        {
            InitializeComponent();

            entities = new klinikaEntities();
            SetData();
            FillSzakok();
        }

        void SetData()
        {
            bindingSource1.DataSource = entities.orvos.ToList().Select(x => new OrvosAdat
            {
                orvosNeve = x.nev,
                szakterulet = x.szakterulet.nev,
                oradij = x.oradij.Value,
                telefon = x.telefon
            });
        }

        void FillSzakok()
        {
            comboBoxSzak.DataSource = entities.szakterulet.OrderBy(x => x.nev).ToList();
            comboBoxSzak.DisplayMember = "nev";
            comboBoxSzak.ValueMember = "szakterulet_id";
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            var adat = bindingSource1.Current as OrvosAdat;
            textBoxNev.Text = adat.orvosNeve;
            textBoxSzak.Text = adat.szakterulet;
            textBoxDij.Text = adat.oradij.ToString("# ###");
            textBoxTel.Text = adat.telefon;
        }

        private void numericUpDownTol_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownIg.Minimum = numericUpDownTol.Value + 500;
        }

        private void buttonBe_Click(object sender, EventArgs e)
        {
            bindingSource1.DataSource = entities.orvos.ToList().
                Where(x => x.szakterulet_id == (int)comboBoxSzak.SelectedValue).
                Where(x => (int)numericUpDownTol.Value <= x.oradij && x.oradij <= (int)numericUpDownIg.Value).
                Select(x => new OrvosAdat
            {
                orvosNeve = x.nev,
                szakterulet = x.szakterulet.nev,
                oradij = x.oradij.Value,
                telefon = x.telefon
            });
        }

        private void buttonKi_Click(object sender, EventArgs e)
        {
            SetData();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogSavePath.ShowDialog() != DialogResult.OK) return;

            string fname = $"orvos_{comboBoxSzak.Text.ToLower()}_[{numericUpDownTol.Value}_{numericUpDownIg.Value} Ft]_{DateTime.Now: yyyy-MM-dd}.csv";

            fname = Path.Combine(folderBrowserDialogSavePath.SelectedPath, fname);

            File.WriteAllLines(fname, entities.orvos.ToList().
                Where(x => x.szakterulet_id == (int)comboBoxSzak.SelectedValue).
                Where(x => (int)numericUpDownTol.Value <= x.oradij && x.oradij <= (int)numericUpDownIg.Value).
                Select(x => new OrvosAdat
                {
                    orvosNeve = x.nev,
                    szakterulet = x.szakterulet.nev,
                    oradij = x.oradij.Value,
                    telefon = x.telefon
                }).Select(x => x.CSV));

            MessageBox.Show("Sikeres export");
        }
    }
}
