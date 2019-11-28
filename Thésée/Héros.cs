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
      public override async Task<Choix> Agir(Carte carte)
      {
         AfficherMenu(carte);
         return LireChoix();
      }
      static void AfficherMenu(Carte carte)
      {
         Console.WriteLine($"Carte de format {carte.Hauteur} x {carte.Largeur}");
         Console.WriteLine($"Entrez {(char)Choix.Quitter} pour quitter");
      }
      static Choix LireChoix()
      {
         Console.Write("Votre choix? ");
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
         }
      }
   }
}
