using System;
using System.Threading.Tasks;

namespace BauGrid
{
    /// <summary>
    ///     Interface de la tabla (evitamos tener que definir el genérico en las columnas
    /// </summary>
    internal interface IGridTable
    {
        /// <summary>
        ///     Registra una columna
        /// </summary>
        void Register(Column column);

        /// <summary>
        ///     Actualiza la tabla
        /// </summary>
        void Refresh();

        /// <summary>
        ///     Modifica el modo de ordenación de una columna
        /// </summary>
        Task UpdateSortModeAsync(Column column);
    }
}
