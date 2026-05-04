using FinalProjectSGMS;
using System;
using System.Linq;

Users user = new Users();
Console.WriteLine("Please login: ");
Console.Write("Username: ");
user.Username = Console.ReadLine();
Console.Write("Password: ");
user.Password = Console.ReadLine();

TrainingDbContext dbContext = new TrainingDbContext();

using (dbContext)
{
    var loggedInUser = dbContext.Users
        .FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

    if (loggedInUser == null)
    {
        Console.WriteLine("Invalid login");
        return;
    }

    if (loggedInUser.Username.StartsWith("admin"))
    {
        AdminMenu(dbContext);
    }
    else if(loggedInUser.Username.StartsWith("teacher"))
    {
        TeacherMenu(dbContext);
    }
}

// ================= ADMIN =================
static void AdminMenu(TrainingDbContext dbContext)
{
    Console.WriteLine();
    Console.WriteLine("===============Admin Menu====================");
    Console.WriteLine("Welcome admin,please select an option ");
    Console.WriteLine("1)Create class");
    Console.WriteLine("2)Create subject");
    Console.WriteLine("3)Create teacher");
    Console.WriteLine("4)View classes");
    Console.WriteLine("5)View subjects");
    Console.WriteLine("6)View teachers");

    Console.Write("Your choice: ");
    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            Console.WriteLine("Please provide following information to create a new class:");
            Console.Write("Class name: ");
            dbContext.Classes.Add(new SchoolClass
            {
                ClassName = Console.ReadLine()
            });
            
            break;

        case 2:
            Console.WriteLine("Please provide following information to create a new subject:");
            Console.Write("Subject name: ");
            string subjectName = Console.ReadLine();

            var cls = GetClassByName(dbContext);

            dbContext.Subjects.Add(new SubjectClass
            {
                SubjectName = subjectName,
                ClassId = cls.Id
            });
            
            break;

        case 3:
            Console.WriteLine("Please provide following information to create a new teacher:");
            Console.Write("Teacher name: ");
            string name = Console.ReadLine();

            Console.Write("Teacher username: ");
            string tu = Console.ReadLine();

            Console.Write("Teacher password: ");
            string tp = Console.ReadLine();

            dbContext.Teachers.Add(new Teacher
            {
                Id = Guid.NewGuid(),
                TeacherName = name,
                Username = tu,
                Password = tp
            });

            dbContext.Users.Add(new Users
            {
                Id = Guid.NewGuid(),
                Username = tu,
                Password = tp
            });



            break;

        case 4:
            Console.WriteLine("Following classes are present in the system:");
            dbContext.Classes.ToList()
                .ForEach(c => Console.WriteLine(c.ClassName));
            Console.WriteLine("What you want to do");
            Console.WriteLine("1) Edit class");
            Console.WriteLine("2) Delete class");
            Console.WriteLine("3) Assign subject");
           
            Console.Write("Your choice: ");
            int choice2 = int.Parse(Console.ReadLine());

            //Edit class
            if (choice2 == 1)
            {
                Console.WriteLine("Provide information to edit class:");
                var cls2 = GetClassByName(dbContext);
                Console.Write("New class name: ");
                cls2.ClassName = Console.ReadLine();
                dbContext.Classes.Update(cls2);

            }

            //Delete class
            else if (choice2 == 2)
            {
                
                var cls2 = GetClassByName(dbContext);
                Console.WriteLine("Provide information to delete a class:");
                dbContext.Classes.Remove(cls2);
            }

            //Assign subject
            else if (choice2 == 3)
            {
                var cls2 = GetClassByName(dbContext);
                Console.WriteLine("Provide information to assign a subject to class:");
                Console.Write("Subject name: ");
                string subjectName2 = Console.ReadLine();
                dbContext.Subjects.Add(new SubjectClass
                {
                    SubjectName = subjectName2,
                    ClassId = cls2.Id
                });
            }

            break;

        case 5:
            Console.WriteLine("Following subjects are present in the system:");
            dbContext.Subjects.ToList()
                .ForEach(s => Console.WriteLine(s.SubjectName));
            Console.WriteLine("What you want to do");
            Console.WriteLine("1) Edit subject");
            Console.WriteLine("2) Delete subject");
            Console.WriteLine("3) Assign a teacher");

            Console.Write("Your choice: ");
            int choice3 = int.Parse(Console.ReadLine());

            //Edit subject
            if (choice3 == 1)
            {
                Console.WriteLine("Provide following information to edit subject:");
                Console.Write("Current subject name: ");
                string currentSubjectName = Console.ReadLine();
                var subject = dbContext.Subjects.First(s => s.SubjectName == currentSubjectName);
                Console.Write("New subject name: ");
                subject.SubjectName = Console.ReadLine();
                dbContext.Subjects.Update(subject);
            }

            //Delete subject
            else if (choice3 == 2)
            {
                Console.WriteLine("Provide following information to delete subject:");
                Console.Write("Subject name: ");
                string subjectName2 = Console.ReadLine();
                var subject = dbContext.Subjects.First(s => s.SubjectName == subjectName2);
                dbContext.Subjects.Remove(subject);
            }

            //Assign a teacher
            else if (choice3 == 3)
            {
                Console.WriteLine("Provide following information to assign a teacher:");
                Console.Write("Class name: ");
                string className = Console.ReadLine();
                var selectedClass = dbContext.Classes.First(c => c.ClassName == className);
                Console.Write("Subject name: ");
                string subjectName2 = Console.ReadLine();
                var subject = dbContext.Subjects.First(s => s.SubjectName == subjectName2);
                Console.Write("Teacher name: ");
                string teacherName = Console.ReadLine();
                subject.TeacherName = teacherName;
                dbContext.Subjects.Update(subject);
            }

            break;

        case 6:
            Console.WriteLine("Following teachers are present in the system:");
            dbContext.Teachers.ToList()
                .ForEach(t => Console.WriteLine(t.TeacherName));
            break;
            
    }
    dbContext.SaveChanges();
}

