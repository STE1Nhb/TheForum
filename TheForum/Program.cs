using System.Runtime.InteropServices;

namespace TheForum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Forum forum = new Forum("The Greatest Forum All Over The World"); // Forum declaration

            // Primary forum statistics
            Console.WriteLine($"Statistics right after creation \"{forum.ForumName}\" forum:\n");
            Console.WriteLine($"Amount of questions - {forum.Statistics.QAmount}\nAmount of replies - {forum.Statistics.RAmount}");
            Console.WriteLine($"Average amount of replies - {forum.Statistics.RAVG}\nAmount of questions without replies - {forum.Statistics.RNone}");
            Console.WriteLine($"Amount of questions with at least one reply - {forum.Statistics.QAmount}\n");

            // Users declaration
            User x = new User("guru1337", forum); 
            User y = new User("SayMyName", forum);
            User z = new User("Mr.Indifference", forum);

            // Subscribing users on the forum
            forum.NewContent += x.OnNewContent;
            forum.NewContent += y.OnNewContent;
            forum.NewContent += z.OnNewContent;

            Console.WriteLine("\n#######################\n");
            // Setting up user notification type
            // On default user recieve all notifications
            y.ChangeNotificationType("personal"); // "personal" setting allows user to recieve notifications only about answers on own questions
            z.ChangeNotificationType("none"); // This user will not recieve any notification
            Console.WriteLine("\n#######################\n");

            // Doing some questions...
            y.CreateQuestion("Hey! Does anybody know the recipe of pancakes?");
            z.CreateQuestion("Yo, I turned off all the notifications }:-> Am I cool enough?");
            x.CreateQuestion("Guys, I need some help to build a REAL rocket. If anybody can help, email me (adress in my profile).");
            Console.WriteLine("\n#######################\n");

            // Doing some answers...
            x.AnswerQuestion(1, "Oh Im a real pancake lover! I can give you a link to a website with recipies (here is a link -> all-pancake-recipies.com).");
            x.AnswerQuestion(2,"Man, it's the worst thing I've ever seen! Do not do such things anymore!");
            y.AnswerQuestion(2, "Wait, what?! How could you do that! What an insensitive person...");
            Console.WriteLine("\n#######################\n");

            // Final forum statistics
            Console.WriteLine($"Statistics after all the actions on \"{forum.ForumName}\" forum:\n");
            Console.WriteLine($"Amount of questions - {forum.Statistics.QAmount}\nAmount of replies - {forum.Statistics.RAmount}");
            Console.WriteLine($"Average amount of replies - {forum.Statistics.RAVG}\nAmount of questions without replies - {forum.Statistics.RNone}");
            Console.WriteLine($"Amount of questions with at least one reply - {forum.Statistics.QAmount}\n");
            Console.WriteLine("\n#######################\n");
            Console.WriteLine("Some statistics that users can access on request:");
            Console.WriteLine("Question and all its replies:\n");
            x.GetQuestion(2);
            Console.WriteLine("List of all the questions on this forum:\n");
            x.ForumQuestions();


        }
    }
}