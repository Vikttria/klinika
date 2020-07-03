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
    public partial class Form1 : Form
    {
        klinikaEntities entities;

        class KlinikaAdat
        {
            public string orvosNeve;
            public string szakterulet;
            public string paciensNeve;
            public int paciensKora;
            public string megjegyzes;
            public DateTime datum;
            public int kezdesido;
            public int dij;
        }

        public Form1()
        {
            InitializeComponent();

            entities = new klinikaEntities();
            SetData();
        }

        void SetData()
        {
            bindingSource1.DataSource = entities.kezeles.ToList().Select(x => new KlinikaAdat
            {
                orvosNeve = x.orvos.nev,
                szakterulet = x.orvos.szakterulet.nev,
                paciensNeve = x.paciens.nev,
                paciensKora = DateTime.Now.Date.Year - x.paciens.szuletett.Value.Year,
                megjegyzes = x.megjegyzes,
                datum = x.datum.Value,
                kezdesido = x.kezdesido.Value,
                dij = x.idotartam.Value * x.orvos.oradij.Value
            }).OrderBy(x => x.paciensNeve).ThenByDescending(x => x.datum).ToList();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            var adat = bindingSource1.Current as KlinikaAdat;
            textBoxONeve.Text = adat.orvosNeve;
            textBoxOSzak.Text = adat.szakterulet;
            textBoxPNev.Text = adat.paciensNeve;
            textBoxPKor.Text = adat.paciensKora.ToString();
            textBoxMegj.Text = adat.megjegyzes;
            textBoxDatum.Text = adat.datum.ToString("yyyy-MM-dd");
            textBoxKezdete.Text = adat.kezdesido.ToString();
            textBoxDij.Text = adat.dij.ToString("# ###");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Orvosok().ShowDialog();
        }

        private void buttonUj_Click(object sender, EventArgs e)
        {
            new UjKezeles(entities).ShowDialog();
            SetData();
        }

        private void buttonFel_Click(object sender, EventArgs e)
        {
            int leptetes = (int)numericUpDownLeptetes.Value;
            for (int i = 0; i < leptetes; i++)
            {
                try
                {
                    bindingSource1.MoveNext();
                }
                catch
                {

                    return;
                }
            }
        }

        private void buttonLe_Click(object sender, EventArgs e)
        {
            int leptetes = (int)numericUpDownLeptetes.Value;
            for (int i = 0; i < leptetes; i++)
            {
                try
                {
                    bindingSource1.MovePrevious();
                }
                catch
                {

                    return;
                }
            }
        }
    }
}
