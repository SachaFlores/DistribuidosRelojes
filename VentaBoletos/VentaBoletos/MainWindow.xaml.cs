using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Reflection.Emit;


namespace VentaBoletos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> opciones = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            comboBox.ItemsSource = opciones;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=DESKTOP-1TAVE7B\\SQLEXPRESS;Database=AirPabon;User Id=sa;Password=univalle;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Conectado correctamente a la base de datos.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar: " + ex.Message);
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-1TAVE7B\\SQLEXPRESS;Database=AirPabon;User Id=sa;Password=univalle;"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
            SELECT C.Nombre AS Ciudad, P.Nombre AS Pais
            FROM Ciudad C
            INNER JOIN Pais P ON C.PaisId = P.IdPais", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string ciudad = reader["Ciudad"].ToString();
                    string pais = reader["Pais"].ToString();
                    opciones.Add($"{ciudad}, {pais}");
                }

                comboBox.ItemsSource = opciones;
            }
        }
        private void comboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string texto = comboBox.Text.ToLower();

            // Obtener la TextBox interna del ComboBox
            var textBox = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
            if (textBox == null) return;

            // Guardar la posición del cursor
            int caretIndex = textBox.CaretIndex;

            // Filtrar opciones
            var filtrados = opciones
                .Where(op => op.ToLower().Contains(texto))
                .ToList();

            comboBox.ItemsSource = filtrados;
            comboBox.IsDropDownOpen = true;

            // Restaurar el texto y la posición del cursor
            comboBox.Text = texto;
            textBox.CaretIndex = caretIndex;
        }
    }
}
