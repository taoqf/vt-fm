
using System.Threading.Tasks;

namespace Victop.Server.Controls.MVVM.Helpers
{
    /// <summary>
    /// Helper class used when an async method is required,
    /// but the context is synchronous.
    /// </summary>
    public static class Empty
    {
        private static readonly Task ConcreteTask = new Task(
            () =>
            {
            });

        /// <summary>
        /// Gets the empty task.
        /// </summary>
        public static Task Task
        {
            get
            {
                return ConcreteTask;
            }
        }
    }
}