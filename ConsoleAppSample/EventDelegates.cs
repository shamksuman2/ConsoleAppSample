using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample
{


    class Worker
    {
        //public delegate int WorkPerformedHandler(object sender, WorkPerformedEventArgs e);

        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;
        public event EventHandler WorkCompleted;

        public void DoWork(int hour, WorkType workType)
        {
            for (int i = 0; i < hour; i++)
            {
                System.Threading.Thread.Sleep(1000);
                OnWorkPerformed(i + 1, WorkType.GoToMeeting);

            }

            OnWorkCompleted();
        }

        protected virtual void OnWorkPerformed(int hour, WorkType workType)
        {

            if (WorkPerformed is EventHandler<WorkPerformedEventArgs> del)
            {
                del(this, new WorkPerformedEventArgs(hour, workType));
            }
        }

        protected virtual void OnWorkCompleted()
        {
            if (WorkCompleted is EventHandler del)
            {
                del(this, EventArgs.Empty);
            }
        }


        public static void Worker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine(e.Hours + ' ' + e.WorkType);
        }

        public static void Worker_WorkCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Workder is done.");
        }
        //static void DoWork(WorkPerformedHandler del)
        //{
        //    del(5, WorkType.GoToMeeting);
        //}

        //static void workPerformed1(int hours, WorkType workType) { Console.Write("Work1");}
        //static void workPerformed2(int hours, WorkType workType) { Console.Write("Work2"); }
        //static void workPerformed3(int hours, WorkType workType) { Console.Write("Work3"); }


    }

    public enum WorkType
    {
        GoToMeeting,
        Golf,
        GenerateReprots
    }

    public class WorkPerformedEventArgs : EventArgs
    {
        public WorkPerformedEventArgs(int hours, WorkType workType)
        {
            Hours = hours;
            WorkType = workType;
        }
        public int Hours { get; set; }

        public WorkType WorkType { get; set; }
    }
}
