using System;
using System.Collections.Generic;

namespace BauGrid.EventArguments
{
	/// <summary>
	///		Argumento del evento de cambio de página de la tabla
	/// </summary>
	public class GridTablePageEventArgs : EventArgs
	{
		public GridTablePageEventArgs(int newPage, List<(string key, Column.SortMode sort)> sortFields)
		{
			NewPage = newPage;
			if (sortFields != null)
				SortFields = sortFields;
		}

		/// <summary>
		///		Nueva página
		/// </summary>
		public int NewPage { get; }

		/// <summary>
		///		Campos a ordenar
		/// </summary>
		public List<(string key, Column.SortMode sort)> SortFields { get; } = new List<(string key, Column.SortMode sort)>();
	}
}
