using System.Collections.Generic;
using System.Windows;

namespace SortingApp
{
    /// <summary>
    /// Interaction logic for AnalysisWindow.xaml
    /// </summary>
    public partial class AnalysisWindow : Window
    {
        private readonly MainWindow parentWindow;
        private readonly double c;
        private readonly double k;
        private readonly double lambda;
        private List<ExperimentalEntry> entries = new List<ExperimentalEntry>();

        public AnalysisWindow(MainWindow parentWindow, double c, double k, double lambda)
        {
            InitializeComponent();

            this.parentWindow = parentWindow;
            this.c = c;
            this.k = k;
            this.lambda = lambda;
        }

        /// <summary>
        /// Расчитывает экспериментальные данные.
        /// </summary>
        private void CalculateExperimentalRows_Click(object sender, RoutedEventArgs e)
        {
            ExperimentalGrid.ItemsSource = new List<ExperimentalEntry>();
            entries = new List<ExperimentalEntry>();

            for (long i = 10000; i < 100000; i += 10000)
            {
                AddRow(i);
            }

            ExperimentalGrid.ItemsSource = entries;
        }

        /// <summary>
        /// Добавляет результат вычисления характерстик сортировки в таблицу.
        /// </summary>
        /// <param name="i">Количество элементов</param>
        private void AddRow(long i)
        {
            var array = parentWindow.GenerateBurrDistributionArray((int)i, c, k, lambda);
            double[] sortedArray;
            int comparisonCount;
            int swapCount;
            long timeInMs;

            if (parentWindow.BubbleSortRadio.IsChecked.HasValue && parentWindow.BubbleSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.BubbleSort((double[])array.Clone());
            }
            else if (parentWindow.SelectionSortRadio.IsChecked.HasValue && parentWindow.SelectionSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.SelectionSort((double[])array.Clone());
            }
            else if (parentWindow.InsertSortRadio.IsChecked.HasValue && parentWindow.InsertSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.InsertionSort((double[])array.Clone());
            }
            else if (parentWindow.QuickSortRadio.IsChecked.HasValue && parentWindow.QuickSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.QuickSort((double[])array.Clone());
            }
            else if (parentWindow.MergeSortRadio.IsChecked.HasValue && parentWindow.MergeSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.MergeSort((double[])array.Clone());
            }
            else if (parentWindow.HeapSortRadio.IsChecked.HasValue && parentWindow.HeapSortRadio.IsChecked.Value)
            {
                (sortedArray, comparisonCount, swapCount, timeInMs) = parentWindow.HeapSort((double[])parentWindow.array.Clone());
            }
            else
            {
                return;
            }

            entries.Add(new ExperimentalEntry
            {
                Number = i / 10000,
                ElementCount = i,
                SquaredX = i * i,
                TimeInMs = timeInMs,
                XY = timeInMs * i
            });
        }

        /// <summary>
        /// Строит график на основе имеющихся данных.
        /// </summary>
        private void BuildChart_Click(object sender, RoutedEventArgs e)
        {
            AnalysisChart.Series[0].Points.Clear();

            entries.ForEach(entry =>
            {
                AnalysisChart.Series[0].Points.AddXY(entry.ElementCount, entry.TimeInMs);
            });
        }

        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            if (entries.Count == 0)
            {
                MessageBox.Show(
                    "Сначала рассчитайте экспериментальные данные",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }


        }
    }
}
