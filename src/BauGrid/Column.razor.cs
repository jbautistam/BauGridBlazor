using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace BauGrid
{
    /// <summary>
    ///     Componente con los datos de una columna
    /// </summary>
    public partial class Column : ComponentBase
    {
        // Variables privadas
        private Dictionary<object, bool> _filters = new Dictionary<object, bool>();
        private bool _all = true;

        /// <summary>
        ///     Modo de ordenación
        /// </summary>
        public enum SortMode
        {
            /// <summary>No está ordenado</summary>
            NoSort,
            /// <summary>Ascendente</summary>
            Ascending,
            /// <summary>Descendente</summary>
            Descending
        }
        /// <summary>
        ///     Alineación
        /// </summary>
        public enum Align
        {
            /// <summary>A la izquierda</summary>
            Left,
            /// <summary>Centrada</summary>
            Center,
            /// <summary>A la derecha</summary>
            Right
        }

        public void InitFilter(IEnumerable<object> values)
        {
            _filters = values.ToDictionary(k => k, v => true);
        }

        public bool Include(object value) 
        {
            return _filters[value];
        }

        protected override void OnInitialized()
        {
            Table.Register(this);
        }

        /// <summary>
        ///     Modifica el modo de ordenación de la celda
        /// </summary>
        private async Task UpdateSortModeAsync()
        {
            if (Sortable)
            {
                // Cambia el modo de ordenación
                switch (Sort)
                {
                    case SortMode.Ascending:
                            Sort = SortMode.Descending;
                        break;
                    case SortMode.Descending:
                            Sort = SortMode.NoSort;
                        break;
                    case SortMode.NoSort:
                            Sort = SortMode.Ascending;
                        break;
                }
                // Asigna el modo de ordenación a la tabla
                await Table.UpdateSortModeAsync(this);
            }
        }

        private void Filter()
        {
            Table.Refresh();
        }

        private void Filter(object value)
        {
            _filters[value] = !_filters[value];

            _all = _filters.All(kv => kv.Value);

            Filter();
        }

        private void All()
        {
            _all = !_all;

            foreach (var key in _filters.Keys.ToList())
            {
                _filters[key] = _all;
            }

            Filter();
        }

        /// <summary>
        ///     Tabla a la que se asocia la columna
        /// </summary>
        [CascadingParameter]
        internal IGridTable Table { get; set; }

        /// <summary>
        ///     Nombre del campo
        /// </summary>
        [Parameter]
        public string Field { get; set; }

        /// <summary>
        ///     Etiqueta
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        ///     Indica si se puede ordenar por esta columna
        /// </summary>
        [Parameter]
        public bool Sortable { get; set; }

        /// <summary>
        ///     Ordenación predeterminada
        /// </summary>
        [Parameter]
        public SortMode DefaultSort { get; set; }

        /// <summary>
        ///     Indica si el elemento se puede filtrar
        /// </summary>
        [Parameter]
        public bool Filterable { get; set; }

        /// <summary>
        ///     Modo de ordenación
        /// </summary>
        public SortMode Sort { get; set; } = SortMode.NoSort;

        /// <summary>
        ///     Css del indicador de ordenación
        /// </summary>
        internal MarkupString SortIndicator
        { 
            get
            {
                string html = string.Empty;

                    // Obtiene el html del elemento
                    if (Sortable)
                    {
                        // Cabecera
                        html = "<i class='ml-2 fas fa-fw ";
                        // Añade la ordenación
                        switch (Sort)
                        {
                            case SortMode.Ascending:
                                    html += "fa-sort-up";
                                break;
                            case SortMode.Descending:
                                    html += "fa-sort-down";
                                break;
                            case SortMode.NoSort:
                                    html += "fa-sort";
                                break;
                        }
                        // Añade el final de la etiqueta
                        html += "'></i>";
                    }
                    // Devuelve la cadena
                    return (MarkupString) html;
            }
        }
/*
			@if (Sortable)
			{
				@if (Sort != SortMode.NoSort)
				{
					<i class="ml-2 fas fa-fw @(Sort == SortMode.Descending ? "fa-sort-down" : "fa-sort-up")"></i>
				}
				else
				{
					<i class="ml-2 fas fa-fw fa-sort"></i>
				}
			}
*/
        /// <summary>
        ///     Indica si se debe mostrar la ventana de filtros
        /// </summary>
        public bool ShowFilter { get; set; }
    }
}
