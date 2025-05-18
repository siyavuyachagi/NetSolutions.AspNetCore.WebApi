namespace Application.Interfaces
{
    public interface ILogClearner
    {
        /// <summary>
        /// Clean log files
        /// </summary>
        /// <returns></returns>
        Task CleanAsync();
    }
}
