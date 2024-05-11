using StrategoBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StrategoBeta.WPFClient
{
    public class Piece
    {
		public Piece(Character character, int row,int column)
		{
			Column = column;
			Character = character;
			Row = row;
		}
        public Piece(Character character)
        {
               Character=character;
        }
        public int Column { get; set; }
        public Character Character { get; set; }
        public int Row { get; set; }
    }
}
