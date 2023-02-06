using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Exam
{
    public partial class Form1 : Form
    {
        //пример названия файла в Entity
        //public OlympicGamesEntities()
        //    : base("name=OlympicGamesEntities")
        //{
        //}

        OlympicGamesEntities db = null;

        public Form1()
        {
            InitializeComponent();
            db = new OlympicGamesEntities();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Add("Olympiads");
            comboBox2.Items.Add("Country");
            comboBox2.Items.Add("Persons");
            comboBox2.Items.Add("KindsOfSport");
            comboBox2.Items.Add("Results");
            db.Olympiads.Load();
            db.Country.Load();
            db.Persons.Load();
            db.KindsOfSport.Load();
            db.Results.Load();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedItem.ToString())
            {
                case "Olympiads":
                    dataGridView2.DataSource = db.Olympiads.Local.ToBindingList();
                    dataGridView2.Columns["Country"].Visible = false;
                    dataGridView2.Columns["Results"].Visible = false;
                    break;
                case "Country":
                    dataGridView2.DataSource = db.Country.Local.ToBindingList();
                    dataGridView2.Columns["Olympiads"].Visible = false;
                    dataGridView2.Columns["Persons"].Visible = false;
                    break;
                case "Persons":
                    dataGridView2.DataSource = db.Persons.Local.ToBindingList();
                    dataGridView2.Columns["Country"].Visible = false;
                    dataGridView2.Columns["KindsOfSport"].Visible = false;
                    dataGridView2.Columns["Results"].Visible = false;
                    break;
                case "KindsOfSport":
                    dataGridView2.DataSource = db.KindsOfSport.Local.ToBindingList();
                    dataGridView2.Columns["Persons"].Visible = false;
                    break;
                case "Results":
                    dataGridView2.DataSource = db.Results.Local.ToBindingList();
                    dataGridView2.Columns["Olympiads"].Visible = false;
                    dataGridView2.Columns["Persons"].Visible = false;
                    break;
                default:
                    break;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await db.SaveChangesAsync();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Query1();
                    break;
                case 1:
                    Query2();
                    break;
                case 2:
                    Query3();
                    break;
                case 3:
                    Query4();
                    break;
                case 4:
                    Query5();
                    break;
                case 5:
                    Query6();
                    break;
                case 6:
                    Query7();
                    break;
                default:
                    break;
            }
        }

        private async void Query1()
        {
            /* После завершения асинхронной операции оператор await возвращает результат операции,
            если таковой имеется.Когда оператор await применяется к операнду, 
            который представляет уже завершенную операцию, 
            он возвращает результат операции немедленно без приостановки включающего метода. */

            var tmp = db.Results.Select(x => new
            {
                OlimpiadNumber = x.Olympiads.Number,
                Country = x.Persons.Country.NameCountry,
                MedalsI = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 1).Count(),
                MedalsII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 2).Count(),
                MedalsIII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 3).Count()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query2()
        {
            var tmp = db.Results.Select(x => new
            {
                OlimpiadNumber = x.Olympiads.Number,
                KindsOfSport = x.Persons.KindsOfSport.NameKind,
                MedalsI = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdKindOfSport == x.Persons.IdKindOfSport).Where(y => y.Medal == 1).Select(y => y.Persons.FullName).FirstOrDefault(),
                MedalsII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdKindOfSport == x.Persons.IdKindOfSport).Where(y => y.Medal == 2).Select(y => y.Persons.FullName).FirstOrDefault(),
                MedalsIII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdKindOfSport == x.Persons.IdKindOfSport).Where(y => y.Medal == 3).Select(y => y.Persons.FullName).FirstOrDefault()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query3()
        {
            var tmp = db.Results.Select(x => new
            {
                OlimpiadNumber = x.Olympiads.Number,
                Country = db.Results.Where(z => db.Results.Where(y => y.Persons.IdCountry == z.Persons.IdCountry && y.IdOlympiad == x.IdOlympiad && y.Medal == 1).GroupBy(y => y.Persons.IdCountry).Select(y => y.Count()).FirstOrDefault() == db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Medal == 1).GroupBy(y => y.Persons.IdCountry).Select(y => y.Count()).Max()).FirstOrDefault().Persons.Country.NameCountry,
                MedalsI = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Medal == 1).GroupBy(y => y.Persons.IdCountry).Select(y => y.Count()).Max()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query4()
        {
            var tmp = db.Results.Select(x => new
            {
                KindOfSport = x.Persons.KindsOfSport.NameKind,
                NamePerson = db.Results.Where(z => db.Results.Where(y => y.Persons.Id == z.Persons.Id && y.Persons.IdKindOfSport == x.Persons.IdKindOfSport && y.Medal == 1).GroupBy(y => y.Persons.Id).Select(y => y.Count()).FirstOrDefault() == db.Results.Where(y => y.Persons.IdKindOfSport == x.Persons.IdKindOfSport && y.Medal == 1).GroupBy(y => y.Persons.Id).Select(y => y.Count()).Max()).FirstOrDefault().Persons.FullName,
                MedalsI = db.Results.Where(y => y.Persons.IdKindOfSport == x.Persons.IdKindOfSport && y.Medal == 1).GroupBy(y => y.Persons.Id).Select(y => y.Count()).Max()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query5()
        {
            var tmp = db.Olympiads.Select(x => new
            {
                Country = db.Olympiads.Where(z => db.Olympiads.Where(y => y.IdCountry == z.IdCountry).GroupBy(y => y.IdCountry).Select(y => y.Count()).FirstOrDefault() == db.Olympiads.GroupBy(y => y.IdCountry).Select(y => y.Count()).Max()).FirstOrDefault().Country.NameCountry,
                NumberOlympiads = db.Olympiads.GroupBy(y => y.IdCountry).Select(y => y.Count()).Max()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query6()
        {
            int selectedCountry = -1;
            ChoiceCountry form = new ChoiceCountry(db.Country.ToArray());
            if (form.ShowDialog() == DialogResult.OK)
            {
                selectedCountry = form.selectedCountry;
            }
            var tmp = db.Results.Where(x => x.Persons.IdCountry == (selectedCountry + 1)).Select(x => new { OlimpiadNumber = x.Olympiads.Number, NamePerson = x.Persons.FullName });
            dataGridView1.DataSource = await tmp.ToListAsync();
        }

        private async void Query7()
        {
            int selectedCountry = -1;
            ChoiceCountry form = new ChoiceCountry(db.Country.ToArray());
            if (form.ShowDialog() == DialogResult.OK)
            {
                selectedCountry = form.selectedCountry;
            }
            var tmp = db.Results.Where(x => x.Persons.IdCountry == (selectedCountry + 1)).Select(x => new
            {
                OlimpiadNumber = x.Olympiads.Number,
                MedalsI = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 1).Count(),
                MedalsII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 2).Count(),
                MedalsIII = db.Results.Where(y => y.IdOlympiad == x.IdOlympiad && y.Persons.IdCountry == x.Persons.IdCountry).Where(y => y.Medal == 3).Count()
            }).Distinct();
            dataGridView1.DataSource = await tmp.ToListAsync();
        }
    }
}
