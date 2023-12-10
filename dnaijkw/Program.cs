namespace dnaijkw
{
    internal class Program
    {
        class Osszetevo
        {
            private string elnevezes;
            private int mennyiseg;
            private OsszetevoFajta fajta;

            public string Elnevezes
            {
                get { return elnevezes; }
                set { if (value != null) { elnevezes = value; } }
            }

            public int Mennyiseg
            {
                get { return mennyiseg; }
            }

            public OsszetevoFajta Fajta
            {
                get { return fajta; }
            }

            public Osszetevo(string elnevezes, int mennyiseg, OsszetevoFajta fajta)
            {
                this.elnevezes = elnevezes;
                this.mennyiseg = mennyiseg;
                this.fajta = fajta;
            }

            public string Szovegge()
            {
                return $"{mennyiseg} ml {elnevezes} ({fajta})";
            }
        }
            public enum OsszetevoFajta
            {
            Folyadek,
            Alkohol,
            Egyeb
            }


        class Koktel
        {
            private string nev;
            public List<Osszetevo> osszetevok;

            public string Nev
            {
                get { return nev; }
            }

            private Osszetevo Hozzaad(string line)
            {
                string[] data = line.Split(',');
                OsszetevoFajta fajta = (OsszetevoFajta)Enum.Parse(typeof(OsszetevoFajta), (data[0] == "Alkohol") ? "Alkohol" : (data[0] == "Folyadek" ? "Folyadek" : "Egyeb"));
                string nev = data[1];
                int mennyiseg = Convert.ToInt32(data[2]);

                Osszetevo osszetevo = new Osszetevo(nev, mennyiseg, fajta);
                return osszetevo;
            }

            public Koktel(string nev, string filepath)
            {
                this.nev = nev;
                osszetevok = new List<Osszetevo>();
                StreamReader sr = new StreamReader(filepath);
                string line = "";
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    osszetevok.Add(Hozzaad(line));
                }
                sr.Close();
            }

            private int OsszAlkoholtartalom()
            {
                int osszAlkohol = 0;

                foreach (Osszetevo osszetevo in osszetevok)
                {
                    if(osszetevo.Fajta == OsszetevoFajta.Alkohol)
                    {
                        osszAlkohol += osszetevo.Mennyiseg;
                    }
                }

                return osszAlkohol; 
            }

            public OsszetevoFajta MaximalisMennyiseg()
            {
                int maxNum = 0;
                OsszetevoFajta fajta = OsszetevoFajta.Folyadek;

                foreach (Osszetevo osszetevo in osszetevok)
                {
                    if(osszetevo.Mennyiseg > maxNum)
                    {
                        maxNum = osszetevo.Mennyiseg;
                        fajta = osszetevo.Fajta;
                    }
                }

                return fajta;
            }

            private void Csoportosit()
            {
                List<Osszetevo> sortedList = new List<Osszetevo>();

                foreach (Osszetevo osszetevo in osszetevok)
                {
                    if(osszetevo.Fajta == OsszetevoFajta.Alkohol)
                    {
                        sortedList.Add(osszetevo);
                    }
                }

                foreach (Osszetevo osszetevo in osszetevok)
                {
                    if (!sortedList.Contains(osszetevo))
                    {
                        sortedList.Add(osszetevo);
                    }
                }

                osszetevok = sortedList;    
            }

            public void ReceptNyomtatas()
            {
                Csoportosit();
                StreamWriter sw = new StreamWriter($"{nev}.txt");
                foreach (Osszetevo osszetevo in osszetevok)
                {
                    sw.WriteLine(osszetevo.Szovegge());
                }
                sw.WriteLine($"\n-- Teljes alkoholtartalom: {OsszAlkoholtartalom()} ml --");
                sw.Close(); 
            }
        }
        static void Main(string[] args)
        {
            Koktel koktel = new Koktel("asd", "osszetevok.txt");
            koktel.ReceptNyomtatas();
        }
    }
}