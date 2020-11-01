using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalcOfWagesLib.Data_Base
{
    class HoursWorkedRecord
    {
        public string Date { get; private set; }
        public string EmployeeName { get; private set; }
        public byte WorkedHours { get; private set; }
        public string Comment { get; private set; }

        private static readonly string pathToDB = "C:\\Users\\ivant\\source\\repos\\EducationalProjects\\";

        public HoursWorkedRecord(DateTime date, string employeeName, byte workedHours, string comment)
        {
            Date = date.ToShortDateString();
            EmployeeName = employeeName;
            WorkedHours = workedHours;
            Comment = comment;
        }

        public void Save(string fileName)
        {
            List<HoursWorkedRecord> records = ReadRecords(pathToDB + fileName);
            HoursWorkedRecord currentRecord = (HoursWorkedRecord)MemberwiseClone();

            // если запись с той же датой уже существует, то часы работы складываются, иначе исключение
            foreach (var record in records)
            {
                if (currentRecord.Date == record.Date)
                {
                    if (currentRecord.WorkedHours + record.WorkedHours <= 24)
                    {
                        currentRecord.WorkedHours += record.WorkedHours;
                        records.Insert(records.IndexOf(record), currentRecord);
                        break;
                    }
                    else
                    {
                        throw new Exception("Количество часов работы в данный день (" + currentRecord.Date + ") превышает 24 часа.");
                    }
                }
                else if (records.IndexOf(record) == records.Count - 1)
                {
                    records.Add(currentRecord);
                    break;
                }
            }

            using (StreamWriter streamReader = new StreamWriter(fileName, false))
            {
                using (CsvWriter csvReader = new CsvWriter((ISerializer)streamReader))
                {
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.WriteRecords(records);
                }
            }
        }

        public static List<HoursWorkedRecord> ReadRecords(string pathToFile)
        {
            using (StreamReader streamReader = new StreamReader(pathToDB + pathToFile))
            {
                using (CsvReader csvReader = new CsvReader((IParser)streamReader))
                {
                    csvReader.Configuration.Delimiter = ",";
                    return (List<HoursWorkedRecord>)csvReader.GetRecords<HoursWorkedRecord>();
                }
            }
        }
    }
}