// ===== Helper =====
static SchoolClass GetClassByName(TrainingDbContext dbContext)
{
    Console.Write("Current Class name: ");
    string name = Console.ReadLine();
    return dbContext.Classes.First(c => c.ClassName == name);
    
}



// ================= TEACHER =================
static void TeacherMenu(TrainingDbContext dbContext)
{
    Console.WriteLine();
    Console.WriteLine("=============== Teacher Menu ====================");
    Console.WriteLine("1) View grades");
    Console.WriteLine("2) Insert grades");
    Console.WriteLine("3) Add students");

    int choice = int.Parse(Console.ReadLine());

    if (choice == 3)
    {
        var cls = GetClassByName(dbContext);

        Console.Write("Student name: ");
        string studentName = Console.ReadLine();

        dbContext.Students.Add(new Student
        {
            StudentName = studentName,
            ClassId = cls.Id
        });
       
    }

    else if (choice == 2)
    {
        var cls = GetClassByName(dbContext);

        Console.Write("Subject name: ");
        string subjectName = Console.ReadLine();

        Console.Write("Student name: ");
        string studentName = Console.ReadLine();

        Console.Write("Term name (1st, mid, final): ");
        string term = Console.ReadLine();

        Console.Write("Grade (0.00 to 5.00): ");
        double mark = double.Parse(Console.ReadLine());

        var student = dbContext.Students.First(s => s.StudentName == studentName);
        var subject = dbContext.Subjects.First(s => s.SubjectName == subjectName);

        dbContext.Grades.Add(new Grade
        {
            studentId = student.Id,
            subjectId = subject.Id,
            Term = term,
            Mark = mark
        });
       
    }

    else if (choice == 1)
    {
        var cls = GetClassByName(dbContext);

        var students = dbContext.Students
            .Where(s => s.ClassId == cls.Id)
            .ToList();

        Console.WriteLine($"Showing grades for - {cls.ClassName}:");
        Console.WriteLine("Name\t1st\tMid\tFinal");

        foreach (var student in students)
        {
            var grades = dbContext.Grades
                .Where(g => g.studentId == student.Id)
                .ToList();

            double first = grades.FirstOrDefault(g => g.Term == "1st")?.Mark ?? 0;
            double mid = grades.FirstOrDefault(g => g.Term == "mid")?.Mark ?? 0;
            double final = grades.FirstOrDefault(g => g.Term == "final")?.Mark ?? 0;

            Console.WriteLine($"{student.StudentName}\t{first}\t{mid}\t{final}");
            
        }
    }
    dbContext.SaveChanges();

}

