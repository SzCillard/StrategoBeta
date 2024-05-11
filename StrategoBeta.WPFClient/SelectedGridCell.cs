using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoBeta.WPFClient
{
	internal class SelectedGridCell
	{
		public SelectedGridCell(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public int Row {  get; set; }
		public int Column { get; set; }
		
	}
}
