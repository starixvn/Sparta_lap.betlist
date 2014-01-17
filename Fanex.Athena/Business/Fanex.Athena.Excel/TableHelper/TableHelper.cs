// -----------------------------------------------------------------------
// <copyright file="TableHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Excel.TableHelper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TableHelpers
    {
        public DataTable GetGroupedBy(DataTable _dtSource, IList<string> _groupByColumnNames, IList<DataTableAggregateFunction> _fieldsForCalculation)
        {
            try
            {
                //Once the columns are added find the distinct rows and group it bu the numbet
                DataTable _dtReturn = _dtSource.DefaultView.ToTable(true, _groupByColumnNames.ToArray());

                //The column names in data table
                foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
                {
                    _dtReturn.Columns.Add(_calculatedField.OutPutColumnName, typeof(decimal));
                }

                //Gets the collection and send it back
                for (int i = 0; i < _dtReturn.Rows.Count; i = i + 1)
                {
                    #region Gets the filter string
                    string _filterString = string.Empty;
                    for (int j = 0; j < _groupByColumnNames.Count; j = j + 1)
                    {
                        if (j > 0)
                        {
                            _filterString += " AND ";
                        }
                        if (_dtReturn.Columns[_groupByColumnNames[j]].DataType == typeof(System.Int32))
                        {
                            _filterString += _groupByColumnNames[j] + " = " + _dtReturn.Rows[i][_groupByColumnNames[j]].ToString() + "";
                        }
                        else
                        {
                            _filterString += _groupByColumnNames[j] + " = '" + _dtReturn.Rows[i][_groupByColumnNames[j]].ToString() + "'";
                        }
                    }
                    #endregion

                    #region Compute the aggregate command

                    foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
                    {
                        _dtReturn.Rows[i][_calculatedField.OutPutColumnName] = Convert.ToDecimal(_dtSource.Compute(_calculatedField.enmFunction.ToString() + "(" + _calculatedField.ColumnName + ")", _filterString));
                    }

                    #endregion
                }

                return _dtReturn;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}