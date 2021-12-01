using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMSProject
{
    class FlipTable
    {
        private static String EMPTY = "(empty)";
        private static String ANSI_COLORS = "\u001B\\[[;\\d]*m";

        /** Create a new table with the specified headers and row data. */
        public static String of(String[] headers, String[,] data)
        {
            if (headers == null) throw new NullReferenceException("headers == null");
            if (headers.Length == 0) throw new ArgumentException("Headers must not be empty.");
            if (data == null) throw new NullReferenceException("data == null");
            return new FlipTable(headers, data).ToString();
        }

        private String[] headers;
        private String[,] data;
        private int columns;
        private int[] columnWidths;
        private int emptyWidth;

        private FlipTable(String[] headers, String[,] data)
        {
            this.headers = headers;
            this.data = data;

            columns = headers.Length;
            columnWidths = new int[columns];
            for (int row = -1; row < data.Length; row++)
            {
                String[] rowData = (row == -1) ? headers : Enumerable.Range(0, data.GetLength(1))
                .Select(x => data[row,x])
                .ToArray(); // Hack to parse headers too.

                if (rowData.Length != columns)
                {
                    throw new ArgumentException(
                            String.Format("Row {0}'s {1}s columns != {2}s columns", row + 1, rowData.Length, columns));
                }
                for (int column = 0; column < columns; column++)
                {
                    foreach (String rowDataLine in rowData[column].Split("\\n"))
                    {
                        String rowDataWithoutColor = rowDataLine.Replace(ANSI_COLORS, "");
                        columnWidths[column] = Math.Max(columnWidths[column], rowDataWithoutColor.Length);
                    }
                }
            }

            int emptyWidth = 3 * (columns - 1); // Account for column dividers and their spacing.
            foreach (int columnWidth in columnWidths)
            {
                emptyWidth += columnWidth;
            }
            this.emptyWidth = emptyWidth;

            if (emptyWidth < EMPTY.Length)
            { // Make sure we're wide enough for the empty text.
                columnWidths[columns - 1] += EMPTY.Length - emptyWidth;
            }
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            printDivider(builder, "╔═╤═╗");
            printData(builder, headers);
            if (data.Length == 0)
            {
                printDivider(builder, "╠═╧═╣");
                builder.Append('║').Append(pad(emptyWidth, EMPTY)).Append("║\n");
                printDivider(builder, "╚═══╝");
            }
            else
            {
                for (int row = 0; row < data.Length; row++)
                {
                    printDivider(builder, row == 0 ? "╠═╪═╣" : "╟─┼─╢");
                    printData(builder, Enumerable.Range(0, data.GetLength(1))
                .Select(x => data[row,x])
                .ToArray());

                }
                printDivider(builder, "╚═╧═╝");
            }
            return builder.ToString();
        }

        private void printDivider(StringBuilder outt, String format)
        {
            for (int column = 0; column < columns; column++)
            {
                outt.Append(column == 0 ? format[0] : format[2]);
                outt.Append(pad(columnWidths[column], "").Replace(' ', format[1]));
            }
            outt.Append(format[4]).Append('\n');
        }

        private void printData(StringBuilder outt, String[] data)
        {
            for (int line = 0, lines = 1; line < lines; line++)
            {
                for (int column = 0; column < columns; column++)
                {
                    outt.Append(column == 0 ? '║' : '│');
                    String[] cellLines = data[column].Split("\\n");
                    lines = Math.Max(lines, cellLines.Length);
                    String cellLine = line < cellLines.Length ? cellLines[line] : "";
                    outt.Append(pad(columnWidths[column], cellLine));
                }
                outt.Append("║\n");
            }
        }

        private static String pad(int width, String data)
        {
            return String.Format(" %1$-" + width + "s ", data);
        }
    }
}
