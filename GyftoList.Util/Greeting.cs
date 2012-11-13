using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GyftoList.Util
{
    public class Greeting
    {
        #region Constructor

        public Greeting()
        {
            _greeting = new string[] { "Hello",
                "Hi",
                "Good day",
                "Hallo",
                "Chíkmaa",
                "Selam",
                "Bees-e-lees-e",
                "Barev",
                "Grubgott",
                "Salam",
                "Heyello",
                "Kaixo",
                "Pryvitannie",
                "Namaskar",
                "Wai",
                "Koali",
                "Hej",
                "Mingalarba",
                "Ola",
                "Hola",
                "Sga-noh",
                "Hafa adai",
                "Moni bambo",
                "Kia orana",
                "Tansi",
                "Bok",
                "Dobre rano",
                "In-i-che",
                "Shorah",
                "Hutch-e-lul-lul-o",
                "Hoi",
                "Goedendag",
                "Kuzu-zangpo",
                "Koyo",
                "Hey",
                "Yo",
                "How do you do",
                "HowDo",
                "Watchya",
                "Alright",
                "Hiya",
                "Howzitgaun",
                "Saluton",
                "Tere paevast",
                "Salaam Alekum",
                "Bula Uro",
                "Hyvaa paivaa",
                "Salut",
                "Allo",
                "Bonjour",
                "Goeie dei",
                "Dia duit",
                "Gamardjoba",
                "Guten Tag",
                "Grub Gott",
                "Moin",
                "Gruezi",
                "Namaste",
                "Ina kwaana",
                "Aloha",
                "Shalom",
                "Nde-ewo",
                "Selamat pagi",
                "Dia duit",
                "Ciao",
                "Wah gwaan",
                "Kwe kwe",
                "Namaskara",
                "NuqneH",
                "Namaskar",
                "Choni",
                "Sabaidee",
                "Salve",
                "Labdien",
                "Mbote",
                "Laba diena",
                "Coi",
                "Namaskkaram",
                "Selamat datang",
                "Kihineth",
                "Kia ora",
                "Tena koe",
                "Namaskar",
                "Iakwe",
                "Sain baina uu",
                "Niltze",
                "Atetgrealot",
                "Faka lofa lahi atu",
                "Cia",
                "Jwajalapa",
                "Namaskar",
                "Dumelang",
                "Hei",
                "Asham",
                "Alii",
                "Aaarrrrgh",
                "Eyhay",
                "Dzien dobry",
                "Sat sri akal",
                "Khamma Ghani sa",
                "Salut",
                "Privet",
                "Talofa",
                "Haja",
                "Salamaleikum",
                "Zdravo",
                "Hoezit",
                "Buaregh",
                "Hola",
                "Marot",
                "Tja",
                "Ia orana",
                "Li-ho",
                "Vanakkam",
                "Namaskaram",
                "Bondia",
                "Tashi delek",
                "Cho demo",
                "Malo e lelei",
                "Moyo",
                "Minjhani",
                "Merhaba",
                "Dobriy ranok",
                "Assalomu Alaykum",
                "Adaab",
                "Xin chao",
                "Shwmae",
                "Sholem aleikhem",
                "E kaaro",
                "Sawubona"};
        }

        #endregion

        #region Private Members

        private string[] _greeting = null;

        #endregion

        #region

        public string GetRandomGreeting()
        {
            var r = new Random();
            int rNum = r.Next(0, 99);
            return _greeting[rNum];
        }

        #endregion
    }
}
