using System;
using System.Collections.Generic;
using static Thésée.Algos;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

      static État AppliquerChoix(Carte carte, List<(Protagoniste qui, Choix quoi)> choix)
      {
         foreach (var chx in choix)
         {
            if (chx.quoi == Choix.Quitter)
               return État.Quitter;
            else if (EstEntre(chx.quoi, Choix.Droite, Choix.Bas))
               if (Déplacer(carte, chx.qui, deltas[(int)chx.quoi]))
               {
                  if (EstVictoireVilain(carte))
                     return État.VictoireVilain;
                  if (EstVictoireHéros(carte))
                     return État.VictoireHéros;
               }
         }
         return État.Poursuivre;
      }
      static List<(Protagoniste, Choix)> ExécuterTour(Carte carte, Protagoniste[] protagonistes)
      {
         var choix = new List<(Protagoniste, Choix)>();
         Afficher(carte);
         foreach (Protagoniste p in protagonistes)
            choix.Add((p, p.Agir(carte)));
         return choix;
      }
      static void TerminerPartie(Carte carte, État état)
      {
         Console.BackgroundColor = ConsoleColor.Black;
         Afficher(carte);
         switch (état)
         {
            case État.VictoireHéros:
               Console.WriteLine("Le héros s'est échappé, ouf!");
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
         État état;
         var choix = ExécuterTour(carte, protagonistes);
         while ((état = AppliquerChoix(carte, choix)) == État.Poursuivre)
            choix = ExécuterTour(carte, protagonistes);
         TerminerPartie(carte, état);
      }
   }
}
