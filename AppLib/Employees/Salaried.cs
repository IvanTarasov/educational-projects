using AppLib;

namespace CalcOfWagesLib.Employees
{
    class Salaried : Employee
    {
        public Salaried(string name)
        {
            Name = name;
            Role = Roles.Salaried;
            SalaryPerHour = 750;
            SalaryPerOvertimeHour = SalaryPerHour * 2;
        }
    }
}
