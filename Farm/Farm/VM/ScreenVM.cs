using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace Farm.VM
{
    internal class ScreenVM : VMBase
    {
        private ObservableCollection<Animal> _animals;
        public ObservableCollection<Animal> Animals { get { return _animals; } set { _animals = value; OnPropertyChanged(nameof(Animals)); } }
        private int? _numCow;
        public int? NumCow
        {
            get { return _numCow; }
            set { _numCow = value; OnPropertyChanged(nameof(NumCow)); }
        }

        private int? _numSheep;
        public int? NumSheep
        {
            get { return _numSheep; }
            set { _numSheep = value; OnPropertyChanged(nameof(NumSheep)); }
        }
        private int? _numGoat;
        public int? NumGoat
        {
            get { return _numGoat; }
            set { _numGoat = value; OnPropertyChanged(nameof(_numGoat)); }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand showCommand { get; set; }
        public ICommand showVoiceCommand { get; set; }

        public ScreenVM()
        {
            SaveCommand = new RelayCommand<object>((p) => true, (p) => WriteIn());
            showCommand = new RelayCommand<object>((p) => true, (p) => LoadData());
            showVoiceCommand = new RelayCommand<object>((p) => true, p => LoadVoice());
        }
        private void WriteIn()
        {
            if (NumCow == null || NumGoat == null || NumSheep == null)
            {
                MessageBox.Show("Vui long dien day du thong tin.");
                return;
            }
            using (SqlConnection connection = new SqlConnection("Data Source=Admin;Initial Catalog=FARM;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                var command = new SqlCommand("delete from CONVAT", connection);
                int rowA1 = command.ExecuteNonQuery();

                var command2th = new SqlCommand("insert into CONVAT (TEN,SOLUONG,SOSUA) values (@ten,@soluong,0)", connection);
                command2th.Parameters.AddWithValue("@ten", "Bo");
                command2th.Parameters.AddWithValue("@soluong", NumCow);
                int rowA2 = command2th.ExecuteNonQuery();

                var command3th = new SqlCommand("insert into CONVAT (TEN,SOLUONG,SOSUA) values (@ten,@soluong,0)", connection);
                command3th.Parameters.AddWithValue("@ten", "Cuu");
                command3th.Parameters.AddWithValue("@soluong", NumSheep);
                command3th.ExecuteNonQuery();


                var command4th = new SqlCommand("insert into CONVAT (TEN,SOLUONG,SOSUA) values (@ten,@soluong,0)", connection);
                command4th.Parameters.AddWithValue("@ten", "De");
                command4th.Parameters.AddWithValue("@soluong", NumGoat);
                int rowA3 = command4th.ExecuteNonQuery();

                if (rowA1 > 0 && rowA2 > 0 && rowA3 > 0)
                {
                    MessageBox.Show("Cap nhat thanh cong");
                }
                else MessageBox.Show("Co loi xay ra");
            }

        }
        private void LoadVoice()
        {
            string theVoice = "";
            using (SqlConnection connection = new SqlConnection("Data Source=Admin;Initial Catalog=FARM;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                var command = new SqlCommand("select TEN, SOLUONG, SOSUA from CONVAT", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int sl = reader.GetInt32(1);
                        while (sl > 0)
                        {
                            if (reader.GetString(0) == "Bo")
                                theVoice += "Boo ";
                            else if (reader.GetString(0) == "Cuu")
                                theVoice += "Bee ";
                            else theVoice += "Eee ";
                            --sl;
                        }
                    }

                }
                MessageBox.Show("Tieng keu nghe duoc :" + theVoice);
            }
        }
        private void LoadData()
        {
            var data = new ObservableCollection<Animal>();
            using (SqlConnection connection = new SqlConnection("Data Source=Admin;Initial Catalog=FARM;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                var command = new SqlCommand("select TEN, SOLUONG, SOSUA from CONVAT", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Animal
                        {
                            Name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Num = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            Milk = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        });
                    }
                    Animals = data;
                }
            }
        }
    }

}
