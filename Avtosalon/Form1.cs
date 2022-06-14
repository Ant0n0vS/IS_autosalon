using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Avtosalon
{
    public partial class Form1 : Form
    {

        DataBase dataBase = new DataBase();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataBase.openConnection();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM Автомобили ORDER BY [Код авто]", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox4.Items.Add(Convert.ToString(sqlReader["Код авто"]) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"] + "\t\t" + sqlReader["Поставщик"].ToString() + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0,10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10) + "\t" + sqlReader["Наличие"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            sqlReader = null;
            command = new SqlCommand("SELECT * FROM Автомобили WHERE Наличие = 1 ORDER BY [Код авто]", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Код авто"]) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"] + "\t\t" + sqlReader["Поставщик"].ToString() + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0, 10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            sqlReader = null;
            command = new SqlCommand("SELECT Марка, COUNT(Марка) AS Количество FROM Автомобили GROUP BY Марка", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox2.Items.Add(sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Количество"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            sqlReader = null;
            command = new SqlCommand("SELECT Покупатели.Фамилия, Покупатели.Имя, Покупатели.Отчество, Автомобили.Марка, Автомобили.Модель, Автомобили.[Год изготовления ТС], Автомобили.Цена, Продажи.Дата FROM Покупатели INNER JOIN(Автомобили INNER JOIN Продажи ON Автомобили.[Код авто] = Продажи.[Код авто]) ON Покупатели.[Номер паспорта] = Продажи.Покупатель", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox5.Items.Add(sqlReader["Фамилия"].ToString().PadRight(10) + "\t" + sqlReader["Имя"].ToString().PadRight(12) + "\t" + sqlReader["Отчество"].ToString().PadRight(12) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"].ToString().PadRight(15) + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0, 10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Автомобили ([Код авто], Марка, Модель, Поставщик, [Год изготовления ТС], Цена, Наличие, Дата) VALUES(@Code, @Brand, @Model, @Provider, @Year, @Price, @Stock, @Date)", dataBase.getConnection());
            command.Parameters.AddWithValue("Code", textBox1.Text);
            command.Parameters.AddWithValue("Brand", textBox2.Text);
            command.Parameters.AddWithValue("Model", textBox3.Text);
            command.Parameters.AddWithValue("Provider", textBox4.Text);
            command.Parameters.AddWithValue("Year", textBox5.Text);
            command.Parameters.AddWithValue("Price", textBox7.Text);
            command.Parameters.AddWithValue("Stock", checkBox1.Checked);
            command.Parameters.AddWithValue("Date", textBox6.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись добавлена!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            checkBox1.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Автомобили WHERE [Код авто] = @Code", dataBase.getConnection());
            command.Parameters.AddWithValue("Code", textBox15.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись удалена!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            textBox15.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM Автомобили WHERE Цена < @Price AND Наличие = 1", dataBase.getConnection());
            command.Parameters.AddWithValue("Price", textBox17.Text);
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox3.Items.Add(Convert.ToString(sqlReader["Код авто"]) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"] + "\t\t" + sqlReader["Поставщик"].ToString() + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0, 10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataBase.getConnection() != null && dataBase.getConnection().State != ConnectionState.Closed)
                dataBase.closeConnection();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM Автомобили WHERE Наличие = 1 ORDER BY [Код авто]", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Код авто"]) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"] + "\t\t" + sqlReader["Поставщик"].ToString() + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0, 10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM Автомобили ORDER BY [Код авто]", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox4.Items.Add(Convert.ToString(sqlReader["Код авто"]) + "\t" + sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Модель"] + "\t\t" + sqlReader["Поставщик"].ToString() + "\t" + sqlReader["Год изготовления ТС"].ToString() + "\t" + sqlReader["Дата"].ToString().Substring(0, 10) + "\t" + sqlReader["Цена"].ToString().Substring(0, 10) + "\t" + sqlReader["Наличие"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT Марка, COUNT(Марка) AS Количество FROM Автомобили GROUP BY Марка", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox2.Items.Add(sqlReader["Марка"].ToString().PadRight(15) + "\t" + sqlReader["Количество"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("UPDATE Автомобили SET Цена = Цена * @Percent WHERE Поставщик = @Provider", dataBase.getConnection());
            command.Parameters.AddWithValue("Percent", (Convert.ToDouble(textBox16.Text) / 100 + 1));
            command.Parameters.AddWithValue("Provider", textBox8.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Цена изменена!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            textBox8.Clear();
            textBox16.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT Покупатели.[Номер паспорта], Покупатели.Фамилия, Покупатели.Имя, Покупатели.Отчество, Покупатели.Телефон INTO [Выборка покупателей] FROM Покупатели WHERE Покупатели.Адрес = @City", dataBase.getConnection());
            command.Parameters.AddWithValue("City", textBox9.Text);
            command.ExecuteNonQuery();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Выборка покупателей]", dataBase.getConnection());
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox6.Items.Add(sqlReader["Номер паспорта"].ToString() + "\t" + sqlReader["Фамилия"].ToString().PadRight(15) + "\t" + sqlReader["Имя"].ToString().PadRight(15) + "\t" + sqlReader["Отчество"].ToString().PadRight(15) + "\t" + sqlReader["Телефон"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }
    }
}
