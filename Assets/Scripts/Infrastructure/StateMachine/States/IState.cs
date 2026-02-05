using System.Threading;
using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachine.States
{ 
    public interface IState
    {
        UniTask Enter(CancellationToken token);
        UniTask Exit(CancellationToken token);
    }
}

