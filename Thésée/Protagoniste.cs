using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
    public enum Choix { Droite, Haut, Gauche, Bas, Quitter = 'q', Rien }
    public enum Orientation { Est, Nord, Ouest, Sud }
    public interface IMoniteurDéplacement
    {
        bool Autoriser(Point avant, Point après);
    }
    public abstract class Protagoniste
    {
        List<IMoniteurDéplacement> Surveillants { get; } = new List<IMoniteurDéplacement>();
        public Point Position { get; protected set; }
        // pour usage futur
        public Orientation Direction { get; private set; } = Orientation.Nord;
        public char Symbole { get; }
        public Protagoniste(char symbole, Point pos)
        {
            Symbole = symbole;
            Position = pos;
        }
        public void Abonner(IMoniteurDéplacement surveillant)
        {
            if (surveillant == null)
                throw new ArgumentNullException();
            Surveillants.Add(surveillant);
        }
        public bool Déplacer(Point nouvellePosition) =>
           Surveillants.All(
              s => s.Autoriser(Position, nouvellePosition)
           ) && DéplacerImpl(nouvellePosition);
        protected virtual bool DéplacerImpl(Point nouvellePosition)
        {
            Position = nouvellePosition;
            return true;
        }
        public virtual async Task<(Protagoniste, Choix)> Agir(Carte carte)
        {
            return (this, Choix.Rien);
        }
    }
}
