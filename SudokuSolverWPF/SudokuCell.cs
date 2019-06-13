using System.Windows.Controls;

namespace SudokuSolverWPF
{
    class SudokuCell : TextBox
    {
        int value;
        private int posX;
        private int posY;
        public SudokuCell(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;

            //Modifying cell look
            this.Height = 45;
            this.Width = 45;
            this.FontSize = 18;
            this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            this.Margin = new System.Windows.Thickness(1);
        }

        public int Value { get => value; set => this.value = value; }
        public int PosY { get => posY; set => posY = value; }
        public int PosX { get => posX; set => posX = value; }
    }
}
