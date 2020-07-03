using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orvos
{
    public partial class UjKezeles : Form
    {
        klinikaEntities entities;

        public UjKezeles(klinikaEntities entities)
        {
            InitializeComponent();
            this.entities = entities;

            FillOrvosok();
            FillPaciens();

            dateTimePickerDatum.MinDate = DateTime.Now.Date;
        }

        private void numericUpDownKezd_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownIdotartam.Maximum = 17 - numericUpDownKezd.Value;
        }

        void FillOrvosok()
        {
            comboBoxOrvos.DataSource = entities.orvos.OrderBy(x => x.nev).ToList();
            comboBoxOrvos.DisplayMember = "nev";
            comboBoxOrvos.ValueMember = "orvos_id";
        }

        void FillPaciens()
        {
            comboBoxPaciens.DataSource = entities.paciens.OrderBy(x => x.nev).ToList();
            comboBoxPaciens.DisplayMember = "nev";
            comboBoxPaciens.ValueMember = "Paciens_id";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            entities.kezeles.Add(new kezeles()
            {
                paciens_id = (int)comboBoxPaciens.SelectedValue,
                orvos_id = (int)comboBoxOrvos.SelectedValue,
                datum = dateTimePickerDatum.Value,
                kezdesido = (int)numericUpDownKezd.Value,
                idotartam = (int)numericUpDownIdotartam.Value,
                megjegyzes = richTextBox1.Text
            });

            entities.SaveChanges();
        }
    }
}
