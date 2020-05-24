using System;

namespace BauGrid.Test.Components
{
	/// <summary>
	///		Argumento del evento de cambio de página de la tabla
	/// </summary>
	public class TableTemplatePageEventArgs : EventArgs
	{
		public TableTemplatePageEventArgs(int newPage)
		{
			NewPage = newPage;
		}

		/// <summary>
		///		Nueva página
		/// </summary>
		public int NewPage { get; }
	}
}
