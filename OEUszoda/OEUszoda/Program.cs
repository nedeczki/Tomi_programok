using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OEUszoda
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            Úszó u1 = new Úszó("u1", 4, 1);
            Úszó u2 = new Úszó("u2", 7, 2);
            Úszó u3 = new Úszó("u3", 8, 2);
            Úszó u4 = new Úszó("u4", 9, 3);
            Úszó u5 = new Úszó("u5", 7, 1);
            Úszó u6 = new Úszó("u6", 5);
            Úszó u7 = new Úszó("u7", 2);
            Úszó[] úszók = { u1, u2, u3, u4, u5, u6, u7};
            Medence medence = new Medence(1);
            int time = 0;
            int dtime = 0;
            int sorszám = 0;
            while (!medence.MindenPályaSzabad() || sorszám != úszók.Length)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("A nyitás óta eltelt idő: " + time + " perc");
                if (sorszám < úszók.Length)
                {
                    if (time == dtime)
                    {
                        if (medence.VanSzabadPálya())
                        {
                            medence.ÚjÚszó(úszók[sorszám++]);
                        }
                        else
                        {
                            medence.ÚjVárakozó(úszók[sorszám++]);
                        }
                        dtime += rnd.Next(5, 11);
                    }
                }
                medence.Print();
                medence.Halad();
                time++;
            }

            float átlag = 0;
            for (int i = 0; i < úszók.Length; i++)
            {
                átlag += úszók[i].LeúszottHosszokSzáma();
            }
            átlag /= úszók.Length;
            Console.WriteLine("Az úszók átlagosan {0} hosszt úsztal le.", Math.Round(átlag, 2));

            int maxidő = 0;
            int result = -1;
            for (int i = 0; i < úszók.Length; i++)
            {
                if (maxidő < úszók[i].Idő)
                {
                    maxidő = úszók[i].Idő;
                    result = i;
                }
            }
            Console.WriteLine("A legtöbb időt {0} nevű úszó töltötte az uszodában.", úszók[result].Név);
            Console.ReadKey();
        }
    }

    class OEUszoda
    {
        Medence medence;
        Úszó[] úszók;
        int time;

        public OEUszoda(Úszó[] úszók)
        {
            this.medence = new Medence();
            this.úszók = úszók;
            time = 0;
        }

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        public void Szimuláció()
        {
            medence.Halad();
            time++;
        }
    }

    class Úszó
    {
        string név;
        int maxHosszSzám;
        int idő;
        int tempó;

        public Úszó(string név, int maxHosszSzám)
        {
            this.név = név;
            this.maxHosszSzám = maxHosszSzám;
            this.idő = 0;
            Random rnd = new Random();
            this.tempó = rnd.Next(1,4);
        }

        public Úszó(string név, int maxHosszSzám, int tempó)
        {
            this.név = név;
            this.maxHosszSzám = maxHosszSzám;
            this.idő = 0;
            this.tempó = tempó;
        }

        public int Tempó
        {
            get { return tempó; }
            set { tempó = value; }
        }

        public int Idő
        {
            get { return idő; }
            set { idő = value; }
        }

        public int MaxHosszSzám
        {
            get { return maxHosszSzám; }
            set { maxHosszSzám = value; }
        }

        public string Név
        {
            get { return név; }
            set { név = value; }
        }

        public void Úszik()
        {
            this.idő++;
        }

        public int LeúszottHosszokSzáma() 
        {
            return (int)Math.Round((float)(this.idő/this.tempó));
        }

        public bool HosszVégénVan()
        {
            if (this.idő % this.tempó == 0)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return ""+this.név+" ("+this.LeúszottHosszokSzáma().ToString()+")";
        }
    }

    class Pálya
    {
        Úszó aktúszó;

        public Pálya()
        {
            aktúszó = null;
        }

        public Úszó Aktúszó
        {
            get { return aktúszó; }
            set { aktúszó = value; }
        }

        public void Bemegy(Úszó újÚszó)
        {
            this.aktúszó = újÚszó;
        }

        public void Kimegy()
        {
            this.aktúszó = null;
        }

        public bool Üres()
        {
            if (this.aktúszó == null)
                return true;
            else
                return false;
        }
    }

    class Medence
    {
        int pályaSzám;
        Pálya[] pályák;
        Úszó[] várakozók;

        public Pálya[] Pályák
        {
            get { return pályák; }
            set { pályák = value; }
        }

        public Medence()
        {
            this.pályaSzám = 4;
            pályák = new Pálya[this.pályaSzám];
            for (int i = 0; i < this.pályaSzám; i++)
            {
                pályák[i] = new Pálya();
            }
            várakozók = new Úszó[0];
        }

        public Medence(int pályaSzám)
        {
            this.pályaSzám = pályaSzám;
            pályák = new Pálya[pályaSzám];
            for (int i = 0; i < pályaSzám; i++)
            {
                pályák[i] = new Pálya();
            }
            várakozók = new Úszó[0];
        }

        public bool VanSzabadPálya()
        {
            for (int i = 0; i < pályaSzám; i++)
            {
                if (pályák[i].Üres())
                    return true;
            }
            return false;
        }

        public bool MindenPályaSzabad() 
        {
            for (int i = 0; i < pályaSzám; i++)
            {
                if (!pályák[i].Üres())
                    return false;
            }
            return true;
        }

        public void ÚjÚszó(Úszó újÚszó)
        {
            for (int i = 0; i < várakozók.Length; i++)
            {
                if (várakozók[i] == újÚszó)
                {
                    for (int j = i; j < várakozók.Length - 1; j++)
                        várakozók[j] = várakozók[j + 1];
                    Úszó[] temp_várakozók = new Úszó[várakozók.Length];
                    várakozók.CopyTo(temp_várakozók, 0);
                    várakozók = new Úszó[temp_várakozók.Length - 1];
                    for (int k = 0; k < várakozók.Length; k++)
                    {
                        várakozók[k] = temp_várakozók[k];
                    }
                    break;
                }
            }
            for (int i = 0; i < pályaSzám; i++)
            {
                if (pályák[i].Üres())
                {
                    pályák[i].Bemegy(újÚszó);
                    return;
                }
            }
        }

        public Pálya LegrégebbÓtaFoglaltPálya()
        {
            int max = 0;
            Pálya legrégebbÓtaFoglaltPálya = null;
            for (int i = 0; i < pályaSzám; i++)
            {
                if (!pályák[i].Üres() && pályák[i].Aktúszó.Idő > max)
                {
                    legrégebbÓtaFoglaltPálya = pályák[i];
                    max = pályák[i].Aktúszó.Idő;
                }
            }

            return legrégebbÓtaFoglaltPálya;
        }

        public void Halad()
        {
            for (int i = 0; i < pályaSzám; i++)
            {
                if (!pályák[i].Üres())
                {
                    pályák[i].Aktúszó.Úszik();
                    // Ha elérte a max hosszámot akkor kimegy az uszodából, ha nem akkor marard
                    if (pályák[i].Aktúszó.LeúszottHosszokSzáma() == pályák[i].Aktúszó.MaxHosszSzám)
                        pályák[i].Kimegy();
                }
            }

            // Végig megyünk a várakozókon, és ha van szabad pálya akkor úszik
            for (int i = 0; i < várakozók.Length; i++)
            {
                if (VanSzabadPálya())
                {
                    ÚjÚszó(várakozók[i]);
                }
                else
                {
                    if (LegrégebbÓtaFoglaltPálya().Aktúszó.HosszVégénVan())
                    {
                        LegrégebbÓtaFoglaltPálya().Kimegy();
                        ÚjÚszó(várakozók[i]);
                    }
                }
            }
        }

        public void Print()
        {
            for (int j = 0; j < Pályák.Length; j++)
            {
                if (Pályák[j].Üres())
                    Console.WriteLine("Az " + j + ". pálya üres");
                else
                    Console.WriteLine("Az " + j + ". pályán lévő úszó neve: " + Pályák[j].Aktúszó);
            }
            Console.WriteLine("Várakozó úszók: ");
            for (int i = 0; i < várakozók.Length; i++)
            {
                Console.Write(várakozók[i].Név+" ");
            }
            Console.WriteLine();
        }

        public void ÚjVárakozó(Úszó újVárakozó)
        {
            if (várakozók.Length == 0)
            {
                várakozók = new Úszó[1];
                várakozók[0] = újVárakozó;
            }
            else
            {
                Úszó[] temp_várakozó = new Úszó[várakozók.Length];
                várakozók.CopyTo(temp_várakozó, 0);
                várakozók = new Úszó[temp_várakozó.Length + 1];
                temp_várakozó.CopyTo(várakozók, 0);
                várakozók[várakozók.Length - 1] = újVárakozó;
            }
        }
    }
}
