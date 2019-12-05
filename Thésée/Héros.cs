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
            await Task.Run(() =>
                {
                    AfficherMenu(carte);



                    return ((this, LireChoix(jeton).Result));
                });

        }
        static void AfficherMenu(Carte carte)
        {
            Console.WriteLine($"Carte de format {carte.Hauteur} x {carte.Largeur}");
            Console.WriteLine($"Entrez {(char)Choix.Quitter} pour quitter");
        }
        static async Task<Choix> LireChoix(CancellationToken jeton)
        {
            Console.Write("Votre choix? ");
            Choix choix = default;
            await Task.Run(() =>
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q:
                        choix = Choix.Quitter;
                        break;
                    case ConsoleKey.DownArrow:
                        choix = Choix.Bas;
                        break;
                    case ConsoleKey.UpArrow:
                        choix = Choix.Haut;
                        break;
                    case ConsoleKey.LeftArrow:
                        choix = Choix.Gauche;
                        break;
                    case ConsoleKey.RightArrow:
                        choix = Choix.Droite;
                        break;
                    default:
                        choix = Choix.Rien;
                        break;
                }
            });
            return choix;
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
