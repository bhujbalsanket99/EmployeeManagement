﻿namespace EmployeeManagement.Models
{
    public class EmployeeViewModel
    {
        public List<Employee>? Employees { get; set; }
        public Employee Employee { get; set; }
        public string Operation { get; set; }
    }
}
