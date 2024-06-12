using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace SortingApp
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Массив, с которым проводится работа алгоритмов
        /// </summary>
        public double[] array;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Генерация распределения методом Burr
        /// </summary>
        /// <param name="size">Размер Т</param>
        /// <param name="c">Коэффициент C</param>
        /// <param name="k">Коэффициент k</param>
        /// <param name="lambda">Коэффициент lambda</param>
        /// <returns>Массив чисел, сгенерированный методом Burr</returns>
        public double[] GenerateBurrDistributionArray(int size, double c, double k, double lambda)
        {
            Random rand = new Random();
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
            {
                double u = rand.NextDouble();
                result[i] = lambda * Math.Pow(Math.Pow(1 / (1 - u), 1 / k) - 1, 1 / c);
            }
            return result;
        }

        /// <summary>
        /// Упорядочить массив сортировкой пузырьком
        /// </summary>
        public (double[], int, int, long) BubbleSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    comparisons++;
                    if (array[j] > array[j + 1])
                    {
                        Swap(ref array[j], ref array[j + 1]);
                        swaps++;
                    }
                }
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }

        /// <summary>
        /// Упорядочить массив сортировкой выборкой
        /// </summary>
        public (double[], int, int, long) SelectionSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    comparisons++;
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    Swap(ref array[minIndex], ref array[i]);
                    swaps++;
                }
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }

        /// <summary>
        /// Упорядочить массив сортировкой вставкой
        /// </summary>

        public (double[], int, int, long) InsertionSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 1; i < array.Length; i++)
            {
                double key = array[i];
                int j = i - 1;
                comparisons++;
                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    swaps++;
                    j--;
                    comparisons++;
                }
                array[j + 1] = key;
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }


        /// <summary>
        /// Упорядочить массив быстрой сортировкой
        /// </summary>
        public (double[], int, int, long) QuickSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            QuickSortHelper(array, 0, array.Length - 1, ref comparisons, ref swaps);

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }

        /// <summary>
        /// Вспомогательная функция для быстрой сортировки
        /// </summary>
        private void QuickSortHelper(double[] array, int left, int right, ref int comparisons, ref int swaps)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right, ref comparisons, ref swaps);
                QuickSortHelper(array, left, pivot - 1, ref comparisons, ref swaps);
                QuickSortHelper(array, pivot + 1, right, ref comparisons, ref swaps);
            }
        }

        /// <summary>
        /// Разбить массив по принципу "разделяй и властвуй"
        /// </summary>
        private int Partition(double[] array, int left, int right, ref int comparisons, ref int swaps)
        {
            double pivot = array[right];
            int i = left - 1;
            for (int j = left; j < right; j++)
            {
                comparisons++;
                if (array[j] <= pivot)
                {
                    i++;
                    Swap(ref array[i], ref array[j]);
                    swaps++;
                }
            }
            Swap(ref array[i + 1], ref array[right]);
            swaps++;
            return i + 1;
        }

        /// <summary>
        /// Упорядочить массив сортировкой слияния
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public (double[], int, int, long) MergeSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            MergeSortHelper(array, 0, array.Length - 1, ref comparisons, ref swaps);

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }

        /// <summary>
        /// Вспомогательная функция для сортировки слиянием
        /// </summary>
        private void MergeSortHelper(double[] array, int left, int right, ref int comparisons, ref int swaps)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSortHelper(array, left, mid, ref comparisons, ref swaps);
                MergeSortHelper(array, mid + 1, right, ref comparisons, ref swaps);
                Merge(array, left, mid, right, ref comparisons, ref swaps);
            }
        }

        /// <summary>
        /// Сортировка слиянием
        /// </summary>
        public void Merge(double[] array, int left, int mid, int right, ref int comparisons, ref int swaps)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            double[] leftArray = new double[n1];
            double[] rightArray = new double[n2];

            Array.Copy(array, left, leftArray, 0, n1);
            Array.Copy(array, mid + 1, rightArray, 0, n2);

            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                comparisons++;
                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }

        /// <summary>
        /// Упорядочить массив пирамидальной сортировкой
        /// </summary>
        public (double[], int, int, long) HeapSort(double[] array)
        {
            int comparisons = 0;
            int swaps = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            int n = array.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i, ref comparisons, ref swaps);
            for (int i = n - 1; i > 0; i--)
            {
                Swap(ref array[0], ref array[i]);
                swaps++;
                Heapify(array, i, 0, ref comparisons, ref swaps);
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return (array, comparisons, swaps, elapsedTime);
        }

        /// <summary>
        /// Пирамидальная сортировка массива
        /// </summary>
        private void Heapify(double[] array, int n, int i, ref int comparisons, ref int swaps)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n)
            {
                comparisons++;
                if (array[left] > array[largest])
                    largest = left;
            }

            if (right < n)
            {
                comparisons++;
                if (array[right] > array[largest])
                    largest = right;
            }

            if (largest != i)
            {
                Swap(ref array[i], ref array[largest]);
                swaps++;
                Heapify(array, n, largest, ref comparisons, ref swaps);
            }
        }

        /// <summary>
        /// Меняет два элемнта местами
        /// </summary>
        /// <param name="a">Первый элемент</param>
        /// <param name="b">Второй элемент</param>
        private void Swap(ref double a, ref double b)
        {
            (b, a) = (a, b);
        }

        /// <summary>
        /// Выводит характеристики всех методов сортировки в сравнительную таблицу
        /// </summary>
        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            double c;
            double k;
            double lambda;
            int count;

            try
            {

                c = double.Parse(C.Text);
                k = double.Parse(K.Text);
                lambda = double.Parse(Lambda.Text);
                count = int.Parse(N.Text);

                array = GenerateBurrDistributionArray(count, c, k, lambda);

                OriginalArrayGrid.ItemsSource = array.Select((val, idx) =>
                {
                    return new DistributionEntry
                    {
                        Number = idx + 1,
                        Value = val
                    };
                });
            }
            catch
            {
                MessageBox.Show(
                    "Введите вещественные коэффициенты с, k, λ, и целое число N",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            var (_, comparisonCount1, swapCount1, timeInMs1) = BubbleSort((double[])array.Clone());
            var (_, comparisonCount2, swapCount2, timeInMs2) = SelectionSort((double[])array.Clone());
            var (_, comparisonCount3, swapCount3, timeInMs3) = InsertionSort((double[])array.Clone());
            var (_, comparisonCount4, swapCount4, timeInMs4) = QuickSort((double[])array.Clone());
            var (_, comparisonCount5, swapCount5, timeInMs5) = MergeSort((double[])array.Clone());
            var (_, comparisonCount6, swapCount6, timeInMs6) = HeapSort((double[])array.Clone());

            AnalysisGrid.ItemsSource = new List<AnalysisEntry> {
                 new AnalysisEntry {
                    MethodName="Сортировка пузырьком",
                    ComparisonCount=comparisonCount1,
                    PermutationCount=swapCount1,
                    TimeInMs=timeInMs1
                 },
                 new AnalysisEntry {
                    MethodName="Сортировка выборкой",
                    ComparisonCount=comparisonCount2,
                    PermutationCount=swapCount2,
                    TimeInMs=timeInMs2
                 },
                 new AnalysisEntry {
                    MethodName="Сортировка вставкой",
                    ComparisonCount=comparisonCount3,
                    PermutationCount=swapCount3,
                    TimeInMs=timeInMs3
                 },
                 new AnalysisEntry {
                    MethodName="Быстрая сортировка",
                    ComparisonCount=comparisonCount4,
                    PermutationCount=swapCount4,
                    TimeInMs=timeInMs4
                 },
                 new AnalysisEntry {
                    MethodName="Сортировка слиянием",
                    ComparisonCount=comparisonCount5,
                    PermutationCount=swapCount5,
                    TimeInMs=timeInMs5
                 },
                 new AnalysisEntry {
                    MethodName="Пирамидальная сортировка",
                    ComparisonCount=comparisonCount6,
                    PermutationCount=swapCount6,
                    TimeInMs=timeInMs6
                 }
                };
        }

        /// <summary>
        /// Завершает работу программы
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Очищает поля ввода
        /// </summary>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            OriginalArrayGrid.ItemsSource = new List<DistributionEntry>();
            SortedArrayGrid.ItemsSource = new List<DistributionEntry>();
            AnalysisGrid.ItemsSource = new List<AnalysisEntry>();
            C.Text = string.Empty;
            K.Text = string.Empty;
            Lambda.Text = string.Empty;
            N.Text = string.Empty;
            ComparisonCountBox.Text = string.Empty;
            PermutationCountBox.Text = string.Empty;
            SortTimeInMsBox.Text = string.Empty;
        }

        /// <summary>
        /// Сортирует массив, определяя время сортировки по заданному методу
        /// </summary>
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            double c;
            double k;
            double lambda;
            int count;

            try
            {

                c = double.Parse(C.Text);
                k = double.Parse(K.Text);
                lambda = double.Parse(Lambda.Text);
                count = int.Parse(N.Text);

                array = GenerateBurrDistributionArray(count, c, k, lambda);

                OriginalArrayGrid.ItemsSource = array.Select((val, idx) =>
                {
                    return new DistributionEntry
                    {
                        Number = idx + 1,
                        Value = val
                    };
                });
            }
            catch
            {
                MessageBox.Show(
                    "Введите вещественные коэффициенты с, k, λ, и целое число N",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            if (BubbleSortRadio.IsChecked.HasValue && BubbleSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = BubbleSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
            else if (SelectionSortRadio.IsChecked.HasValue && SelectionSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = SelectionSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
            else if (InsertSortRadio.IsChecked.HasValue && InsertSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = InsertionSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
            else if (QuickSortRadio.IsChecked.HasValue && QuickSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = QuickSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
            else if (MergeSortRadio.IsChecked.HasValue && MergeSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = MergeSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
            else if (HeapSortRadio.IsChecked.HasValue && HeapSortRadio.IsChecked.Value)
            {
                var (sortedArray, comparisonCount, swapCount, timeInMs) = HeapSort((double[])array.Clone());

                AssignValues(sortedArray, comparisonCount, swapCount, timeInMs);
            }
        }

        /// <summary>
        /// Выводит характерстики результата сортировки на экран
        /// </summary>
        private void AssignValues(double[] array, int comparisonCount, int swapCount, long timeInMs)
        {
            SortedArrayGrid.ItemsSource = array.Select((val, idx) =>
            {
                return new DistributionEntry
                {
                    Number = idx + 1,
                    Value = val
                };
            });

            ComparisonCountBox.Text = comparisonCount.ToString();
            PermutationCountBox.Text = swapCount.ToString();
            SortTimeInMsBox.Text = timeInMs.ToString();
        }

        /// <summary>
        /// Открывает окно для анализа с экспериментальными данными
        /// на основе выбранной сортировки
        /// </summary>
        private void OpenAnalysisWindow_Click(object sender, RoutedEventArgs e)
        {
            double c;
            double k;
            double lambda;

            try
            {

                c = double.Parse(C.Text);
                k = double.Parse(K.Text);
                lambda = double.Parse(Lambda.Text);
            }
            catch
            {
                MessageBox.Show(
                    "Введите вещественные коэффициенты с, k, λ для анализа",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            new AnalysisWindow(this, c, k, lambda).ShowDialog();
        }
    }
}
