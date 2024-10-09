using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            CourseRepository repository = new CourseRepository();
            repository.InitializeDatabase(); // Initialize the database and table
            int choice;

            do
            {
                Console.WriteLine("Course Management System:");
                Console.WriteLine("1. Add a Course");
                Console.WriteLine("2. View All Courses");
                Console.WriteLine("3. Update a Course");
                Console.WriteLine("4. Delete a Course");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Enter Course Title: ");
                        string title = Console.ReadLine();
                        Console.Write("Enter Course Duration: ");
                        string duration = Console.ReadLine();
                        Console.Write("Enter Course Price: ");
                        decimal price;
                        while (!repository.ValidateCoursePrice(price = decimal.Parse(Console.ReadLine())))
                        {
                        }
                        repository.AddCourse(new Course(0, title, duration, price));
                        Console.WriteLine("Course added successfully.");
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("List of Courses:");
                        foreach (var course in repository.GetAllCourses())
                        {
                            Console.WriteLine(course);
                        }
                        break;

                    case 3:
                        Console.Clear();
                        Console.Write("Enter the Course ID to update: ");
                        int updateId = int.Parse(Console.ReadLine());
                        var existingCourse = repository.GetCourseById(updateId);
                        if (existingCourse != null)
                        {
                            Console.Write("Enter new Title: ");
                            string newTitle = Console.ReadLine();
                            Console.Write("Enter new Duration: ");
                            string newDuration = Console.ReadLine();
                            Console.Write("Enter new Price: ");
                            decimal newPrice;
                            while (!repository.ValidateCoursePrice(newPrice = decimal.Parse(Console.ReadLine())))
                            {
                            }
                            existingCourse.Title = newTitle;
                            existingCourse.Duration = newDuration;
                            existingCourse.Price = newPrice;
                            repository.UpdateCourse(existingCourse);
                            Console.WriteLine("Course updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Course not found.");
                        }
                        break;

                    case 4:
                        Console.Clear();
                        Console.Write("Enter the Course ID to delete: ");
                        int deleteId = int.Parse(Console.ReadLine());
                        repository.DeleteCourse(deleteId);
                        Console.WriteLine("Course deleted successfully.");
                        break;

                    case 5:
                        Console.Clear();
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }

            } while (choice != 5);
        }
    }
}

