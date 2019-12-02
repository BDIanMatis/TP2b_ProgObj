using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
    public class Héros : Protagoniste
    {
        public const char SYMBOLE = 'T';
        public Héros(Point pos) : base(SYMBOLE, pos)
        {
        }
        public override async Task<(Protagoniste, Choix)> Agir(Carte carte, CancellationToken jeton)
        {
            AfficherMenu(carte);
            await Task.Delay(1000);


            return ((this, LireChoix(jeton).Result));
        }
        static void AfficherMenu(Carte carte)
        {
            Console.WriteLine($"Carte de format {carte.Hauteur} x {carte.Largeur}");
            Console.WriteLine($"Entrez {(char)Choix.Quitter} pour quitter");
        }
        static async Task<Choix> LireChoix(CancellationToken jeton)
        {
            Console.Write("Votre choix? ");
            await Task.Run(() =>(


            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    return Choix.Quitter;
                case ConsoleKey.DownArrow:
                    return Choix.Bas;
                case ConsoleKey.UpArrow:
                    return Choix.Haut;
                case ConsoleKey.LeftArrow:
                    return Choix.Gauche;
                case ConsoleKey.RightArrow:
                    return Choix.Droite;
                default:
                    return Choix.Rien;
            });
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jeton"></param>
        /// <returns></returns>
        static async Task<ConsoleKey> LireTouche(CancellationToken jeton)
        {
            ConsoleKey touche = default;
            await Task.Run(() =>
            {
                while (!Console.KeyAvailable)
                    if (jeton.IsCancellationRequested)
                        return;
                touche = Console.ReadKey(true).Key;
            });
            return touche;
        }
    }
}
