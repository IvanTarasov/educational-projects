using AppLib;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalcOfWagesLib.Data_Base
{
    class EmployeesRecord
    {
        public string Name { get; private set; }
        public string Role { get; private set; }

        private static readonly string pathToDB = "C:\\Users\\ivant\\source\\repos\\EducationalProjects\\";

        public EmployeesRecord(Employee employee)
        {
            Name = employee.Name;
            switch (employee.Role)
            {
                case Employee.Roles.Leader:
                    Role = "Руководитель";
                    break;
                case Employee.Roles.Salaried:
                    Role = "Сотрудник";
                    break;
                case Employee.Roles.Freelancer:
                    Role = "Фрилансер";
                    break;
            }
        }

        public void Save()
        {
            EmployeesRecord record = (EmployeesRecord)MemberwiseClone();

            List<EmployeesRecord> records = ReadRecords(pathToDB + "employees.csv");
            foreach (var rec in records)
            {
                if (rec.Name == record.Name)
                {
                    throw new Exception("Сотрудник с таким именем уже существует.");
                }
            }

            records.Add(record);

            using (StreamWriter streamReader = new StreamWriter(pathToDB + "employees.csv", false))
            {
                using (CsvWriter csvReader = new CsvWriter((ISerializer)streamReader))
                {
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.WriteRecords(records);
                }
            }
        }

        public static List<EmployeesRecord> ReadRecords(string pathToFile)
        {
            using (StreamReader streamReader = new StreamReader( pathToDB + pathToFile))
            {
                using (CsvReader csvReader = new CsvReader((IParser)streamReader))
                {
                    csvReader.Configuration.Delimiter = ",";
                    return (List<EmployeesRecord>)csvReader.GetRecords<EmployeesRecord>();
                }
            }
        }
    }
}
