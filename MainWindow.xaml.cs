using System;
using System.Linq;
using System.Windows;

namespace SortingApp
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Массив, с которым проводится работа алгоритмов
        /// </summary>
        private double[] array;
        /// <summary>
        /// Количество элементов в массиве
        /// </summary>
        private const int N = 9000;
        /// <summary>
        /// Коэффициенты для Burr
        /// </summary>
        private const double c = 2.0;
        private const double k = 5.0;
        private const double lambda = 1.0;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Генерация случайных чисел методом Burr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateArrayButton_Click(object sender, RoutedEventArgs e)
        {
            array = GenerateBurrDistributionArray(N, c, k, lambda);
            OutputTextBox1.Text = string.Join(", ", array.Take(50)) + "...";
            OutputTextBox2.Text = string.Join(", ", array.Take(50)) + "...";
            OutputTextBox3.Text = string.Join(", ", array.Take(50)) + "...";
        }

        /// <summary>
        /// Генерация распределения методом Burr
        /// </summary>
        /// <param name="size">Размер Т</param>
        /// <param name="c">Коэффициент C</param>
        /// <param name="k">Коэффициент k</param>
        /// <param name="lambda">Коэффициент lambda</param>
        /// <returns>Массив чисел, сгенерированный методом Burr</returns>
        private double[] GenerateBurrDistributionArray(int size, double c, double k, double lambda)
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
        /// Обработка простых методов сортировки и вывод на экран
        /// </summary>
        private void SimpleSortButton_Click(object sender, RoutedEventArgs e)
        {
            if (array == null) return;

            var bubbleSorted = (double[])array.Clone();
            BubbleSort(bubbleSorted);

            var selectionSorted = (double[])array.Clone();
            SelectionSort(selectionSorted);

            var insertionSorted = (double[])array.Clone();
            InsertionSort(insertionSorted);

            OutputTextBox1.Text = "Сортировка пузырьком: " + string.Join(", ", bubbleSorted.Take(50)) + "...\n";
            OutputTextBox2.Text = "Сортировка выборкой: " + string.Join(", ", selectionSorted.Take(50)) + "...\n";
            OutputTextBox3.Text = "Сортировка вставкой: " + string.Join(", ", insertionSorted.Take(50)) + "...";
        }

        /// <summary>
        /// Обработка сложных методов сортировка и вывод на экран
        /// </summary>
        private void ComplexSortButton_Click(object sender, RoutedEventArgs e)
        {
            if (array == null) return;

            var quickSorted = (double[])array.Clone();
            QuickSort(quickSorted, 0, quickSorted.Length - 1);

            var mergeSorted = (double[])array.Clone();
            MergeSort(mergeSorted, 0, mergeSorted.Length - 1);

            var heapSorted = (double[])array.Clone();
            HeapSort(heapSorted);

            OutputTextBox1.Text = "Быстрая сортировка: " + string.Join(", ", quickSorted.Take(50)) + "...\n";
            OutputTextBox2.Text = "Сортировка слиянием: " + string.Join(", ", mergeSorted.Take(50)) + "...\n";
            OutputTextBox3.Text = "Пирамидальная сортировка: " + string.Join(", ", heapSorted.Take(50)) + "...";
        }

        /// <summary>
        /// Сортировка пузырьком
        /// </summary>
        /// <param name="array">Входной массив</param>
        private void BubbleSort(double[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }
        }

        /// <summary>
        /// Сортировка выборкой
        /// </summary>
        /// <param name="array">Входной массив</param>
        private void SelectionSort(double[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }
                Swap(ref array[minIndex], ref array[i]);
            }
        }

        /// <summary>
        /// Сортировка вставкой
        /// </summary>
        /// <param name="array">Входной массив</param>
        private void InsertionSort(double[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                double key = array[i];
                int j = i - 1;
                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = key;
            }
        }

        /// <summary>
        /// Быстрая сортирвока
        /// </summary>
        /// <param name="array">Входной массив</param>
        /// <param name="left">Элемент слева</param>
        /// <param name="right">Элемент справа</param>
        private void QuickSort(double[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right);
                QuickSort(array, left, pivot - 1);
                QuickSort(array, pivot + 1, right);
            }
        }

        /// <summary>
        /// Деление массива по принципу "разделяй и властвуй"
        /// </summary>
        /// <param name="array">Массив для деления</param>
        /// <param name="left">Элемент слева</param>
        /// <param name="right">Элемент справа</param>
        /// <returns>Следующий центральный элемент</returns>
        private int Partition(double[] array, int left, int right)
        {
            double pivot = array[right];
            int i = left - 1;
            for (int j = left; j < right; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    Swap(ref array[i], ref array[j]);
                }
            }
            Swap(ref array[i + 1], ref array[right]);
            return i + 1;
        }

        /// <summary>
        /// Сортировка массива слиянием
        /// </summary>
        /// <param name="array">Входной</param>
        /// <param name="left">Левый элемент</param>
        /// <param name="right">Правый элемент</param>
        private void MergeSort(double[] array, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(array, left, mid);
                MergeSort(array, mid + 1, right);
                Merge(array, left, mid, right);
            }
        }

        /// <summary>
        /// Сортировка слиянием
        /// </summary>
        /// <param name="array">Входной массив</param>
        /// <param name="left">Элемент слева</param>
        /// <param name="mid">Центральный элемент</param>
        /// <param name="right">Элемент справа</param>
        private void Merge(double[] array, int left, int mid, int right)
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
        /// Пирамидальная сортировка
        /// </summary>
        /// <param name="array">Входной массив</param>
        private void HeapSort(double[] array)
        {
            int n = array.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);
            for (int i = n - 1; i > 0; i--)
            {
                Swap(ref array[0], ref array[i]);
                Heapify(array, i, 0);
            }
        }

        /// <summary>
        /// Алгоритм пирамидальной сортировки
        /// </summary>
        /// <param name="array">Входной массив</param>
        /// <param name="n">Число</param>
        /// <param name="i">Наибольшее число</param>
        private void Heapify(double[] array, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && array[left] > array[largest])
                largest = left;

            if (right < n && array[right] > array[largest])
                largest = right;

            if (largest != i)
            {
                Swap(ref array[i], ref array[largest]);
                Heapify(array, n, largest);
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
    }
}
