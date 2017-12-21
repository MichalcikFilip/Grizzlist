namespace Grizzlist.Client.BackgroundActions
{
    interface IAction
    {
        bool CanRun();
        void Run();
    }
}
