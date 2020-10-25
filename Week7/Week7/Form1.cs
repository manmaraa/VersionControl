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
using Week7.Entities;

namespace Week7
{
    public partial class Form1 : Form
    {
        int zaroev;
        Random rng = new Random(1234);
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<int> maleNbrs = new List<int>();
        List<int> femaleNbrs = new List<int>();
        public Form1()
        {
            

            InitializeComponent();
            
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProb(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProb(@"C:\Temp\halál.csv");
            for (int year = 2005; year <= zaroev; year++)
            {
               
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year,Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
        public List<DeathProbability> GetDeathProb(string csvpath)
        {
            List<DeathProbability> death = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    death.Add(new DeathProbability()
                    {
                        YearsOld = int.Parse(line[0]),
                       
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        DeathProb =  double.Parse(line[2])
                    });
                }
            }

            return death;
        }
        public List<BirthProbability> GetBirthProb(string csvpath)
        {
            List<BirthProbability> birth = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birth.Add(new BirthProbability()
                    {
                        YearsOld = int.Parse(line[0]),

                        NbrOfChildren = int.Parse(line[1]),
                        BirthProb = double.Parse(line[2])
                    });
                }
            }

            return birth;
        }
        private void SimStep(int year, Person person)
        {
     
            if (!person.IsAlive) return;

             byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.YearsOld == age
                             select x.DeathProb).FirstOrDefault();
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

             if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.YearsOld == age
                                 select x.BirthProb).FirstOrDefault();
                 if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        private void Simulation()
        {
           
            

           
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            maleNbrs.Clear();
            femaleNbrs.Clear();
            Simulation();
            DisplayResults();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            zaroev = int.Parse(numericUpDown1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;
            textBox1.Text = ofd.FileName;
        }
        public void DisplayResults()
        {
            var i = 0;
            for (int year = 2005; year < zaroev; year++)
            {

                richTextBox1.AppendText(string.Format("Szimulációs év: {0} \n \t Fiúk: {1} \n \t Lányok: {2} \n \n", year, maleNbrs[i], femaleNbrs[i]));
                i++;
            }
        }
    }
}
