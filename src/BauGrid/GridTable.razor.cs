using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace BauGrid
{
	public partial class GridTable<TypeData> : ComponentBase, IGridTable
	{
		// Variables privadas
		private readonly Type type = typeof(TypeData);
		private readonly Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
		private readonly List<Column> filters = new List<Column>();
		private readonly List<(string label, Column.SortMode mode)> _sortColumns = new List<(string label, Column.SortMode mode)>();
		private bool first = true;

		/// <summary>
		///		Inicializa el componente
		/// </summary>
		private async Task InitComponentAsync()
		{
			await Task.Delay(1);
			// Inicializa la paginación
			ShowPager();
		}

		/// <summary>
		///		Muestra la paginación
		/// </summary>
		private void ShowPager()
		{
			// Indica si se debe mostrar la paginación
			PagesVisible = TotalRecords > PageSize && PageSize > 0;
			// Muestra los botones
			if (PagesVisible)
				Pages = TotalRecords / PageSize;
		}

		/// <summary>
		///		Modifica la página activa
		/// </summary>
		private async Task UpdatePageAsync(int newPage)
		{
			await OnChangePage.InvokeAsync(new EventArguments.GridTablePageEventArgs(newPage, _sortColumns));
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
		}

		protected override async Task OnInitializedAsync()
		{
			await InitComponentAsync();
		}

		/// <summary>
		///		Actualiza los datos
		/// </summary>
		public void Refresh()
		{
			StateHasChanged();
		}

		/// <summary>
		///		Registra una columna en la tabla
		/// </summary>
		public void Register(Column column)
		{
			// Cambia la ordenación predeterminada por la ordenación actual
			column.Sort = column.DefaultSort;
			// Asigna la ordenación
			AssignSortMode(column);
		}

		/// <summary>
		///		Asigna el modo de ordenación y actualiza la tabla
		/// </summary>
		public async Task UpdateSortModeAsync(Column column)
		{
			// Asigna el modo de ordenación
			AssignSortMode(column);
			// Actualiza el grid
			await UpdatePageAsync(Page);
		}

		/// <summary>
		///		Asigna el modo de ordenación de las columnas
		/// </summary>
		private void AssignSortMode(Column column)
		{
			string columnKey = column.Field;

				// Si no se ha definido etiqueta, se coge el título
				if (string.IsNullOrWhiteSpace(columnKey))
					columnKey = column.Label;
				// Elimina la columna de las ordenaciones
				switch (column.Sort)
				{
					case Column.SortMode.NoSort:
							for (int index = _sortColumns.Count - 1; index >= 0; index--)
								if (_sortColumns[index].label.Equals(columnKey, StringComparison.CurrentCultureIgnoreCase))
									_sortColumns.RemoveAt(index);
						break;
					default:
							bool found = false;
							List<(string key, Column.SortMode sort)> newSort = new List<(string key, Column.SortMode sort)>();

								// Añade las ordenaciones a la lista intermedia, en caso que encuentre la columna, la cambia
								foreach ((string key, Column.SortMode sort) in _sortColumns)
									if (key.Equals(columnKey, StringComparison.CurrentCultureIgnoreCase))
									 {
										// Añade la ordenación
										newSort.Add((key, column.Sort));
										// Indica que se ha encontrado
										found = true;
									}
									else
										newSort.Add((key, sort));
								// Si no se ha encontrado, se añade
								if (!found)
									newSort.Add((columnKey, column.Sort));
								// Reemplaza la lista de ordenaciones
								_sortColumns.Clear();
								_sortColumns.AddRange(newSort);
						break;
				}
		}

		/// <summary>
		///		Elementos de la tabla
		/// </summary>
		[Parameter]
		public IEnumerable<TypeData> Items { get; set; }

		/// <summary>
		///     Fragmento utilizado durante la carga
		/// </summary>
		[Parameter]
		public RenderFragment Loading { get; set; }

		/// <summary>
		///     Fragmento con los datos de la cabecera
		/// </summary>
		[Parameter]
		public RenderFragment Header { get; set; }

		/// <summary>
		///		Datos de la fila
		/// </summary>
		[Parameter]
		public RenderFragment<TypeData> Row { get; set; }

		/// <summary>
		///		Evento que se lanza cuando se pulsa sobre la fila
		/// </summary>
		[Parameter]
		public EventCallback<TypeData> OnRowClick { get; set; }

		/// <summary>
		///		Tamaño de página
		/// </summary>
		[Parameter]
		public int PageSize { get; set; } = 20;

		/// <summary>
		///		Página
		/// </summary>
		[Parameter]
		public int Page { get; set; } = 1;

		/// <summary>
		///		Número de filas
		/// </summary>
		[Parameter]
		public int TotalRecords { get; set; }

		/// <summary>
		///		Evento al que se llama al cambiar de página
		/// </summary>
		[Parameter]
		public EventCallback<EventArguments.GridTablePageEventArgs> OnChangePage { get; set; }

		/// <summary>
		///		Indica si son visibles las páginas
		/// </summary>
		protected bool PagesVisible { get; private set; }

		/// <summary>
		///		Número de páginas
		/// </summary>
		protected int Pages { get; private set; }

		/// <summary>
		///     Clase de la tabla
		/// </summary>
		[Parameter]
		public string TableCss { get; set; } = "table table-bordered table-striped table-sm";

		/// <summary>
		///     Clase de la cabecera
		/// </summary>
		[Parameter]
		public string HeaderCss { get; set; } = "bg-info";
	}
}
