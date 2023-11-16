using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DataSource
{
    public class DatabaseHandler : IDisposable
    {
        private SqlConnection connection;

        // Konstruktor klasy, inicjalizuje połączenie na podstawie dostarczonego ciągu połączenia
        public DatabaseHandler(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        // Metoda otwierająca połączenie z bazą danych
        public void OpenConnection()
        {
            // Sprawdza, czy połączenie jest zamknięte, a następnie je otwiera
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // Metoda zamykająca połączenie z bazą danych
        public void CloseConnection()
        {
            // Sprawdza, czy połączenie jest otwarte, a następnie je zamyka
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Metoda do wykonywania zapytań SQL i pobierania danych (DataReader) z bazy danych
        public SqlDataReader ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Jeśli dostarczono parametry, dodaje je do zapytania SQL
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                // Sprawdza, czy połączenie jest zamknięte, a następnie je otwiera
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                // Wykonuje zapytanie i zwraca wynik w postaci DataReader
                return cmd.ExecuteReader();
            }
        }

        // Metoda do wykonywania zapytań SQL, które nie zwracają wyników (np. INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, List<SqlParameter> parameters = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Jeśli dostarczono parametry, dodaje je do zapytania SQL
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                // Sprawdza, czy połączenie jest zamknięte, a następnie je otwiera
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                // Wykonuje zapytanie, nie oczekując wyników, i zwraca liczbę zmodyfikowanych wierszy
                return cmd.ExecuteNonQuery();
            }
        }

        // Metoda do pobierania pojedynczej wartości (skalarnej) z wyników zapytania SQL
        public T ExecuteScalar<T>(string query, List<SqlParameter> parameters = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Jeśli dostarczono parametry, dodaje je do zapytania SQL
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                // Sprawdza, czy połączenie jest zamknięte, a następnie je otwiera
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                // Wykonuje zapytanie i zwraca pojedynczą wartość (skalar) jako obiekt
                object result = cmd.ExecuteScalar();

                // Konwertuje wynik na oczekiwany typ generyczny i zwraca go
                if (result != null && result != DBNull.Value)
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                // Jeśli wynik jest pusty lub nieprawidłowy, zwraca wartość domyślną dla danego typu
                return default(T);
            }
        }

        // Implementacja interfejsu IDisposable do zwalniania zasobów
        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
        }
    }
}
