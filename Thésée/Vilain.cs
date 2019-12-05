using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
   public class Vilain : Protagoniste, IMoniteurDéplacement
   {
      public const char SYMBOLE = 'M';
      const double SEUIL_PROXIMITÉ = 4;
      bool Proche
      {
         get => PositionCible.Distance(Position) <= SEUIL_PROXIMITÉ;
      }

      Point PositionCible { get; set; }
      public bool Autoriser(Point avant, Point après)
      {
         PositionCible = après;
         return true;
      }

      Random De { get; } = new Random();
      public Vilain(Point pos) : base(SYMBOLE, pos)
      {
      }

        public override async Task<(Protagoniste, Choix)> Agir(Carte carte, CancellationToken jeton)
        {
            await Task.Delay(100, jeton);
            int faireMouvement;

            if (!jeton.IsCancellationRequested)
            {
                faireMouvement = De.Next(1, 5);
                if (faireMouvement == 1)
                    return ((this, (Choix)De.Next((int)Choix.Droite, (int)Choix.Bas)));
                else
                    return ((this, Choix.Rien));
            }
            else
                return ((this, Choix.Rien));
        }

      protected override bool DéplacerImpl(Point nouvellePosition)
      {
         Position = nouvellePosition;
         Console.BackgroundColor = Proche ? ConsoleColor.Red : ConsoleColor.Black;
         return true;
      }
   }
}
