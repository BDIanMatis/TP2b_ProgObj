using System;
using System.Collections.Generic;
using static Thésée.Algos;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//console.readkey lit et lorsque le shit cancel, le readline ne cancel pas

//boucle est ce que je dois canceler

//si je veux pas canceler je dois mettre mon readkey

namespace Thésée
{
    class Program
    {
        static void Afficher(Carte carte)
        {
            carte.Afficher();
        }

        class DéplacementIllégalException : Exception { }

        static bool Déplacer(Carte carte, Protagoniste p, Point delta)
        {
            Point nouvellePos = p.Position + delta;
            if (p.Position.Distance(nouvellePos) > 1)
                throw new DéplacementIllégalException();
            if (carte[nouvellePos.Y, nouvellePos.X] == '#')
                return false;
            // insérer un observateur de déplacements
            p.Déplacer(nouvellePos);
            return true;
        }

        enum État { Poursuivre, Quitter, VictoireHéros, VictoireVilain }
        static bool EstVictoireHéros(Carte carte) =>
           carte.Trouver(Carte.SYMBOLE_BONHEUR).Count != 0;
        static bool EstVictoireVilain(Carte carte) =>
           carte.Trouver(Carte.SYMBOLE_DÉCÈS).Count != 0;


        static readonly Point[] deltas = new[]
        {
         new Point(1, 0), new Point(0, -1),
         new Point(-1, 0), new Point(0, 1)
      };

        static async Task<État> AppliquerChoix(Carte carte, Task<(Protagoniste qui, Choix quoi)> choix)
        {
            return await Task.Run(() =>
            {
                if (choix.Result.quoi == Choix.Quitter)
                    return État.Quitter;
                else if (EstEntre(choix.Result.quoi, Choix.Droite, Choix.Bas))
                    if (Déplacer(carte, choix.Result.qui, deltas[(int)choix.Result.quoi]))
                    {
                        if (EstVictoireVilain(carte))
                            return État.VictoireVilain;
                        if (EstVictoireHéros(carte))
                            return État.VictoireHéros;
                    }
                return État.Poursuivre;
            });
        }

        static async Task<(Protagoniste, Choix)> ExécuterTour(Carte carte, Protagoniste[] protagonistes)
        {
            var choix = new List<Task<(Protagoniste, Choix)>>();
            CancellationTokenSource source = new CancellationTokenSource();

            foreach (Protagoniste p in protagonistes)
                choix.Add((p.Agir(carte, source.Token)));

            Task<(Protagoniste, Choix)> premier = await Task.WhenAny(choix);

            return premier.Result;
        }
        static void TerminerPartie(Carte carte, État état)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Afficher(carte);
            switch (état)
            {
                case État.VictoireHéros:
                    Console.WriteLine("Le héros s'est échappé, oof!");
                    break;
                case État.VictoireVilain:
                    Console.WriteLine("Oh non, le héros a été dévoré!");
                    break;
            }
            Console.WriteLine("Au revoir!");
        }
        static void Main(string[] args)
        {

            Carte carte = FabriqueCarte.Créer(args.Length == 0 ?
             "../../CarteTest.txt" : args[0]);
            Héros héros = new Héros(carte.Trouver(Héros.SYMBOLE)[0]);
            Vilain vilain = new Vilain(carte.Trouver(Vilain.SYMBOLE)[0]);
            var protagonistes = new Protagoniste[] { héros, vilain };
            foreach (var p in protagonistes)
                p.Abonner(carte);
            héros.Abonner(vilain);
            var état = Task.WhenAny(DemarrerPartie(carte, protagonistes));
            TerminerPartie(carte, état.Result.Result);
        }

        static async Task<État> DemarrerPartie(Carte carte, Protagoniste[] protagonistes)
        {
            return await Task.Run(() =>
            {
                do
                {
                    carte.Afficher();
                } while (AppliquerChoix(carte, ExécuterTour(carte, protagonistes)).Result == État.Poursuivre);
                return AppliquerChoix(carte, ExécuterTour(carte, protagonistes)).Result;
            });
        }
    }

}