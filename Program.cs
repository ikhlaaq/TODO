using System;
using System.Data.SqlClient;
using TODO.Domain;
using static System.Console;


namespace TODO
{
    class Program
    {


        static int taskIdCounter = 1;
        // 3 delar: adress till instans ; databasnamn ; authentisering
        // static string connectionString = "Data Source=.;Initial Catalog=TODO-g;Integrated Security = true";
        //static string connectionString = "Data Source=(local);Initial Catalog=TODO-g;Integrated Security = true";
        // static string connectionString = "Data Source=localhost;Initial Catalog=TODO-g;Integrated Security = true";

        //static string connectionString = "Server=.;Initial Catalog=TODO-g;Integrated Security = true";
        static string connectionString = "Server=.;Database=TODO-g;Integrated Security = true";
        static void Main(string[] args)
        {
            bool shouldRun = true;



            while (shouldRun)
            {

                Clear();

                WriteLine("1. Add todo");
                WriteLine("2. List todo");
                WriteLine("3. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:

                        Write("Title:");

                        string title = ReadLine();

                        Write("Due date (yyyy-mm-dd hh:mm): ");

                        DateTime dueDate = DateTime.Parse(ReadLine());

                        CreateTask(title, dueDate);

                        break;

                    case ConsoleKey.D2:

                        WriteLine("ID  Title                   Due date    Completed   ");
                        WriteLine("----------------------------------------------------");

                        var tasks = FetchAllTasks();

                        foreach (var task in tasks)
                        {
                            if (task == null) continue;

                            WriteLine($"{task.Id}  {task.Title}{task.DueDate.ToString().PadLeft(25, ' ')}");
                        }

                        ReadKey(true);

                        break;


                    case ConsoleKey.D3:

                        shouldRun = false;
                        break;
                }
            }
        }

        private static Task[] FetchAllTasks()
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string sql = @"SELECT Id
                          ,Title
                          ,DueDate
                          ,Completed
                     FROM Task";


            SqlCommand command = new SqlCommand(sql, connection);


            connection.Open();

            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                string id = dataReader["Id"].ToString();
                string title = dataReader["Title"].ToString();
                string dueDate = dataReader["Duedate"].ToString();
                string completed = dataReader["Completed"].ToString();

                Write(id.PadRight(5, ' '));
                Write(title.PadRight(30, ' '));
                Write(dueDate.PadRight(20, ' '));
                WriteLine(completed);

            }

            WriteLine("Successfully connected to database manager instance");

            connection.Close();


            return new Task[100];
        }

        private static void CreateTask(string title, DateTime dueDate)
        {
            Task[] tasks = FetchAllTasks();

            tasks[GetIndexPosition()] = new Task(taskIdCounter++, title, dueDate);

        }

        static int GetIndexPosition()
        {

            Task[] tasks = FetchAllTasks();


            int result = -1;
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i] != null)
                {
                    continue;
                }
                if (tasks[i] == null)
                {
                    result = i;
                    break;
                }
                if (result == -1)
                {
                    throw new Exception("No avalible position");
                }
            }
            return result;

        }
    }
}
