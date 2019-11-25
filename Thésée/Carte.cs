using System;
using System.Collections.Generic;
using System.Linq;
using static Thésée.Algos;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
   public class Carte : IMoniteurDéplacement
   {
      public const char SYMBOLE_MUR = '#';
      public const char SYMBOLE_VIDE = ' ';
      public const char SYMBOLE_DÉCÈS = 'X';
      public const char SYMBOLE_BONHEUR = '!';
      char[,] Représentation { get; set; }
      public char this [int ligne, int colonne]
      {
         get => Représentation[ligne, colonne];
      }
      public void Afficher()
      {
         Console.Clear();
         for (int i = 0; i != Hauteur; ++i)
         {
            for (int j = 0; j != Largeur; ++j)
               Console.Write($"{this[i, j]}");
            Console.WriteLine();
         }
      }
      public bool Autoriser(Point avant, Point après)
      {
         if (!EstDans(après) || avant.Distance(après) > 1)
            return false;
         if (this[après.Y, après.X] == SYMBOLE_VIDE)
         {
            Permuter(ref Représentation[avant.Y, avant.X],
                     ref Représentation[après.Y, après.X]);
            if(EstSurFrontière(après) && this[après.Y, après.X] == Héros.SYMBOLE)
               Représentation[après.Y, après.X] = SYMBOLE_BONHEUR;
         }
         else
         {
            // char symbole = this[après.Y, après.X];
            Représentation[avant.Y, avant.X] = SYMBOLE_VIDE;
            Représentation[après.Y, après.X] = SYMBOLE_DÉCÈS;
         }
         Afficher();
         return true;
      }
      public bool EstDans(Point p) =>
         EstEntre(p.X, 0, Largeur - 1) &&
         EstEntre(p.Y, 0, Hauteur - 1);
      public bool EstSurFrontière(Point p) =>
         p.X == 0 || p.X == Largeur - 1 ||
         p.Y == 0 || p.Y == Hauteur - 1;
      public int Hauteur { get => Représentation.GetLength(0); }
      public int Largeur { get => Représentation.GetLength(1); }
      internal Carte(char [,] source)
      {
         Représentation = new char[source.GetLength(0), source.GetLength(1)];
         for (int i = 0; i != Hauteur; ++i)
            for (int j = 0; j != Largeur; ++j)
               Représentation[i, j] = source[i, j];
      }
      public List<Point> Trouver(char symbole)
      {
         var pts = new List<Point>();
         for (int i = 0; i != Hauteur; ++i)
            for (int j = 0; j != Largeur; ++j)
               if (this[i, j] == symbole)
                  pts.Add(new Point(j, i));
         return pts;
      }
   }
   public class PasDeCarteException : Exception { }
   public class CarteNonRectangulaireException : Exception { }
   public class SymboleManquantException : Exception
   {
      public char Symbole { get; }
      public SymboleManquantException(char symbole)
      {
         Symbole = symbole;
      }
   }
   public class SymboleIllégalException : Exception
   {
      public char Symbole { get; }
      public SymboleIllégalException(char symbole)
      {
         Symbole = symbole;
      }
   }
   public static class FabriqueCarte
   {
      static readonly char[] symbolesRequis = new[]
      {
         Héros.SYMBOLE, Vilain.SYMBOLE
      };
      static readonly char[] symbolesPermis = new[]
      {
         Carte.SYMBOLE_MUR, Carte.SYMBOLE_VIDE,
         Héros.SYMBOLE, Vilain.SYMBOLE
      };
      static Carte Valider(Carte carte)
      {
         foreach (char c in symbolesRequis)
            if (carte.Trouver(c).Count == 0)
               throw new SymboleManquantException(c);
         for(int i = 0; i != carte.Hauteur; ++i)
            for(int j = 0; j != carte.Largeur; ++j)
               if(!symbolesPermis.Contains(carte[i,j]))
                 throw new SymboleIllégalException(carte[i, j]);
         return carte;
      }
      static List<string> Valider(List<string> lst)
      {
         if (lst.Count == 0 || lst[0].Length == 0)
            throw new PasDeCarteException();
         int largeurRéférence = lst[0].Length;
         if (!lst.All(s => s.Length == largeurRéférence))
            throw new CarteNonRectangulaireException();
         return lst;
      }
      // précondition : lst est valide
      static char[,] ToData(List<string> lst)
      {
         char[,] data = new char[lst.Count, lst[0].Length];
         for (int i = 0; i != lst.Count; ++i)
            for (int j = 0; j != lst[i].Length; ++j)
               data[i, j] = lst[i][j];
         return data;
      }
      public static Carte Créer(string nomFichier)
      {
         var reader = new System.IO.StreamReader(nomFichier);
         var lst = new List<string>();
         for (string s = reader.ReadLine();
              s != null; s = reader.ReadLine())
            lst.Add(s);
         return Valider(new Carte(ToData(Valider(lst))));
      }
   }
}
