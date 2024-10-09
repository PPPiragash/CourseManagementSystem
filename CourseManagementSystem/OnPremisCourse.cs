using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementSystem
{
    public class OnPremisCourse : Course
    {
        public string Schedule { get; set; }
        public int ClassroomCapacity { get; set; }

        public OnPremisCourse(int courseId, string title, string duration, decimal price, string schedule, int classroomCapacity)
            : base(courseId, title, duration, price)
        {
            Schedule = schedule;
            ClassroomCapacity = classroomCapacity;
        }

        public override string DisplayCourseInfo()
        {
            return $"{base.DisplayCourseInfo()}, Schedule: {Schedule}, Capacity: {ClassroomCapacity}";
        }
    }


}
