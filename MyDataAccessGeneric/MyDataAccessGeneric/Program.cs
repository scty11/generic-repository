using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataAccessGeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<EmployeeDb>());
            using (IRepository<Employee> emRes 
                = new SqlRepository<Employee>(new EmployeeDb()))
            {
                AddEmployess(emRes);
                AddManager(emRes);
                CountEmployees(emRes);
                FindEmployee(emRes);              
                DumpPeople(emRes);
                Console.Read();
            }
        }

        //using contravariance here as Manaegr is a employee.
        private static void AddManager(IWriteOnlyRepository<Manager> emRes)
        {
            emRes.Add(new Manager() { Name = "Lee" });
            emRes.Commit();
        }

        //using covariance in the generic interface to allow this.
        private static void DumpPeople(IReadOnlyRepository<Person> emRes)
        {
            var employees = emRes.FindAll();
            foreach (var item in employees)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static void FindEmployee(IRepository<Employee> emRes)
        {
            var em = emRes.FindBy(1);
            Console.WriteLine(em.Name);
        }

        private static void CountEmployees(IRepository<Employee> emRes)
        {
            Console.WriteLine(emRes.FindAll().Count());
        }

        private static void AddEmployess(IRepository<Employee> emRes)
        {
            emRes.Add(new Employee { Name = "Scott" });
            emRes.Add(new Employee { Name = "Lee" });
            emRes.Commit();
        }

        
    }
}
